using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using tfs_api.query.data.Models;
using tfs_api.query.data.Utilities;

namespace TFS_API.Dto.Mappers
{
    public class WarehouseMapper
    {
        public List<WarehouseDto> Map(DataTable tblTable)
        {
            List<WarehouseDto> lstTable;
            try
            {
                lstTable = new List<WarehouseDto>();
                foreach (DataRow dataRow in tblTable.Rows) { lstTable.Add(Map(dataRow)); }
            }
            catch (Exception objException) { throw objException; }
            return lstTable;
        }

        public WarehouseDto Map(DataRow dataRow)
        {
            var result = new WarehouseDto
            {
                Pallet_Number = ConvertDbValue.ToType<string>(dataRow["Pallet_Number"]),
                Destination_Id = ConvertDbValue.ToType<string>(dataRow["Destination_Id"]),
                Destination_Name = ConvertDbValue.ToType<string>(dataRow["Destination_Name"]),
                Source_Id = ConvertDbValue.ToType<string>(dataRow["Source_Id"]),
                SourceName = ConvertDbValue.ToType<string>(dataRow["SourceName"]),
                SKU = ConvertDbValue.ToType<string>(dataRow["SKU"]),
                Qty = ConvertDbValue.ToType<int?>(dataRow["Qty"]),
                Description = ConvertDbValue.ToType<string>(dataRow["Description"]),
                Shipment_Date = ConvertDbValue.ToType<DateTime>(dataRow["Shipment_Date"]),
                BookedIn_Date = ConvertDbValue.ToType<DateTime>(dataRow["BookedIn_Date"]),
                IsDelivered = ConvertDbValue.ToType<bool?>(dataRow["IsDelivered"]),
                IsHeld = ConvertDbValue.ToType<bool?>(dataRow["IsHeld"])
            };
            return result;
        }
    }
}