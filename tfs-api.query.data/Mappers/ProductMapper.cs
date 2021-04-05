using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tfs_api.query.data.Models;
using tfs_api.query.data.Utilities;

namespace tfs_api.query.data.Mappers
{
    public class ProductMapper
    {
        public List<ProductDto> Map(DataTable tblTable)
        {
            List<ProductDto> lstTable;
            try
            {
                lstTable = new List<ProductDto>();
                foreach (DataRow dataRow in tblTable.Rows) { lstTable.Add(Map(dataRow)); }
            }
            catch (Exception objException) { throw objException; }
            return lstTable;
        }

        public ProductDto Map(DataRow dataRow)
        {
            var result = new ProductDto
            {
                variantcode = ConvertDbValue.ToType<string>(dataRow["variantcode"]),
                varint = ConvertDbValue.ToType<int?>(dataRow["varint"]),
                proddesc = ConvertDbValue.ToType<string>(dataRow["proddesc"])
                
            };

            return result;
        }

    }
}
