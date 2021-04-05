using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TFS_API.Repositorys.Interfaces
{
    public interface IUnitOfWork
    {
        void SaveChanges();
    }


}