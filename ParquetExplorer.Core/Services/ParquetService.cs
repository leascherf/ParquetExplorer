using System.Data;
using Parquet;
using Parquet.Data;
using Parquet.Schema;
using ParquetExplorer.Services.Interfaces;

namespace ParquetExplorer.Services
{
    /// <summary>
    /// Reads a Parquet file and returns its content as a <see cref="DataTable"/>.
    /// This service has no dependency on Windows Forms and can be reused by any frontend.
    /// </summary>
    public class ParquetService : IParquetService
    {
        public async Task<DataTable> LoadAsync(string filePath)
        {
            var table = new DataTable();

            using var fileStream = File.OpenRead(filePath);
            using var reader = await ParquetReader.CreateAsync(fileStream).ConfigureAwait(false);
            var schema = reader.Schema;
            var dataFields = schema.DataFields;

            foreach (var df in dataFields)
            {
                var clrType = Nullable.GetUnderlyingType(df.ClrType) ?? df.ClrType;
                Type columnType;
                try
                {
                    // Verify DataColumn supports this CLR type; fall back to string if not.
                    _ = new System.Data.DataColumn("__probe__", clrType);
                    columnType = clrType;
                }
                catch
                {
                    columnType = typeof(string);
                }
                var col = new System.Data.DataColumn(df.Name, columnType);
                col.AllowDBNull = true;
                table.Columns.Add(col);
            }

            for (int rg = 0; rg < reader.RowGroupCount; rg++)
            {
                using var rowGroupReader = reader.OpenRowGroupReader(rg);
                var columnData = new (DataField Field, Array? Data)[dataFields.Length];

                for (int ci = 0; ci < dataFields.Length; ci++)
                {
                    try
                    {
                        var dc = await rowGroupReader.ReadColumnAsync(dataFields[ci]).ConfigureAwait(false);
                        columnData[ci] = (dataFields[ci], (Array)dc.Data);
                    }
                    catch
                    {
                        // Column could not be read; leave Data as null and fill with DBNull below.
                        columnData[ci] = (dataFields[ci], null);
                    }
                }

                // Determine row count from the shortest successfully-read column to stay in bounds.
                int rowCount = columnData
                    .Where(c => c.Data != null)
                    .Select(c => c.Data!.Length)
                    .DefaultIfEmpty(0)
                    .Min();

                for (int r = 0; r < rowCount; r++)
                {
                    var values = new object[columnData.Length];
                    for (int ci = 0; ci < columnData.Length; ci++)
                    {
                        var data = columnData[ci].Data;
                        values[ci] = (data != null && r < data.Length) ? data.GetValue(r) ?? DBNull.Value : DBNull.Value;
                    }
                    table.Rows.Add(values);
                }
            }

            return table;
        }
    }
}
