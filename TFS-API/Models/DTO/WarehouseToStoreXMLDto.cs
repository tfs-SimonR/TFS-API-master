using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace TFS_API.Models.DTO
{
    public class WarehouseToStoreXMLDto
    {
		[XmlRoot(ElementName = "Location")]
		public class Location
		{
			[XmlAttribute(AttributeName = "TypeCode")]
			public string TypeCode { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "QuantityOrdered")]
		public class QuantityOrdered
		{
			[XmlAttribute(AttributeName = "Units")]
			public string Units { get; set; }
			[XmlAttribute(AttributeName = "UnitOfMeasureCode")]
			public string UnitOfMeasureCode { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "FromSubLoc")]
		public class FromSubLoc
		{
			[XmlAttribute(AttributeName = "LocationType")]
			public string LocationType { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "LineItem")]
		public class LineItem
		{
			[XmlElement(ElementName = "ItemID")]
			public string ItemID { get; set; }
			[XmlElement(ElementName = "CartonNumber")]
			public string CartonNumber { get; set; }
			[XmlElement(ElementName = "LineItemNumber")]
			public string LineItemNumber { get; set; }
			[XmlElement(ElementName = "QuantityOrdered")]
			public QuantityOrdered QuantityOrdered { get; set; }
			[XmlElement(ElementName = "FromSubLoc")]
			public FromSubLoc FromSubLoc { get; set; }
		}

		[XmlRoot(ElementName = "Courier")]
		public class Courier
		{
			[XmlAttribute(AttributeName = "ServiceLevelCode")]
			public string ServiceLevelCode { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "Delivery")]
		public class Delivery
		{
			[XmlElement(ElementName = "CartonNumber")]
			public string CartonNumber { get; set; }
			[XmlElement(ElementName = "TrackingNumber")]
			public string TrackingNumber { get; set; }
			[XmlElement(ElementName = "Courier")]
			public Courier Courier { get; set; }
		}

		[XmlRoot(ElementName = "Transfer")]
		public class Transfer
		{
			[XmlElement(ElementName = "DocumentID")]
			public string DocumentID { get; set; }
			[XmlElement(ElementName = "ToParty")]
			public string ToParty { get; set; }
			[XmlElement(ElementName = "LineItem")]
			public List<LineItem> LineItem { get; set; }
			[XmlElement(ElementName = "Delivery")]
			public Delivery Delivery { get; set; }
			[XmlAttribute(AttributeName = "DocumentType")]
			public string DocumentType { get; set; }
		}

		[XmlRoot(ElementName = "InventoryControlTransaction")]
		public class InventoryControlTransaction
		{
			[XmlElement(ElementName = "Transfer")]
			public Transfer Transfer { get; set; }
		}

		[XmlRoot(ElementName = "Transaction")]
		public class Transaction
		{
			[XmlElement(ElementName = "Location")]
			public Location Location { get; set; }
			[XmlElement(ElementName = "WorkstationID")]
			public string WorkstationID { get; set; }
			[XmlElement(ElementName = "TillID")]
			public string TillID { get; set; }
			[XmlElement(ElementName = "SequenceNumber")]
			public string SequenceNumber { get; set; }
			[XmlElement(ElementName = "BeginDateTime")]
			public string BeginDateTime { get; set; }
			[XmlElement(ElementName = "EndDateTime")]
			public string EndDateTime { get; set; }
			[XmlElement(ElementName = "BusinessDayDate")]
			public string BusinessDayDate { get; set; }
			[XmlElement(ElementName = "OperatorID")]
			public string OperatorID { get; set; }
			[XmlElement(ElementName = "InventoryControlTransaction")]
			public InventoryControlTransaction InventoryControlTransaction { get; set; }
			[XmlAttribute(AttributeName = "TrainingModeFlag")]
			public string TrainingModeFlag { get; set; }
			[XmlAttribute(AttributeName = "CancelFlag")]
			public string CancelFlag { get; set; }
		}

		[XmlRoot(ElementName = "POSLog")]
		public class POSLog
		{
			[XmlElement(ElementName = "Transaction")]
			public Transaction Transaction { get; set; }
			[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Xsi { get; set; }
			[XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
			public string SchemaLocation { get; set; }
		}
	}
}