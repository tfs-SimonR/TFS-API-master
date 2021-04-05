using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFS_API.Services.Interfaces
{
    public interface IStoredProcedureService
    {
        IEnumerable<dynamic> DataEnumerable(string cmdText);
    }
}
