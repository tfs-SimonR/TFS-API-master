using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TFS_API.Models.BindingModels;

namespace TFS_API.Mappers
{
    public class HandyScannerMappers
    {
        public static HandyScannerBM dataToVM(HandyScannerBM vm, decimal removeDecimal)
        {
            vm.description = vm.description;
            vm.sku = vm.sku;
            vm.barcode = vm.barcode;
            vm.sellingprice = removeDecimal;
            vm.stock_qty = vm.stock_qty;
            return vm;
        }
    }
}