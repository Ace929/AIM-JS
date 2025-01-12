using System;
using System.IO;
using System.Net;

class FtpProgram
{
    static void Main(string[] args)
    {
        string ftpServer = "ftp://example.com";
        string username = "yourUsername";
        string password = "yourPassword";
        string remoteDownloadPath = "/path/to/remote/file.txt";
        string localDownloadPath = "C:\\Temp\\file.txt";
        string localUploadPath = "C:\\Temp\\processed_file.txt";
        string remoteUploadPath = "/path/to/remote/processed_file.txt";

        // Step 1: Download the file from FTP
        Console.WriteLine("Downloading file...");
        DownloadFile(ftpServer + remoteDownloadPath, localDownloadPath, username, password);

        // Step 2: Process the file (example: appending text)
        Console.WriteLine("Processing file...");
        ProcessFile(localDownloadPath, localUploadPath);

        // Step 3: Upload the processed file back to FTP
        Console.WriteLine("Uploading file...");
        UploadFile(ftpServer + remoteUploadPath, localUploadPath, username, password);

        Console.WriteLine("Operation complete!");
    }

    static void DownloadFile(string remotePath, string localPath, string username, string password)
    {
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remotePath);
        request.Method = WebRequestMethods.Ftp.DownloadFile;
        request.Credentials = new NetworkCredential(username, password);

        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
        using (Stream responseStream = response.GetResponseStream())
        using (FileStream fileStream = new FileStream(localPath, FileMode.Create))
        {
            responseStream.CopyTo(fileStream);
        }

        Console.WriteLine($"File downloaded to {localPath}");
    }

    static void ProcessFile(string inputPath, string outputPath)
    {
        // Example: Read the file and append a line
        string content = File.ReadAllText(inputPath);
        content += "\nProcessed by FTP Program at " + DateTime.Now;
        File.WriteAllText(outputPath, content);

        Console.WriteLine($"File processed and saved to {outputPath}");
    }

    static void UploadFile(string remotePath, string localPath, string username, string password)
    {
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remotePath);
        request.Method = WebRequestMethods.Ftp.UploadFile;
        request.Credentials = new NetworkCredential(username, password);

        byte[] fileContents = File.ReadAllBytes(localPath);
        request.ContentLength = fileContents.Length;

        using (Stream requestStream = request.GetRequestStream())
        {
            requestStream.Write(fileContents, 0, fileContents.Length);
        }

        using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
        {
            Console.WriteLine($"Upload status: {response.StatusDescription}");
        }
    }
}
