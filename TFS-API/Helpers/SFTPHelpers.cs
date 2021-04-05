using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace TFS_API.Helpers
{
    public class SFTPHelpers
    {
        public static bool CheckIfFileExists(string fileName)
        {
            Directory.CreateDirectory(@"C:\SFTPDownloads");
            string host = @"tofs.exavault.com";
            string username = "tofs";
            string password = @"Burnley123";

            // Path to file on SFTP server
            string pathRemoteFile = "/StockLevels/" + fileName;
            using (var sftp = new SftpClient(host, username, password))
            {
                try
                {
                    sftp.Connect();
                    var fileexist = sftp.Exists(pathRemoteFile);
                    if (fileexist)
                    {
                        sftp.Disconnect();
                        return true;

                    }
                    else
                    {
                        sftp.Disconnect();
                        return false;

                    }
                }
                catch (Exception er)
                {
                    Console.WriteLine("An exception has been caught " + er.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Downloads a file in the desktop synchronously
        /// </summary>
        public static void downloadFile(string fileName)
        {
            Directory.CreateDirectory(@"C:\SFTPDownloads");
            string host = @"tofs.exavault.com";
            string username = "tofs";
            string password = @"Burnley123";

            // Path to file on SFTP server
            string pathRemoteFile = "/StockLevels/" + fileName;

                string pathLocalFile = Path.Combine(@"C:\SFTPDownloads", fileName);

            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                try
                {
                    sftp.Connect();

                    Console.WriteLine("Downloading {0}", pathRemoteFile);

                    using (Stream fileStream = File.OpenWrite(pathLocalFile))
                    {
                        sftp.DownloadFile(pathRemoteFile, fileStream);
                    }

                    sftp.Disconnect();
                }
                catch (Exception er)
                {
                    Console.WriteLine("No file called " + fileName + " on exavault");
                }
            }
        }

        /// <summary>
        /// Downloads a file in the desktop synchronously
        /// </summary>
        public static void uploadFile(string fileName)
        {
            string host = @"tofs.exavault.com";
            string username = "tofs";
            string password = @"Burnley123";

            // Path to file on SFTP server
            string pathRemoteFile = "/StockLevels/" + fileName;


            string pathLocalFile = Path.Combine(@"C:\SPQ_ecomm_downloads", fileName);

            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                try
                {
                    sftp.Connect();

                    Console.WriteLine("Uploading {0}", pathRemoteFile);

                    using (Stream fileStream = File.OpenRead(pathLocalFile))
                    {
                        sftp.UploadFile(fileStream, pathRemoteFile);
                    }

                    sftp.Disconnect();
                }
                catch (Exception er)
                {
                    Console.WriteLine("An exception has been caught " + er.ToString());
                }
            }
        }

        /// <summary>
        /// List a remote directory in the console.
        /// </summary>
        public static IEnumerable<SftpFile> listFiles(string path)
        {
            string host = @"tofs.exavault.com";
            string username = "tofs";
            string password = @"Burnley123";

            string remoteDirectory = path;

            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                try
                {
                    sftp.Connect();

                    var files = sftp.ListDirectory(remoteDirectory);

                    foreach (var file in files)
                    {
                        Console.WriteLine(file.Name);
                    }

                    sftp.Disconnect();

                    return files;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception has been caught " + e.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete a remote file
        /// </summary>
        public static void deleteFile(string fileName)
        {
            string host = @"tofs.exavault.com";
            string username = "tofs";
            string password = @"Burnley123";

            // Path to file on SFTP server
            string pathRemoteFile = "/To_SPQ/" + fileName;

            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                try
                {
                    sftp.Connect();
                    Console.WriteLine("Deleting {0}", pathRemoteFile);
                    // Delete file
                    sftp.DeleteFile(pathRemoteFile);

                    sftp.Disconnect();
                }
                catch (Exception er)
                {
                    Console.WriteLine("An exception has been caught " + er.ToString());
                }
            }
        }

        public static void RenameFile(string fileName)
        {
            string host = @"tofs.exavault.com";
            string username = "tofs";
            string password = @"Burnley123";
            string remoteDirectory = "/To_SPQ/";

            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                sftp.Connect();
                sftp.RenameFile(remoteDirectory + fileName, remoteDirectory + "shopify_Orders_VAT_READY.csv");
                sftp.Disconnect();
            }
        }
    }
}