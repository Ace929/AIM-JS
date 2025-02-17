using System;
using System.Collections.Generic;
using System.IO;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout.Font;

namespace ETFReportGenerator
{
    // Data model for an ETF Transaction.
    public class ETFTransaction
    {
        public DateTime TransactionDate { get; set; }
        public string Ticker { get; set; }
        public int Shares { get; set; }
        public decimal Price { get; set; }
    }

    // Data model for the overall ETF operations report.
    public class ETFOperationsReportData
    {
        public string ReportTitle { get; set; }
        public DateTime ReportDate { get; set; }
        public string ETFName { get; set; }
        public List<ETFTransaction> Transactions { get; set; }
    }

    // Interface to abstract the report generator.
    public interface IReportGenerator
    {
        /// <summary>
        /// Generates a report based on the provided data and saves it to the given output path.
        /// </summary>
        void GenerateReport(ETFOperationsReportData data, string outputPath);
    }

    // Static class to generate HTML content for the report.
    public static class ReportTemplate
    {
        public static string GenerateHtml(ETFOperationsReportData data)
        {
            // Build table rows dynamically based on transactions.
            string transactionsHtml = "";
            foreach (var transaction in data.Transactions)
            {
                transactionsHtml += $@"
                    <tr>
                        <td>{transaction.TransactionDate:yyyy-MM-dd}</td>
                        <td>{transaction.Ticker}</td>
                        <td>{transaction.Shares}</td>
                        <td>{transaction.Price:C}</td>
                    </tr>";
            }

            // HTML template with inline CSS.
            string html = $@"
            <html>
                <head>
                    <meta charset='UTF-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; margin: 20px; }}
                        h1, h2 {{ color: #003366; }}
                        table {{ width: 100%; border-collapse: collapse; margin-top: 20px; }}
                        th, td {{ border: 1px solid #cccccc; padding: 8px; text-align: left; }}
                        th {{ background-color: #f2f2f2; }}
                    </style>
                </head>
                <body>
                    <h1>{data.ReportTitle}</h1>
                    <h2>{data.ETFName}</h2>
                    <p>Report Date: {data.ReportDate:yyyy-MM-dd}</p>
                    <table>
                        <thead>
                            <tr>
                                <th>Transaction Date</th>
                                <th>Ticker</th>
                                <th>Shares</th>
                                <th>Price</th>
                            </tr>
                        </thead>
                        <tbody>
                            {transactionsHtml}
                        </tbody>
                    </table>
                </body>
            </html>";
            return html;
        }
    }

    // Implementation of the PDF report generator using iText7.
    public class PdfReportGenerator : IReportGenerator
    {
        public void GenerateReport(ETFOperationsReportData data, string outputPath)
        {
            // Generate the HTML content for the report.
            string htmlContent = ReportTemplate.GenerateHtml(data);

            // Ensure the output directory exists.
            string directory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Set up converter properties (base URI and fonts).
            ConverterProperties converterProperties = new ConverterProperties();
            converterProperties.SetBaseUri(directory);

            var fontProvider = new FontProvider();
            fontProvider.AddSystemFonts();
            converterProperties.SetFontProvider(fontProvider);

            // Create the PDF document.
            using (PdfWriter writer = new PdfWriter(outputPath))
            using (PdfDocument pdfDoc = new PdfDocument(writer))
            {
                HtmlConverter.ConvertToPdf(htmlContent, pdfDoc, converterProperties);
            }

            Console.WriteLine($"Report generated successfully: {outputPath}");
        }
    }

    // Main program to demonstrate the robust report generation.
    class Program
    {
        static void Main(string[] args)
        {
            // Sample ETF operations data.
            ETFOperationsReportData reportData = new ETFOperationsReportData
            {
                ReportTitle = "ETF Operations Report",
                ReportDate = DateTime.Now,
                ETFName = "Global Equity ETF",
                Transactions = new List<ETFTransaction>
                {
                    new ETFTransaction { TransactionDate = new DateTime(2025, 1, 10), Ticker = "AAPL", Shares = 50, Price = 150.25m },
                    new ETFTransaction { TransactionDate = new DateTime(2025, 1, 12), Ticker = "GOOGL", Shares = 10, Price = 2750.50m },
                    new ETFTransaction { TransactionDate = new DateTime(2025, 1, 15), Ticker = "MSFT", Shares = 20, Price = 300.75m }
                }
            };

            // Define the folder where reports will be stored.
            string folderPath = @"C:\Users\seali\OneDrive\Documents\GitHub\AIM-JS\html-to-pdf\Reports";
            // Combine the folder path with the report file name.
            string pdfFilePath = Path.Combine(folderPath, "ETFOperationsReport.pdf");

            // Create an instance of the PDF report generator.
            IReportGenerator reportGenerator = new PdfReportGenerator();

            // Generate the report and handle any exceptions.
            try
            {
                reportGenerator.GenerateReport(reportData, pdfFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while generating the report:");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
