using System;
using System.IO;
using System.Windows.Forms;
using OfficeOpenXml;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Excel File Processor!");

        // Prompt the user to select an Excel file
        string inputFilePath = SelectExcelFile();

        if (string.IsNullOrEmpty(inputFilePath) || !File.Exists(inputFilePath))
        {
            Console.WriteLine("Invalid file selected. Please restart the program and try again.");
            return;
        }

        // Process the Excel file
        Console.WriteLine("Processing the file...");
        string outputFilePath = ProcessExcelFile(inputFilePath);

        if (!string.IsNullOrEmpty(outputFilePath))
        {
            Console.WriteLine($"Processing complete! The modified file is saved at: {outputFilePath}");
        }
        else
        {
            Console.WriteLine("An error occurred during file processing.");
        }
    }

    static string SelectExcelFile()
    {
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
            openFileDialog.Title = "Select an Excel File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
        }

        return null;
    }

    static string ProcessExcelFile(string filePath)
    {
        try
        {
            string outputDirectory = Path.Combine(Path.GetDirectoryName(filePath), "ProcessedFiles");
            Directory.CreateDirectory(outputDirectory);

            string outputFilePath = Path.Combine(outputDirectory, $"Processed_{Path.GetFileName(filePath)}");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];

                int totalRows = worksheet.Dimension.Rows;
                int totalColumns = worksheet.Dimension.Columns;

                worksheet.Cells[1, totalColumns + 1].Value = "Processed Column";

                for (int row = 2; row <= totalRows; row++)
                {
                    if (worksheet.Cells[row, 1].Value != null && double.TryParse(worksheet.Cells[row, 1].Value.ToString(), out double value))
                    {
                        worksheet.Cells[row, totalColumns + 1].Value = value + 100;
                    }
                }

                package.SaveAs(new FileInfo(outputFilePath));
            }

            return outputFilePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }
}
