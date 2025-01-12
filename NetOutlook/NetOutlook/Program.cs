using System;
using NetOffice;
using NetOffice.OutlookApi;
using NetOffice.OutlookApi.Enums;
using NetOffice.OutlookApi.Events;

namespace NetOfficeOutlookExample
{
    class Program
    {
        // Mark as nullable so .NET 8 compiler doesn’t complain about non-null initialization
        private static _Items? _inboxItems;

        static void Main(string[] args)
        {
            // 1) Create Outlook application via NetOffice
            Application outlookApp = new Application();

            // 2) Get the MAPI namespace (returns _NameSpace, not NameSpace)
            _NameSpace outlookNamespace = outlookApp.GetNamespace("MAPI");

            // 3) Logon to Outlook
            //    Signature: Logon(string Profile, string Password, object ShowDialog, object NewSession)
            //    Pass empty strings for default profile, false for ShowDialog, false for NewSession
            outlookNamespace.Logon("", "", false, false);

            // 4) Get the default Inbox folder (_MAPIFolder)
            _MAPIFolder inbox = outlookNamespace.GetDefaultFolder(OlDefaultFolders.olFolderInbox);

            // 5) Grab the Items collection (_Items)
            _inboxItems = inbox.Items;

            // 6) Hook the ItemAddEvent if _inboxItems is not null
            //    The event expects a method with signature: void(object item)
            if (_inboxItems != null)
            {
                _inboxItems.ItemAddEvent += OnNewEmail;
            }

            Console.WriteLine("Monitoring Inbox for new emails... Press ENTER to exit.");
            Console.ReadLine();

            // 7) Cleanup / Dispose
            if (_inboxItems != null)
                _inboxItems.Dispose();

            inbox.Dispose();
            outlookNamespace.Logoff();
            outlookNamespace.Dispose();
            outlookApp.Quit();
            outlookApp.Dispose();

            Console.WriteLine("Exited cleanly.");
        }

        // This signature matches Items_ItemAddEventHandler in NetOffice
        private static void OnNewEmail(object item)
        {
            // Attempt to cast the new item to a MailItem
            MailItem mailItem = item as MailItem;
            if (mailItem != null && mailItem.Attachments.Count > 0)
            {
                foreach (Attachment attachment in mailItem.Attachments)
                {
                    string savePath = @"C:\YourFolder\" + attachment.FileName;
                    attachment.SaveAsFile(savePath);
                    Console.WriteLine($"Attachment saved: {savePath}");

                    // Dispose each attachment
                    attachment.Dispose();
                }

                Console.WriteLine("Processing files...");
            }

            // Dispose of mailItem
            mailItem?.Dispose();
        }
    }
}
