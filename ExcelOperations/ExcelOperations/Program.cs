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

        // Show the menu and perform operations
        while (true)
        {
            ShowMenu();
            string choice = Console.ReadLine();

            if (choice == "0")
            {
                Console.WriteLine("Exiting the application. Goodbye!");
                break;
            }

            string outputFilePath = PerformOperation(choice, inputFilePath);

            if (!string.IsNullOrEmpty(outputFilePath))
            {
                Console.WriteLine($"Operation complete! The modified file is saved at: {outputFilePath}");
            }
            else
            {
                Console.WriteLine("Operation failed or canceled.");
            }
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

    static void ShowMenu()
    {
        Console.WriteLine("\nPlease choose an operation to perform on the Excel file:");
        Console.WriteLine("1 - Add a new column with calculated values");
        Console.WriteLine("2 - Remove empty rows");
        Console.WriteLine("3 - Count the number of rows and columns");
        Console.WriteLine("0 - Exit");
        Console.Write("Enter your choice: ");
    }

    static string PerformOperation(string choice, string inputFilePath)
    {
        switch (choice)
        {
            case "1":
                return AddCalculatedColumn(inputFilePath);
            case "2":
                return RemoveEmptyRows(inputFilePath);
            case "3":
                CountRowsAndColumns(inputFilePath);
                return null;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                return null;
        }
    }

    static string AddCalculatedColumn(string filePath)
    {
        try
        {
            string outputDirectory = Path.Combine(Path.GetDirectoryName(filePath), "ProcessedFiles");
            Directory.CreateDirectory(outputDirectory);

            string outputFilePath = Path.Combine(outputDirectory, $"Calculated_{Path.GetFileName(filePath)}");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];

                int totalRows = worksheet.Dimension.Rows;
                int totalColumns = worksheet.Dimension.Columns;

                worksheet.Cells[1, totalColumns + 1].Value = "Calculated Column";

                for (int row = 2; row <= totalRows; row++)
                {
                    if (worksheet.Cells[row, 1].Value != null && double.TryParse(worksheet.Cells[row, 1].Value.ToString(), out double value))
                    {
                        worksheet.Cells[row, totalColumns + 1].Value = value * 2; // Example: Double the value
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

    static string RemoveEmptyRows(string filePath)
    {
        try
        {
            string outputDirectory = Path.Combine(Path.GetDirectoryName(filePath), "ProcessedFiles");
            Directory.CreateDirectory(outputDirectory);

            string outputFilePath = Path.Combine(outputDirectory, $"Cleaned_{Path.GetFileName(filePath)}");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int totalRows = worksheet.Dimension.Rows;

                for (int row = totalRows; row >= 2; row--) // Iterate backward to safely delete rows
                {
                    bool isEmpty = true;
                    for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                    {
                        if (worksheet.Cells[row, col].Value != null && !string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Value.ToString()))
                        {
                            isEmpty = false;
                            break;
                        }
                    }

                    if (isEmpty)
                    {
                        worksheet.DeleteRow(row);
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

    static void CountRowsAndColumns(string filePath)
    {
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];

                int totalRows = worksheet.Dimension.Rows;
                int totalColumns = worksheet.Dimension.Columns;

                Console.WriteLine($"The file has {totalRows} rows and {totalColumns} columns.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
