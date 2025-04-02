using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionApp4
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("Function1")]
        [BlobOutput("output/{name}", Connection = "")]
        public async Task<string> Run(
            [BlobTrigger("input/{name}", Connection = "")] string inputBlob,
            string name)
        {
            _logger.LogInformation($"Function triggered for CSV blob: {name}");

            if (string.IsNullOrWhiteSpace(inputBlob))
            {
                _logger.LogWarning("Input blob is empty or null.");
                return inputBlob;
            }

            // Split CSV content into lines (handling both Windows and Unix line breaks)
            var lines = inputBlob.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0)
            {
                _logger.LogWarning("CSV file contains no data.");
                return inputBlob;
            }

            // Assume the first line is the header
            var headerFields = lines[0].Split(',').Select(field => field.Trim()).ToArray();
            int columnCount = headerFields.Length;
            _logger.LogInformation($"CSV header has {columnCount} columns: {string.Join(", ", headerFields)}");

            // Process and clean each row: trim whitespace from each field and ignore rows that are completely empty.
            var cleanedRows = lines.Select((line, index) =>
            {
                var fields = line.Split(',').Select(field => field.Trim()).ToArray();
                return new { Index = index, Fields = fields };
            })
            .Where(x => x.Fields.Any(field => !string.IsNullOrWhiteSpace(field))) // filter out completely empty rows
            .ToList();

            // Perform EDA: count data rows (excluding header) and missing values per column.
            int dataRowCount = cleanedRows.Count - 1; // assuming first row is header
            _logger.LogInformation($"Total data rows after cleaning (excluding header): {dataRowCount}");

            int[] missingCounts = new int[columnCount];
            // Start from 1 to skip header row
            for (int i = 1; i < cleanedRows.Count; i++)
            {
                var fields = cleanedRows[i].Fields;
                for (int j = 0; j < columnCount && j < fields.Length; j++)
                {
                    if (string.IsNullOrEmpty(fields[j]))
                    {
                        missingCounts[j]++;
                    }
                }
            }

            for (int j = 0; j < columnCount; j++)
            {
                _logger.LogInformation($"Column '{headerFields[j]}' has {missingCounts[j]} missing values.");
            }

            // Reassemble the cleaned CSV content.
            var cleanedCsv = string.Join("\n", cleanedRows.Select(row => string.Join(",", row.Fields)));

            _logger.LogInformation("CSV cleaning and EDA complete. Returning cleaned CSV content.");
            return await Task.FromResult(cleanedCsv);
        }
    }
}
