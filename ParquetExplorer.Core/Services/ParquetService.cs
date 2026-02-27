using System.Data;
using Parquet;
using Parquet.Data;
using Parquet.Schema;
using ParquetExplorer.Services.Interfaces;

namespace ParquetExplorer.Services
{
    /// <summary>
    /// Reads a Parquet or delimited text file and returns its content as a
    /// <see cref="DataTable"/>.  The file type is detected from the extension:
    /// <c>.parquet</c> → Parquet format, <c>.csv</c> → comma-delimited,
    /// <c>.tsv</c> → tab-delimited, <c>.txt</c> → auto-detect delimiter (comma then tab).
    /// This service has no dependency on Windows Forms and can be reused by any frontend.
    /// </summary>
    public class ParquetService : IParquetService
    {
        public async Task<DataTable> LoadAsync(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            if (ext == ".csv" || ext == ".tsv" || ext == ".txt")
                return await Task.Run(() => LoadTextFile(filePath)).ConfigureAwait(false);

            return await LoadParquetAsync(filePath).ConfigureAwait(false);
        }

        // ── Text-file loading ─────────────────────────────────────────────────

        private static DataTable LoadTextFile(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLowerInvariant();

            // Detect delimiter: TSV is always tab; for CSV/TXT sniff the first line.
            char delimiter;
            if (ext == ".tsv")
            {
                delimiter = '\t';
            }
            else
            {
                var firstLine = File.ReadLines(filePath).FirstOrDefault() ?? "";
                delimiter = firstLine.Contains('\t') ? '\t' : ',';
            }

            var table = new DataTable();
            using var reader = new StreamReader(filePath);

            // ── Header row ───────────────────────────────────────────────────
            string? headerLine = reader.ReadLine();
            if (headerLine == null) return table;

            var headers = SplitCsvLine(headerLine, delimiter);
            for (int hi = 0; hi < headers.Count; hi++)
            {
                string h = headers[hi];
                table.Columns.Add(string.IsNullOrWhiteSpace(h) ? $"Col{hi + 1}" : h.Trim('"'));
            }

            // ── Data rows ────────────────────────────────────────────────────
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var fields = SplitCsvLine(line, delimiter);
                var row = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++)
                    row[i] = i < fields.Count ? fields[i].Trim('"') : string.Empty;
                table.Rows.Add(row);
            }

            return table;
        }

        /// <summary>
        /// Splits a single CSV/TSV line into fields, respecting double-quoted fields
        /// that may contain the delimiter character or newlines.
        /// </summary>
        private static List<string> SplitCsvLine(string line, char delimiter)
        {
            var fields = new List<string>();
            var current = new System.Text.StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (inQuotes)
                {
                    if (c == '"')
                    {
                        // Escaped quote inside a quoted field ("").
                        if (i + 1 < line.Length && line[i + 1] == '"')
                        {
                            current.Append('"');
                            i++; // skip second "
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        current.Append(c);
                    }
                }
                else
                {
                    if (c == '"')
                    {
                        inQuotes = true;
                    }
                    else if (c == delimiter)
                    {
                        fields.Add(current.ToString());
                        current.Clear();
                    }
                    else
                    {
                        current.Append(c);
                    }
                }
            }

            fields.Add(current.ToString());
            return fields;
        }

        // ── Parquet loading ───────────────────────────────────────────────────

        private async Task<DataTable> LoadParquetAsync(string filePath)
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
