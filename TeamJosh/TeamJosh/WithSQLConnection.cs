/*
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EndOfDayReportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Fetch data from the database
            string connectionString = "Server=YourServer;Database=YourDatabase;User Id=YourUsername;Password=YourPassword;";
            decimal totalAUM = 0;
            decimal totalVolume = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Example query for AUM
                    using (SqlCommand cmd = new SqlCommand("SELECT SUM(AUM) FROM YourAUMTable", conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            totalAUM = Convert.ToDecimal(result);
                        }
                    }

                    // Example query for Volume
                    using (SqlCommand cmd = new SqlCommand("SELECT SUM(Volume) FROM YourVolumeTable", conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            totalVolume = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching data: " + ex.Message);
                // Handle or log error as needed
            }

            // 2. Build HTML email body
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<style>");
            sb.AppendLine("table { border-collapse: collapse; font-family: Arial, sans-serif; }");
            sb.AppendLine("th, td { border: 1px solid #dddddd; text-align: left; padding: 8px; }");
            sb.AppendLine("th { background-color: #f2f2f2; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<h2>End of Day Financial Summary</h2>");
            sb.AppendLine("<table>");
            sb.AppendLine("<tr><th>Metric</th><th>Value</th></tr>");
            sb.AppendLine($"<tr><td>Total AUM</td><td>{totalAUM:C}</td></tr>");
            sb.AppendLine($"<tr><td>Total Volume</td><td>{totalVolume:N0}</td></tr>");
            // Add more rows as needed
            sb.AppendLine("</table>");
            sb.AppendLine("<br/>");
            sb.AppendLine($"<p>Report generated on: {DateTime.Now:dd-MMM-yyyy HH:mm:ss}</p>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            string emailBody = sb.ToString();

            // 3. Send the email
            string smtpServer = "smtp.example.com";
            int smtpPort = 587; // or your SMTP port
            string smtpUsername = "yourSMTPUsername";
            string smtpPassword = "yourSMTPPassword";

            // Create the mail message
            MailMessage mail = new MailMessage
            {
                From = new MailAddress("reports@yourcompany.com", "EOD Reports"),
                Subject = $"End of Day Report - {DateTime.Now:yyyy-MM-dd}",
                Body = emailBody,
                IsBodyHtml = true
            };

            // Add recipients
            mail.To.Add("recipient@theircompany.com");

            // Optionally, add CC or BCC
            // mail.CC.Add("someone_else@yourcompany.com");

            // Configure SMTP client and send
            try
            {
                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true; // Set to true if your SMTP server requires SSL
                    smtpClient.Send(mail);
                }

                Console.WriteLine("End of Day Report email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
                // Handle or log error as needed
            }
        }
    }
}
*/