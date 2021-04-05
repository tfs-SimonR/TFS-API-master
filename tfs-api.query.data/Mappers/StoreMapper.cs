using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tfs_api.query.data.Models;
using tfs_api.query.data.Utilities;

namespace tfs_api.query.data.Mappers {
    public class StoreMapper {

        public List<StoresDto> Map(DataTable tblTable)
        {
            List<StoresDto> lstTable;
            try {
                lstTable = new List<StoresDto>();
                foreach (DataRow dataRow in tblTable.Rows) { lstTable.Add(Map(dataRow)); }
            }
            catch (Exception objException) { throw objException; }
            return lstTable;
        }
        
        public StoresDto Map(DataRow dataRow)
        {
            var result = new StoresDto
            {
                store_id = ConvertDbValue.ToType<string>(dataRow["storeId"]),
                store_name = ConvertDbValue.ToType<string>(dataRow["store_name"]),
                lat = ConvertDbValue.ToType<decimal>(dataRow["properties_lat"]),
                longitude = ConvertDbValue.ToType<decimal>(dataRow["properties_long"])
            };

            return result;
        }
    }
}
