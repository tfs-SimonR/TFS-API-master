using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClickAndCollectDAL;

namespace TFS_API.Factories.IFactories
{
    public interface IDbFactory<TContext> : IDisposable where TContext : DbContext, new()
    {
        TContext Init();
    }
}
