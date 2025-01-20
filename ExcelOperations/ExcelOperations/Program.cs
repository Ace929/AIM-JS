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
        Console.WriteLine("4 - Transpose");
        Console.WriteLine("5 - Filter rows by value");
        Console.WriteLine("6 - Sort data");
        Console.WriteLine("7 - Add row summaries");
        Console.WriteLine("8 - Find and Replace");
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
            case "4":
                return TransposeData(inputFilePath);
            case "5":
                return FilterRowsByValue(inputFilePath, column: 2, threshold: 1000); // Example
            case "6":
                return SortData(inputFilePath, column: 3, ascending: false);
            case "7":
                return AddRowSummaries(inputFilePath);
            case "8":
                return FindAndReplace(inputFilePath, "OldValue", "NewValue");

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

    static string TransposeData(string filePath)
    {
        try
        {
            string outputDirectory = Path.Combine(Path.GetDirectoryName(filePath), "ProcessedFiles");
            Directory.CreateDirectory(outputDirectory);

            string outputFilePath = Path.Combine(outputDirectory, $"Transposed_{Path.GetFileName(filePath)}");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var transposedWorksheet = package.Workbook.Worksheets.Add("Transposed");

                int totalRows = worksheet.Dimension.Rows;
                int totalColumns = worksheet.Dimension.Columns;

                for (int row = 1; row <= totalRows; row++)
                {
                    for (int col = 1; col <= totalColumns; col++)
                    {
                        transposedWorksheet.Cells[col, row].Value = worksheet.Cells[row, col].Value;
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
    static string FilterRowsByValue(string filePath, int column, double threshold)
    {
        try
        {
            string outputDirectory = Path.Combine(Path.GetDirectoryName(filePath), "ProcessedFiles");
            Directory.CreateDirectory(outputDirectory);

            string outputFilePath = Path.Combine(outputDirectory, $"Filtered_{Path.GetFileName(filePath)}");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var filteredWorksheet = package.Workbook.Worksheets.Add("Filtered");

                int totalRows = worksheet.Dimension.Rows;
                int totalColumns = worksheet.Dimension.Columns;

                int newRow = 1;
                for (int row = 1; row <= totalRows; row++)
                {
                    if (row == 1 || (worksheet.Cells[row, column].Value != null &&
                        double.TryParse(worksheet.Cells[row, column].Value.ToString(), out double value) && value > threshold))
                    {
                        for (int col = 1; col <= totalColumns; col++)
                        {
                            filteredWorksheet.Cells[newRow, col].Value = worksheet.Cells[row, col].Value;
                        }
                        newRow++;
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

    static string SortData(string filePath, int column, bool ascending = true)
    {
        try
        {
            string outputDirectory = Path.Combine(Path.GetDirectoryName(filePath), "ProcessedFiles");
            Directory.CreateDirectory(outputDirectory);

            string outputFilePath = Path.Combine(outputDirectory, $"Sorted_{Path.GetFileName(filePath)}");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int totalRows = worksheet.Dimension.Rows;
                int totalColumns = worksheet.Dimension.Columns;

                // Extract data into a list for sorting
                var data = new List<object[]>();
                for (int row = 2; row <= totalRows; row++) // Skip header
                {
                    var rowData = new object[totalColumns];
                    for (int col = 1; col <= totalColumns; col++)
                    {
                        rowData[col - 1] = worksheet.Cells[row, col].Value;
                    }
                    data.Add(rowData);
                }

                // Sort data
                data = ascending
                    ? data.OrderBy(row => row[column - 1]).ToList()
                    : data.OrderByDescending(row => row[column - 1]).ToList();

                // Write back sorted data
                for (int row = 2; row <= totalRows; row++)
                {
                    var rowData = data[row - 2];
                    for (int col = 1; col <= totalColumns; col++)
                    {
                        worksheet.Cells[row, col].Value = rowData[col - 1];
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

    static string AddRowSummaries(string filePath)
    {
        try
        {
            string outputDirectory = Path.Combine(Path.GetDirectoryName(filePath), "ProcessedFiles");
            Directory.CreateDirectory(outputDirectory);

            string outputFilePath = Path.Combine(outputDirectory, $"RowSummaries_{Path.GetFileName(filePath)}");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int totalRows = worksheet.Dimension.Rows;
                int totalColumns = worksheet.Dimension.Columns;

                worksheet.Cells[1, totalColumns + 1].Value = "Row Sum"; // Add header for the new column

                for (int row = 2; row <= totalRows; row++)
                {
                    double rowSum = 0;
                    for (int col = 1; col <= totalColumns; col++)
                    {
                        if (worksheet.Cells[row, col].Value != null &&
                            double.TryParse(worksheet.Cells[row, col].Value.ToString(), out double value))
                        {
                            rowSum += value;
                        }
                    }
                    worksheet.Cells[row, totalColumns + 1].Value = rowSum; // Add sum to the new column
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

    static string FindAndReplace(string filePath, string findValue, string replaceValue)
    {
        try
        {
            string outputDirectory = Path.Combine(Path.GetDirectoryName(filePath), "ProcessedFiles");
            Directory.CreateDirectory(outputDirectory);

            string outputFilePath = Path.Combine(outputDirectory, $"Replaced_{Path.GetFileName(filePath)}");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int totalRows = worksheet.Dimension.Rows;
                int totalColumns = worksheet.Dimension.Columns;

                for (int row = 1; row <= totalRows; row++)
                {
                    for (int col = 1; col <= totalColumns; col++)
                    {
                        if (worksheet.Cells[row, col].Value != null &&
                            worksheet.Cells[row, col].Value.ToString() == findValue)
                        {
                            worksheet.Cells[row, col].Value = replaceValue;
                        }
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
