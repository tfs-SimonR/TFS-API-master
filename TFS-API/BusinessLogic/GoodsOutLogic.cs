using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using tfs_api.query.data.Models;
using TFS_API.Helpers;
using TFS_API.Models.ViewModels;
using TFS_API.Service;
using TFS_API.Service.Interface;
using TFS_API.Services;

namespace TFS_API.BusinessLogic
{
    public class GoodsOutLogic
    {
        private static CreateFlatFileService _createFileService = new CreateFlatFileService();
        private static ScannerService scannerService = new ScannerService();

        public static List<WarehouseDto> CreateWarehouseInsertList(List<CreateISTDTO> dto, string gdoNumber)
        {
            var wareHouseInsertList = new List<WarehouseDto>();

            foreach (var item in dto)
            foreach (var items in item.shipment_items)
            {
                var getProductObject = scannerService.GetProductObject(items.sku).SingleOrDefault();

                if (getProductObject == null)
                    getProductObject = new ProductDto
                    {
                        proddesc = "No Description", variantcode = items.sku, varint = 1
                    };
                var getDestination = scannerService.GetStoresById(item.destination_store_id).First().store_name;
                var source = scannerService.GetStoresById(item.store_id).First().store_name;

                var model = new WarehouseDto
                {
                    Pallet_Number = gdoNumber,
                    Destination_Id = item.destination_store_id,
                    Destination_Name = getDestination,
                    Source_Id = item.store_id,
                    SourceName = source,
                    SKU = items.sku,
                    Qty = int.Parse(items.sku_counted_qty),
                    Description = getProductObject.proddesc,
                    IsDelivered = false,
                    IsHeld = false,
                    varint = getProductObject.varint
                };
                wareHouseInsertList.Add(model);
            }

            //wareHouseInsertList.RemoveAll(x => x.Description == null && x.varint == null);
            return wareHouseInsertList;
        }

        public static bool CreateDataForFlatFile(CreateISTDTO dto, string storeid, string createGdoString)
        {
            var flatfile = new List<string>();
            var date = DateTime.Now.Date.ToString("ddMMyy");
            var time = DateTime.Now.ToString("hhmm");
            string line1;
            /*If the shipment is back to the Warehouse we need to change the ID to MW in the header line*/
            if (dto.destination_store_id == "900")
                line1 = "MW" + storeid + "010000" + "0002" + date + time + "900" + createGdoString + "  ";
            else if (dto.destination_store_id == "777")
                line1 = "RS" + storeid + "010000" + "0002" + date + time + dto.destination_store_id +
                        createGdoString + "  ";
            else
                line1 = "MO" + storeid + "010000" + "0002" + date + time + dto.destination_store_id +
                        createGdoString + "  ";
            flatfile.Add(line1);
            foreach (var item in dto.shipment_items)
            {
                
                double? RetailPrice = 0.00;

                if (RetailPrice != null)
                {
                    var totalValue = Math.Round((decimal)(int.Parse(item.sku_counted_qty) * RetailPrice), 2);

                    var truncate = DataHelpers.TruncateDecimal(totalValue, 2);
                    var removedecimalpoint = DataHelpers.ConvertdecimaltoInt(truncate);

                    var intToString = DataHelpers.setQuantity(removedecimalpoint.ToString(CultureInfo.CurrentCulture));
                    var setQty = DataHelpers.setQuantity(item.sku_counted_qty);

                    string lines;
                    if (item.sku.Length <= 6)
                        lines = string.Format("MV" + item.sku + "  " + intToString + " " + setQty);
                    else
                        lines = string.Format("MV" + item.sku + intToString + " " + setQty);
                    flatfile.Add(lines);
                }
            }
            _createFileService.CreateFile(storeid, flatfile);
            return true;
        }
    }
}