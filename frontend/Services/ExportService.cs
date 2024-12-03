using System.Reflection;
using System.Security.AccessControl;
using OfficeOpenXml;

namespace Placement_Preparation.Services{
  public  class ExportService{
        public byte[] ExportToExcel<T>(IEnumerable<T> data , string filename){
            if (data == null || !data.Any())
            throw new ArgumentException("Data collection cannot be null or empty.");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // For non-commercial use
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("sheet1");

            // Use reflection to get properties
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Add header row
            for (int col = 0; col < properties.Length; col++)
            {
                worksheet.Cells[1, col + 1].Value = properties[col].Name; // Property name as header
                worksheet.Cells[1, col + 1].Style.Font.Bold = true;      // Make headers bold
            }

            // Add data rows
            int row = 2;
            foreach (var item in data)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    var value = properties[col].GetValue(item);           // Get property value
                    worksheet.Cells[row, col + 1].Value = value;         // Set cell value
                }
                row++;
            }

            // Auto-fit columns for better readability
            worksheet.Cells.AutoFitColumns();

            return package.GetAsByteArray();
        }

        public byte[] ExportToExcelBySpecificColumn(List<Dictionary<string,dynamic>> data, string[] columns)
        {
            if (data == null || !data.Any())
                throw new ArgumentException("Data collection cannot be null or empty.");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // For non-commercial use
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("sheet1");

            // Add header row
            for (int col = 0; col < columns.Length; col++)
            {
                worksheet.Cells[1, col + 1].Value = columns[col]; // Property name as header
                worksheet.Cells[1, col + 1].Style.Font.Bold = true; // Make headers bold
            }

            // Add data rows
            int row = 2;
            foreach (Dictionary<string,dynamic> item in data as List<Dictionary<string,dynamic>>)
            {
                for (int col = 0; col < columns.Length; col++)
                {
                    var p2 = columns[col];
                    var p3 = item[p2];
                    var value = item[columns[col]];
                    if (value == null)
                    {
                        worksheet.Cells[row, col + 1].Value = "N/A"; // Handle missing properties
                        continue;
                    }

                    worksheet.Cells[row, col + 1].Value = value is DateTime dateTime
                        ? dateTime.ToString("yyyy-MM-dd") // Example formatting
                        : value?.ToString(); // Convert other types to string
                }
                row++;
            }

            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();

            return package.GetAsByteArray();
        }

    }
}