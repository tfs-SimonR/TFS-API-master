using System.Collections.Generic;

namespace TFS_API.Services
{
    public interface ICreateFileService
    {
        List<string> CreateFile(string store, List<string> flatfile);
        List<string> TestCreateFile(string store, List<string> flatfile);
        List<string> HoldFlatFile(string filename, string store, List<string> flatfile);
        List<string> TestHoldFlatFile(string filename, string store, List<string> flatfile);
    }
}