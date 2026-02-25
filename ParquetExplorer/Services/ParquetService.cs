using System.Data;
using Parquet;
using Parquet.Data;
using Parquet.Schema;

namespace ParquetExplorer.Services
{
    /// <summary>
    /// Reads a Parquet file and returns its content as a <see cref="DataTable"/>.
    /// This service has no dependency on Windows Forms and can be reused by any frontend.
    /// </summary>
    public static class ParquetService
    {
        public static async Task<DataTable> LoadAsync(string filePath)
        {
            var table = new DataTable();

            using var fileStream = File.OpenRead(filePath);
            using var reader = await ParquetReader.CreateAsync(fileStream);
            var schema = reader.Schema;

            foreach (var field in schema.Fields)
            {
                if (field is DataField df)
                {
                    var col = new System.Data.DataColumn(df.Name, Nullable.GetUnderlyingType(df.ClrType) ?? df.ClrType);
                    col.AllowDBNull = true;
                    table.Columns.Add(col);
                }
            }

            for (int rg = 0; rg < reader.RowGroupCount; rg++)
            {
                using var rowGroupReader = reader.OpenRowGroupReader(rg);
                var dataFields = schema.Fields.OfType<DataField>().ToArray();
                var columnData = new (DataField Field, Array Data)[dataFields.Length];

                for (int ci = 0; ci < dataFields.Length; ci++)
                {
                    var dc = await rowGroupReader.ReadColumnAsync(dataFields[ci]);
                    columnData[ci] = (dataFields[ci], (Array)dc.Data);
                }

                int rowCount = columnData.Length > 0 ? columnData[0].Data.Length : 0;
                for (int r = 0; r < rowCount; r++)
                {
                    var row = table.NewRow();
                    for (int ci = 0; ci < columnData.Length; ci++)
                    {
                        var val = columnData[ci].Data.GetValue(r);
                        row[ci] = val ?? DBNull.Value;
                    }
                    table.Rows.Add(row);
                }
            }

            return table;
        }
    }
}
