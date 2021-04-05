using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tfs_api.query.data.Models;
using tfs_api.query.data.Utilities;

namespace tfs_api.query.data.Mappers {
    public class RTWReasonCodeMapper {

        public List<RTWReasonCodeDto> Map(DataTable tblTable) {
            List<RTWReasonCodeDto> lstTable;
            try
            {
                lstTable = new List<RTWReasonCodeDto>();
                foreach (DataRow dataRow in tblTable.Rows) { lstTable.Add(Map(dataRow)); }
            }
            catch (Exception objException) { throw objException; }
            return lstTable;
        }
        public RTWReasonCodeDto Map(DataRow dataRow)
        {
            var result = new RTWReasonCodeDto {
                id = ConvertDbValue.ToType<int>(dataRow["id"]),
                description = ConvertDbValue.ToType<string>(dataRow["description"])
            };

            return result;
        }
    }
}
