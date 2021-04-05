using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PICountDataAccessLayer;

namespace TFS_API.Repositorys.Interfaces
{
    public interface IProductCountRepository
    {
        List<tbl_product_count> GetAllRecords();
    }
}