using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using TFS_API.Models;
using TFS_API.Models.Tables;
using tfs_api.query.data.Models;
using TFS_API.Service.Interface;
using TFS_API.ServiceReference1;
using TFS_API.Services;
using TFS_API.Services.Interfaces;

namespace TFS_API.Service
{
    public class ArtsService: IARTSService, IDisposable
    {
        #region Disposal 

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //_dal?.Dispose();
            }
        }

        #endregion

        static readonly RetailHubContractClient client = new RetailHubContractClient("BasicHttpBinding_IRetailHubContract");

        public delegate void ArtsServiceExceptionHandler(Exception exception, string methodName);

        public delegate void ArtsServiceEventHandler(string methodName, string eventText);

        public delegate void ArtsServiceResultHandler(bool result, string methodName);

        public event ArtsServiceEventHandler ArtsServiceServiceEvent = delegate { };
        public event ArtsServiceExceptionHandler ArtsServiceException = delegate { };
        public event ArtsServiceResultHandler ArtsServiceResult = delegate { };


        public string StockAdjustmentAdhoc(List<AdHocDTO> dto)
        {
            int counter = 1;


            XNamespace xmlns = XNamespace.Get("");
            XNamespace xsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
            XNamespace schemaLocation = XNamespace.Get("http://www.nrf-arts.org/IXRetail/namespace/POSLog.xsd");


            XElement xml;
            xml = new XElement(xmlns + "POSLog", new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                new XAttribute(xsi + "schemaLocation", schemaLocation),
                from s in dto
                select
                    new XElement("Transaction", new XAttribute("TrainingModeFlag", "false"),
                        new XAttribute("CancelFlag", "false"),
                        new XElement("Location", s.store_id),
                        new XElement("WorkstationID", 32450897),
                        new XElement("SequenceNumber", "H" + DateTime.Now.ToString("HHmmss")),
                        new XElement("BusinessDayDate", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement("InventoryControlTransaction",
                            new XElement("InventoryAdjustment",
                                new XElement("ReasonCode", "PP"),
                                s.counted_items.Select(x =>
                                    new XElement("LineItem",
                                        new XElement("ItemID", x.sku),
                                        new XElement("LineItemNumber", counter++),
                                        new XElement("QuantityOrdered", new XAttribute("Units", x.sku_counted_qty),
                                            new XAttribute("UnitOfMeasureCode", "EA"), x.sku_counted_qty),
                                        new XElement("FromSubLoc"), new XAttribute("LocationType", "Category"), 1))
                            )
                        )));

            var createEnvelopeRequestWithPayloadOfString = CreateEnvelopeRequestWithPayloadOfString(xml);

            try
            {
                var send = client.PostStoreTransactions(createEnvelopeRequestWithPayloadOfString);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            client.Close();

            return xml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public string WarehouseReceipt(List<WarehouseDto> dto)
        {
            int counter = 1;
            var grouped = dto
                .OrderBy(x => x.Pallet_Number)
                .GroupBy(x => x.Pallet_Number)
                .Select(g => new
                {
                    pallet_identifier = g.Key,
                    destinationId = g.Select(x => x.Destination_Id).FirstOrDefault(),
                    sourceId = g.Select(x => x.Source_Id).FirstOrDefault(),
                    shipment_items = g.Select(ShipmentItems => new
                    {
                        sku = ShipmentItems.SKU,
                        stock_qty = ShipmentItems.Qty,
                        count = counter++
                    })
                });

            XNamespace xmlns = XNamespace.Get("");
            XNamespace xsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
            XNamespace schemaLocation = XNamespace.Get("http://www.nrf-arts.org/IXRetail/namespace/POSLog.xsd");


            XElement xml;
            xml = new XElement(xmlns + "POSLog", new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                new XAttribute(xsi + "schemaLocation", schemaLocation),
                from s in grouped
                select
                    new XElement("Transaction", new XAttribute("TrainingModeFlag", "false"),
                        new XAttribute("CancelFlag", "false"),
                        new XElement("Location", s.destinationId),
                        new XElement("WorkstationID", 32450897),
                        new XElement("SequenceNumber", "H" + DateTime.Now.ToString("HHmmss")),
                        new XElement("BusinessDayDate", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement("InventoryControlTransaction",
                            new XElement("ReceiveInventory", new XAttribute("DocumentType", "Transfer"),
                                new XElement("DocumentID", s.pallet_identifier),
                                new XElement("FromParty", s.sourceId),
                                s.shipment_items.Select(x =>
                                    new XElement("LineItem",
                                        new XElement("ItemID", x.sku),
                                        new XElement("LineItemNumber", x.count),
                                        new XElement("QuantityReceived", new XAttribute("Units", x.stock_qty),
                                            new XAttribute("UnitOfMeasureCode", "EA"), x.stock_qty)))
                            )
                        )));

            var createEnvelopeRequestWithPayloadOfString = CreateEnvelopeRequestWithPayloadOfString(xml);

            try
            {
                var send = client.PostStoreTransactions(createEnvelopeRequestWithPayloadOfString);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            client.Close();

            return xml.ToString();
        }

        public string PurchaseOrderReceipt(List<WarehouseDto> dto)
        {
            int counter = 1;
            var grouped = dto
                .OrderBy(x => x.Pallet_Number)
                .GroupBy(x => x.Pallet_Number)
                .Select(g => new
                {
                    pallet_identifier = g.Key,
                    destinationId = g.Select(x => x.Destination_Id).FirstOrDefault(),
                    sourceId = g.Select(x => x.Source_Id).FirstOrDefault(),
                    shipment_items = g.Select(ShipmentItems => new
                    {
                        sku = ShipmentItems.SKU,
                        stock_qty = ShipmentItems.Qty,
                        count = counter++
                    })
                });

            XNamespace xmlns = XNamespace.Get("");
            XNamespace xsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
            XNamespace schemaLocation = XNamespace.Get("http://www.nrf-arts.org/IXRetail/namespace/POSLog.xsd");


            XElement xml;
            xml = new XElement(xmlns + "POSLog", new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                new XAttribute(xsi + "schemaLocation", schemaLocation),
                from s in grouped
                select
                    new XElement("Transaction", new XAttribute("TrainingModeFlag", "false"),
                        new XAttribute("CancelFlag", "false"),
                        new XElement("Location", s.destinationId),
                        new XElement("WorkstationID", 32450897),
                        new XElement("SequenceNumber", "H" + DateTime.Now.ToString("HHmmss")),
                        new XElement("BusinessDayDate", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement("InventoryControlTransaction",
                            new XElement("ReceiveInventory", new XAttribute("DocumentType", "Transfer"),
                                new XElement("DocumentID", s.pallet_identifier),
                                new XElement("FromParty", s.sourceId),
                                s.shipment_items.Select(x =>
                                    new XElement("LineItem",
                                        new XElement("ItemID", x.sku),
                                        new XElement("LineItemNumber", x.count),
                                        new XElement("QuantityReceived", new XAttribute("Units", x.stock_qty),
                                            new XAttribute("UnitOfMeasureCode", "EA"), x.stock_qty)))
                            )
                        )));

            var createEnvelopeRequestWithPayloadOfString = CreateEnvelopeRequestWithPayloadOfString(xml);

            try
            {
                var send = client.PostStoreTransactions(createEnvelopeRequestWithPayloadOfString);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            client.Close();

            return xml.ToString();
        }

        public string InterStoreReceipt(List<WarehouseDto> dto)
        {
            int counter = 1;
            var grouped = dto
                .OrderBy(x => x.Pallet_Number)
                .GroupBy(x => x.Pallet_Number)
                .Select(g => new
                {
                    pallet_identifier = g.Key,
                    destinationId = g.Select(x => x.Destination_Id).FirstOrDefault(),
                    sourceId = g.Select(x => x.Source_Id).FirstOrDefault(),
                    shipment_items = g.Select(ShipmentItems => new
                    {
                        sku = ShipmentItems.SKU,
                        stock_qty = ShipmentItems.Qty,
                        count = counter++
                    })
                });

            XNamespace xmlns = XNamespace.Get("");
            XNamespace xsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
            XNamespace schemaLocation = XNamespace.Get("http://www.nrf-arts.org/IXRetail/namespace/POSLog.xsd");


            XElement xml;
            xml = new XElement(xmlns + "POSLog", new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                new XAttribute(xsi + "schemaLocation", schemaLocation),
                from s in grouped
                select
                    new XElement("Transaction", new XAttribute("TrainingModeFlag", "false"),
                        new XAttribute("CancelFlag", "false"),
                        new XElement("Location", s.destinationId),
                        new XElement("WorkstationID", 32450897),
                        new XElement("SequenceNumber", "H" + DateTime.Now.ToString("HHmmss")),
                        new XElement("BusinessDayDate", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement("InventoryControlTransaction",
                            new XElement("ReceiveInventory", new XAttribute("DocumentType", "Transfer"),
                                new XElement("DocumentID", s.pallet_identifier),
                                new XElement("FromParty", s.sourceId),
                                s.shipment_items.Select(x =>
                                    new XElement("LineItem",
                                        new XElement("ItemID", x.sku),
                                        new XElement("LineItemNumber", x.count),
                                        new XElement("QuantityReceived", new XAttribute("Units", x.stock_qty),
                                            new XAttribute("UnitOfMeasureCode", "EA"), x.stock_qty)))
                            )
                        )));

            var createEnvelopeRequestWithPayloadOfString = CreateEnvelopeRequestWithPayloadOfString(xml);

            try
            {
                var send = client.PostStoreTransactions(createEnvelopeRequestWithPayloadOfString);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            client.Close();

            return xml.ToString();
        }

        public string MovementOutOfStore(List<WarehouseDto> dto)
        {
            int counter = 1;
            var grouped = dto
                .OrderBy(x => x.Pallet_Number)
                .GroupBy(x => x.Pallet_Number)
                .Select(g => new
                {
                    pallet_identifier = g.Key,
                    destinationId = g.Select(x => x.Destination_Id).FirstOrDefault(),
                    sourceId = g.Select(x => x.Source_Id).FirstOrDefault(),
                    shipment_items = g.Select(ShipmentItems => new
                    {
                        sku = ShipmentItems.SKU,
                        stock_qty = ShipmentItems.Qty,
                        count = counter++
                    })
                });

            XNamespace xmlns = XNamespace.Get("");
            XNamespace xsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
            XNamespace schemaLocation = XNamespace.Get("http://www.nrf-arts.org/IXRetail/namespace/POSLog.xsd");


            XElement xml;
            xml = new XElement(xmlns + "POSLog", new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                new XAttribute(xsi + "schemaLocation", schemaLocation),
                from s in grouped
                select
                    new XElement("Transaction", new XAttribute("TrainingModeFlag", "false"),
                        new XAttribute("CancelFlag", "false"),
                        new XElement("Location", s.destinationId),
                        new XElement("WorkstationID", 32450897),
                        new XElement("SequenceNumber", "H" + DateTime.Now.ToString("HHmmss")),
                        new XElement("BusinessDayDate", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XElement("InventoryControlTransaction",
                            new XElement("ReceiveInventory", new XAttribute("DocumentType", "Transfer"),
                                new XElement("DocumentID", s.pallet_identifier),
                                new XElement("FromParty", s.sourceId),
                                s.shipment_items.Select(x =>
                                    new XElement("LineItem",
                                        new XElement("ItemID", x.sku),
                                        new XElement("LineItemNumber", x.count),
                                        new XElement("QuantityReceived", new XAttribute("Units", x.stock_qty),
                                            new XAttribute("UnitOfMeasureCode", "EA"), x.stock_qty)))
                            )
                        )));

            var createEnvelopeRequestWithPayloadOfString = CreateEnvelopeRequestWithPayloadOfString(xml);

            try
            {
                var send = client.PostStoreTransactions(createEnvelopeRequestWithPayloadOfString);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            client.Close();

            return xml.ToString();
        }



        private static RequestWithPayloadOfString CreateEnvelopeRequestWithPayloadOfString(XElement xml)
        {
            var guid = "02F616F5-CAE8-4D3B-A827-78B720007382";
            var body = xml.ToString();
            var createEnvelopeRequestWithPayloadOfString = new RequestWithPayloadOfString
            {
                Password = "wilko",
                PasswordEncrypted = false,
                RequestID = "1",
                SessionID = new Guid(guid),
                Terminal = "1232",
                Timestamp = DateTime.Now,
                Trancode = "stdad009p",
                User = "mgb",
                Version = "3.44",
                Payload = body
            };
            return createEnvelopeRequestWithPayloadOfString;
        }
    }
}