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
            // Sample data - replace with data from your database/queries
            decimal dailyAUM = 1_234_567.89m;
            decimal mtdAUM = 11_234_567.89m;
            decimal ytdAUM = 111_234_567.89m;

            decimal dailyVolume = 987_654;
            decimal mtdVolume = 1_234_567;
            decimal ytdVolume = 12_345_678;

            // Build the enhanced HTML email body
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("  <meta charset=\"UTF-8\">");
            sb.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"/>");
            sb.AppendLine("  <style>");
            sb.AppendLine("    body { margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f5f7fa; }");
            sb.AppendLine("    .container { width: 100%; max-width: 700px; margin: 0 auto; background-color: #ffffff; box-shadow: 0 0 10px rgba(0,0,0,0.1); }");
            sb.AppendLine("    .header { display: flex; align-items: center; padding: 20px; background-color: #2c3e50; color: #ffffff; }");
            sb.AppendLine("    .header img { max-height: 60px; margin-right: 20px; }");
            sb.AppendLine("    .header h1 { margin: 0; font-size: 24px; }");
            sb.AppendLine("    .content { padding: 20px; color: #333333; }");
            sb.AppendLine("    .content h2 { margin-top: 0; color: #2c3e50; }");
            sb.AppendLine("    .intro { font-size: 14px; line-height: 1.6; margin-bottom: 20px; }");
            sb.AppendLine("    table { width: 100%; border-collapse: collapse; margin-bottom: 20px; }");
            sb.AppendLine("    th, td { border: 1px solid #dddddd; padding: 12px; text-align: left; font-size: 14px; }");
            sb.AppendLine("    th { background-color: #f2f2f2; }");
            sb.AppendLine("    tr:nth-child(even) { background-color: #fcfcfc; } /* Zebra striping */");
            sb.AppendLine("    .timestamp { font-size: 12px; color: #777; margin-top: 20px; }");
            sb.AppendLine("    .footer { text-align: center; font-size: 12px; color: #999999; padding: 20px; border-top: 1px solid #eeeeee; }");
            sb.AppendLine("  </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("  <div class='container'>");

            // Header with Logo + Title
            sb.AppendLine("    <div class='header'>");
            // Replace the following src with your actual logo URL
            sb.AppendLine("      <img src='https://www.allianz.com/en/mediacenter/press-service/media-database/_jcr_content/root/parsys/wrapper_copy_copy_12/wrapper/multi_column_grid_co/grid-0-par/image.img.82.640.jpeg/1689075336400/allianz-logo-web-asset-1to1p8.jpeg' alt='Company Logo' />");
            sb.AppendLine("      <h1>End of Day Report</h1>");
            sb.AppendLine("    </div>");

            // Main Content
            sb.AppendLine("    <div class='content'>");
            sb.AppendLine("      <h2>Financial Summary</h2>");
            sb.AppendLine("      <p class='intro'>Below are today’s end-of-day financial highlights. Please review the data for accuracy. If you have any questions, don’t hesitate to contact the EOD Report Team.</p>");

            // Table
            sb.AppendLine("      <table>");
            sb.AppendLine("        <tr>");
            sb.AppendLine("          <th>Metric</th>");
            sb.AppendLine("          <th>Daily</th>");
            sb.AppendLine("          <th>MTD</th>");
            sb.AppendLine("          <th>YTD</th>");
            sb.AppendLine("        </tr>");

            // AUM row
            sb.AppendLine("        <tr>");
            sb.AppendLine("          <td>Total AUM</td>");
            sb.AppendLine($"         <td>{dailyAUM:C}</td>");
            sb.AppendLine($"         <td>{mtdAUM:C}</td>");
            sb.AppendLine($"         <td>{ytdAUM:C}</td>");
            sb.AppendLine("        </tr>");

            // Volume row
            sb.AppendLine("        <tr>");
            sb.AppendLine("          <td>Total Volume</td>");
            sb.AppendLine($"         <td>{dailyVolume:N0}</td>");
            sb.AppendLine($"         <td>{mtdVolume:N0}</td>");
            sb.AppendLine($"         <td>{ytdVolume:N0}</td>");
            sb.AppendLine("        </tr>");

            // ... Add more rows as needed
            sb.AppendLine("      </table>");

            // Timestamp
            sb.AppendLine($"      <p class='timestamp'>Report generated on: {DateTime.Now:dd-MMM-yyyy HH:mm:ss}</p>");

            sb.AppendLine("    </div>"); // end .content

            // Footer / Signature
            sb.AppendLine("    <div class='footer'>");
            sb.AppendLine("      <p><strong>The EOD Report Team</strong><br/>");
            sb.AppendLine("      Your Company, Inc.<br/>");
            sb.AppendLine("      <em>Phone: (555) 123-4567 | Email: eod.reports@yourcompany.com</em></p>");
            sb.AppendLine("    </div>");

            sb.AppendLine("  </div>"); // end .container
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            string emailBody = sb.ToString();

            // SMTP info (adjust to your environment)
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "ifpicswerefunny@gmail.com"; // your SMTP username
            string smtpPassword = "jfwf ihvr kjjm zlev";       // your SMTP password

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
