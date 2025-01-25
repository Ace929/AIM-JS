using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EndOfDayReportTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // For testing, hardcode some values instead of querying a database
            // You can replace these with real SQL queries once you have them
            decimal dailyAUM = 1_234_567.89m;
            decimal mtdAUM = 11_234_567.89m;
            decimal ytdAUM = 111_234_567.89m;

            decimal dailyVolume = 987_654;
            decimal mtdVolume = 1_234_567;
            decimal ytdVolume = 12_345_678;

            // Build HTML email body
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

            sb.AppendLine("<h2>End of Day Financial Summary (Test)</h2>");

            // Example table with multiple columns (Daily, MTD, YTD)
            sb.AppendLine("<table>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th>Metric</th>");
            sb.AppendLine("<th>Daily</th>");
            sb.AppendLine("<th>MTD</th>");
            sb.AppendLine("<th>YTD</th>");
            sb.AppendLine("</tr>");

            // Row: AUM
            sb.AppendLine("<tr>");
            sb.AppendLine("<td>Total AUM</td>");
            sb.AppendLine($"<td>{dailyAUM:C}</td>");
            sb.AppendLine($"<td>{mtdAUM:C}</td>");
            sb.AppendLine($"<td>{ytdAUM:C}</td>");
            sb.AppendLine("</tr>");

            // Row: Volume
            sb.AppendLine("<tr>");
            sb.AppendLine("<td>Total Volume</td>");
            sb.AppendLine($"<td>{dailyVolume:N0}</td>");
            sb.AppendLine($"<td>{mtdVolume:N0}</td>");
            sb.AppendLine($"<td>{ytdVolume:N0}</td>");
            sb.AppendLine("</tr>");

            // Add more rows/columns as needed for other metrics...
            sb.AppendLine("</table>");

            // Date/Time Stamp
            sb.AppendLine("<br/>");
            sb.AppendLine($"<p>Report generated on: {DateTime.Now:dd-MMM-yyyy HH:mm:ss}</p>");

            // Add an HTML "signature" section
            sb.AppendLine("<br/>");
            sb.AppendLine("<p>Best regards,</p>");
            sb.AppendLine("<p><strong>The EOD Report Team</strong><br/>");
            sb.AppendLine("Your Company, Inc.<br/>");
            sb.AppendLine("<em>Phone: (555) 123-4567</em><br/>");
            sb.AppendLine("<em>Email: eod.reports@yourcompany.com</em></p>");

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            string emailBody = sb.ToString();

            // SMTP info (adjust to your environment)
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "ifpicswerefunny@gmail.com";
            string smtpPassword = "jfwf ihvr kjjm zlev";

            // Create the mail message
            MailMessage mail = new MailMessage
            {
                From = new MailAddress("eod.reports@yourcompany.com", "EOD Reports"),
                Subject = $"End of Day Report - {DateTime.Now:yyyy-MM-dd}",
                Body = emailBody,
                IsBodyHtml = true
            };

            // Add recipients (use your own email for testing)
            mail.To.Add("sealsgaming29@gmail.com");

            // Send the email
            try
            {
                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true; // set to true if required by your SMTP server
                    smtpClient.Send(mail);
                }
                Console.WriteLine("Test report email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
