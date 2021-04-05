using System;
using System.Data.Entity;
using TFS_API.Factories.IFactories;
using TFS_API.Repositorys;

namespace TFS_API.Factories
{
    public class DbFactory<TContext> : Disposable, IDbFactory<TContext> where TContext : DbContext, new()
    {
        TContext _dbContext;

        public TContext Init()
        {
            return _dbContext ?? (_dbContext = new TContext());
        }
    }

    
}
