using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using TFS_API.Helpers;
using TFS_API.Services.Interfaces;

namespace TFS_API.Services
{
    /// <inheritdoc />
    public class CreateFlatFileService : ICreateFileService
    {
        #region Setup
        private readonly IExceptionService _errorService;
        #endregion

        /// <inheritdoc />
        public CreateFlatFileService(IExceptionService errorService)
        {
            _errorService = errorService;
        }

        public CreateFlatFileService()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        /// <param name="flatfile"></param>
        /// <returns></returns>
        public List<string> CreateFile(string store, List<string> flatfile)
        {
            try
            {
                const string username = "sroberts";
                const string domain = "fsdomain.local";
                const string password = "Burnley123";
                //using (var impersonation = new ImpersonatedUser(username, domain, password))
                //{
                var starttime = TimeSpan.Parse("20:00"); // 8 PM
                var endtime = TimeSpan.Parse("06:00"); // 6 AM
                var rightnow = DateTime.Now.TimeOfDay;
                if (starttime <= endtime)
                {
                    // start and stop times are in the same day
                    if (rightnow >= starttime && rightnow <= endtime)
                    {
                        // current time is between start and stop but we dont need this at the moment
                    }
                }
                else
                {
                    // start and stop times are in different days
                    if (rightnow >= starttime || rightnow <= endtime)
                    {
                        var fi = new FileInfo(@"\\tfs-vmmas01-prod\live\polldata\heldscannerfiles\DATA" + store);
                        var exists = fi.Exists;

                        if (!exists)
                        {
                            var filePath = @"\\tfs-vmmas01-prod\live\polldata\heldscannerfiles\DATA" + store;
                            var myPerm = new FileIOPermission(FileIOPermissionAccess.AllAccess, filePath + flatfile);
                            myPerm.Assert();
                            File.WriteAllLines(filePath, flatfile);
                        }
                        else
                        {
                            using (var file =
                                new StreamWriter(@"\\tfs-vmmas01-prod\live\polldata\heldscannerfiles\DATA" + store,
                                    true))
                            {
                                foreach (var line in flatfile) file.WriteLine(line);
                            }
                        }
                    }
                    else
                    {
                        var fi = new FileInfo(@"\\tfs-vmmas01-prod\live\PolldataScan\DATA" + store);
                        var exists = fi.Exists;

                        if (!exists)
                        {
                            var filePath = @"\\tfs-vmmas01-prod\live\PolldataScan\DATA" + store;
                            var myPerm = new FileIOPermission(FileIOPermissionAccess.AllAccess, filePath + flatfile);
                            myPerm.Assert();
                            File.WriteAllLines(filePath, flatfile);
                        }
                        else
                        {
                            using (var file = new StreamWriter(@"\\tfs-vmmas01-prod\live\PolldataScan\DATA" + store,
                                true))
                            {
                                foreach (var line in flatfile) file.WriteLine(line);
                            }
                        }
                    }
                }
                    
                //}

            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;

            }

            return flatfile;
        }
        public List<string> TestCreateFile(string store, List<string> flatfile)
        {
            try
            {
                const string username = "sroberts";
                const string domain = "fsdomain.local";
                const string password = "Burnley123";
                using (var impersonation = new ImpersonatedUser(username, domain, password))
                {
                    TimeSpan starttime = TimeSpan.Parse("20:00"); // 8 PM
                    TimeSpan endtime = TimeSpan.Parse("06:00");   // 6 AM
                    TimeSpan rightnow = DateTime.Now.TimeOfDay;
                    if (starttime <= endtime)
                    {
                        // start and stop times are in the same day
                        if (rightnow >= starttime && rightnow <= endtime)
                        {
                            // current time is between start and stop but we dont need this at the moment
                        }
                    }
                    else
                    {
                        // start and stop times are in different days
                        if (rightnow >= starttime || rightnow <= endtime)
                        {
                            var fi = new FileInfo(@"\\tfs-vmmas01-pro\live\polldata\testheldscannerfiles\DATA" + store);
                            var exists = fi.Exists;

                            if (!exists)
                            {
                                var filePath = @"\\tfs-vmmas01-pro\live\polldata\testheldscannerfiles\DATA" + store;
                                var myPerm = new FileIOPermission(FileIOPermissionAccess.AllAccess, filePath + flatfile);
                                myPerm.Assert();
                                File.WriteAllLines(filePath, flatfile);

                            }
                            else
                            {

                                using (var file = new StreamWriter(@"\\tfs-vmmas01-pro\live\polldata\testheldscannerfiles\DATA" + store, true))
                                {
                                    foreach (var line in flatfile) file.WriteLine(line);
                                }

                            }
                        }
                        else
                        {
                            var fi = new FileInfo(@"\\tfs-vmmas01-pro\live\TestPolldatascan\DATA" + store);
                            var exists = fi.Exists;

                            if (!exists)
                            {
                                var filePath = @"\\tfs-vmmas01-pro\live\TestPolldatascan\DATA" + store;
                                var myPerm = new FileIOPermission(FileIOPermissionAccess.AllAccess, filePath + flatfile);
                                myPerm.Assert();
                                File.WriteAllLines(filePath, flatfile);

                            }
                            else
                            {

                                using (var file = new StreamWriter(@"\\tfs-vmmas01-pro\live\TestPolldatascan\DATA" + store, true))
                                {
                                    foreach (var line in flatfile) file.WriteLine(line);
                                }

                            }

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;

            }

            return flatfile;
        }

        public List<string> HoldFlatFile(string filename, string store, List<string> flatfile)
        {
            try
            {
                const string username = "sroberts";
                const string domain = "fsdomain.local";
                const string password = "Burnley123";
                using (var impersonation = new ImpersonatedUser(username, domain, password))
                {
                    var fi = new FileInfo(@"\\tfs-vmmas01-pro\live\polldata\helddatafiles\" + filename);
                    var exists = fi.Exists;

                    if (!exists)
                    {
                        var filePath = @"\\tfs-vmmas01-pro\live\polldata\helddatafiles\" + filename;
                        var myPerm = new FileIOPermission(FileIOPermissionAccess.AllAccess, filePath + flatfile);
                        myPerm.Assert();
                        File.WriteAllLines(filePath, flatfile);

                    }
                    else
                    {
                        using (var file = new StreamWriter(@"\\tfs-vmmas01-pro\live\polldata\helddatafiles\" + filename, true))
                        {
                            foreach (var line in flatfile) file.WriteLine(line);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;

            }

            return flatfile;

        }

        public List<string> TestHoldFlatFile(string filename, string store, List<string> flatfile)
        {
            try
            {
                const string username = "sroberts";
                const string domain = "fsdomain.local";
                const string password = "Burnley123";
                using (var impersonation = new ImpersonatedUser(username, domain, password))
                {
                    var fi = new FileInfo(@"\\tfs-vmmas01-pro\live\polldata\testheldscannerfiles\" + filename);
                    var exists = fi.Exists;

                    if (!exists)
                    {
                        var filePath = @"\\tfs-vmmas01-pro\live\polldata\testheldscannerfiles\" + filename;
                        var myPerm = new FileIOPermission(FileIOPermissionAccess.AllAccess, filePath + flatfile);
                        myPerm.Assert();
                        File.WriteAllLines(filePath, flatfile);

                    }
                    else
                    {
                        using (var file = new StreamWriter(@"\\tfs-vmmas01-pro\live\polldata\testheldscannerfiles\" + filename, true))
                        {
                            foreach (var line in flatfile) file.WriteLine(line);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                _errorService.Capture(ex);
                throw;

            }

            return flatfile;

        }
    }
}