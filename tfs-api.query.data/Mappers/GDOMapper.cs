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
    public class GDOMapper {

        public List<GdoDto> Map(DataTable tblTable)
        {
            List<GdoDto> lstTable;
            try
            {
                lstTable = new List<GdoDto>();
                foreach (DataRow dataRow in tblTable.Rows) { lstTable.Add(Map(dataRow)); }
            }
            catch (Exception objException) { throw objException; }
            return lstTable;
        }

        public GdoDto Map(DataRow dataRow) {
            var result = new GdoDto {
                RandomValue = ConvertDbValue.ToType<string>(dataRow["RandomValue"]),
                StoreDestination = ConvertDbValue.ToType<string>(dataRow["storeddestination"]),
                StoreSource = ConvertDbValue.ToType<string>(dataRow["storesource"]),
                GeneratedGDO = ConvertDbValue.ToType<string>(dataRow["generatedGDO"]),
                DateCreated = ConvertDbValue.ToType<DateTime?>(dataRow["datecreated"]),
                isUSed = ConvertDbValue.ToType<bool?>(dataRow["isUsed"])
            };
            return result;
        }
    }
}
