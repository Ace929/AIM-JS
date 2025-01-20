using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MimeKit;
using OfficeOpenXml;

namespace EmailMonitorDemo
{
    class Program
    {
        // Folder path for storing attachments
        private static readonly string AttachmentsFolder = @"C:\Users\seali\OneDrive\Documents\GitHub\AIM-JS\EmailMonitorDemo\ProcessedFiles";

        static async Task Main(string[] args)
        {
            // Create the folder if it doesn't exist
            Directory.CreateDirectory(AttachmentsFolder);

            // Gmail IMAP & SMTP settings
            string imapHost = "imap.gmail.com";
            int imapPort = 993; // IMAP over SSL
            string smtpHost = "smtp.gmail.com";
            int smtpPort = 465; // SMTP over SSL

            // Gmail info, set with the App Password
            string userName = "ifpicswerefunny@gmail.com";
            string password = "jfwf ihvr kjjm zlev";

            bool useSSL = true;

            // Checking interval in minutes
            int checkIntervalMinutes = 1;

            // Subject we’re looking for in unread emails
            string subjectCriteria = "Daily Report";

            Console.WriteLine("Email monitor started. Press Ctrl+C to exit.\n");

            // Continuous loop: check inbox every X minutes
            while (true)
            {
                try
                {
                    await CheckInboxAsync(
                        imapHost, imapPort,
                        userName, password, useSSL,
                        subjectCriteria,
                        smtpHost, smtpPort
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
                }

                Console.WriteLine($"Waiting {checkIntervalMinutes} minute(s) before next check...\n");
                await Task.Delay(TimeSpan.FromMinutes(checkIntervalMinutes));
            }
        }

        private static async Task CheckInboxAsync(
            string imapHost, int imapPort,
            string userName, string password, bool useSSL,
            string subjectCriteria,
            string smtpHost, int smtpPort)
        {
            using (var imapClient = new ImapClient())
            {
                // Connect to Gmail IMAP
                await imapClient.ConnectAsync(imapHost, imapPort, useSSL);
                await imapClient.AuthenticateAsync(userName, password);

                // Access the INBOX folder
                var inbox = imapClient.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadWrite);

                // Search for UNSEEN (unread) messages
                var uids = await inbox.SearchAsync(SearchQuery.NotSeen);

                foreach (var uid in uids)
                {
                    // Retrieve the full email message
                    var message = await inbox.GetMessageAsync(uid);

                    // Check if the subject matches
                    if (message.Subject != null &&
                        message.Subject.Contains(subjectCriteria, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"[FOUND] Unread email with subject: {message.Subject}");

                        // Process attachments (download Excel & do operations)
                        //    Return the processed file path (if any)
                        string processedFilePath = await HandleAttachmentsAsync(message);

                        // Send a reply if we have a from-address
                        var fromAddress = message.From.Mailboxes.FirstOrDefault()?.Address;
                        if (!string.IsNullOrEmpty(fromAddress))
                        {
                            await SendEmailAsync(
                                smtpHost, smtpPort,
                                userName, password,
                                fromAddress,
                                subject: "Re: " + message.Subject,
                                body: "Your file has been processed. See attached.",
                                attachmentPath: processedFilePath
                            );
                        }

                        // Mark the email as SEEN so we don't process it again
                        await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
                        Console.WriteLine("Marked message as seen.\n");
                    }
                }

                // Disconnect cleanly
                await imapClient.DisconnectAsync(true);
            }
        }

        private static async Task<string> HandleAttachmentsAsync(MimeMessage message)
        {
            // We'll store the final "processed" file path, if any
            // in case you need to attach it to the reply
            string finalProcessedFilePath = null;

            foreach (var attachment in message.Attachments)
            {
                // Check for Excel MIME types
                if (attachment is MimePart part &&
                    (part.ContentType.MimeType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ||
                     part.ContentType.MimeType == "application/vnd.ms-excel"))
                {
                    // Use provided file name or fallback
                    string fileName = part.FileName;
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = "attachment.xlsx";
                    }

                    // 8. Build a local path for the original attachment
                    string originalAttachmentPath = Path.Combine(AttachmentsFolder, fileName);

                    // Save attachment to local file
                    using (var stream = File.Create(originalAttachmentPath))
                    {
                        await part.Content.DecodeToAsync(stream);
                    }

                    Console.WriteLine($"Attachment saved to: {originalAttachmentPath}");

                    // 9. Process the Excel file
                    //    Then we get a processed file path
                    finalProcessedFilePath = ProcessExcelFile(originalAttachmentPath);
                }
            }

            return finalProcessedFilePath;
        }

        private static string ProcessExcelFile(string filePath)
        {
            // EPPlus licensing
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // We'll save the processed file to the same folder but with a new name
            // e.g., "Processed-<originalFileName>.xlsx"
            var folder = Path.GetDirectoryName(filePath) ?? "";
            var baseName = Path.GetFileNameWithoutExtension(filePath);
            var processedFileName = $"Processed-{baseName}.xlsx";
            var processedFilePath = Path.Combine(folder, processedFileName);

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                // If no worksheets exist, just skip
                if (package.Workbook.Worksheets.Count == 0)
                {
                    Console.WriteLine("No worksheets found in the Excel file.");
                    return null;
                }

                var worksheet = package.Workbook.Worksheets[0];

                // Example: read cell A1
                var valueA1 = worksheet.Cells["A1"].Text;
                Console.WriteLine("Cell A1 Value: " + valueA1);

                // Example: do something with each row
                int rowCount = worksheet.Dimension?.Rows ?? 0;
                for (int row = 2; row <= rowCount; row++)
                {
                    // Read from column 1
                    var data = worksheet.Cells[row, 1].Text;
                    // Do something with 'data'
                }

                // Write something to B1
                worksheet.Cells["B1"].Value = "Processed on " + DateTime.Now.ToString("g");

                // Save changes as "Processed-<originalFileName>.xlsx"
                package.SaveAs(processedFilePath);
            }

            Console.WriteLine($"Excel file processed. Saved as: {processedFilePath}");
            return processedFilePath;
        }

        private static async Task SendEmailAsync(
            string smtpHost, int smtpPort,
            string userName, string password,
            string toEmail,
            string subject,
            string body,
            string attachmentPath = null)
        {
            // Create a new email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("My Processor App", userName));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            // Build the body
            var builder = new BodyBuilder
            {
                TextBody = body
            };

            // Attach the processed Excel file, if it exists
            if (!string.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath))
            {
                builder.Attachments.Add(attachmentPath);
            }

            message.Body = builder.ToMessageBody();

            // Send via SMTP
            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(smtpHost, smtpPort, true); // SSL = true
                await smtpClient.AuthenticateAsync(userName, password);
                await smtpClient.SendAsync(message);
                await smtpClient.DisconnectAsync(true);
            }

            Console.WriteLine($"Reply sent to {toEmail}\n");
        }
    }
}
