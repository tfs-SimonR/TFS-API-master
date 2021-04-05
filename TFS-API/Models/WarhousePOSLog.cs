using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace TFS_API.Models
{
    public class WarhousePOSLog
	{
		
         [XmlRoot(ElementName = "Location")]
		public class Location
		{
			[XmlAttribute(AttributeName = "TypeCode")]
			public string TypeCode { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "QuantityReceived")]
		public class QuantityReceived
		{
			[XmlAttribute(AttributeName = "Units")]
			public string Units { get; set; }
			[XmlAttribute(AttributeName = "UnitOfMeasureCode")]
			public string UnitOfMeasureCode { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "LineItem")]
		public class LineItem
		{
			[XmlElement(ElementName = "ItemID")]
			public string ItemID { get; set; }
			[XmlElement(ElementName = "LineItemNumber")]
			public string LineItemNumber { get; set; }
			[XmlElement(ElementName = "QuantityReceived")]
			public QuantityReceived QuantityReceived { get; set; }
		}

		[XmlRoot(ElementName = "ReceiveInventory")]
		public class ReceiveInventory
		{
			[XmlElement(ElementName = "DocumentID")]
			public string DocumentID { get; set; }
			[XmlElement(ElementName = "FromParty")]
			public string FromParty { get; set; }
			[XmlElement(ElementName = "LineItem")]
			public List<LineItem> LineItem { get; set; }
			[XmlAttribute(AttributeName = "DocumentType")]
			public string DocumentType { get; set; }
		}

		[XmlRoot(ElementName = "InventoryControlTransaction")]
		public class InventoryControlTransaction
		{
			[XmlElement(ElementName = "ReceiveInventory")]
			public ReceiveInventory ReceiveInventory { get; set; }
		}

		[XmlRoot(ElementName = "Transaction")]
		public class Transaction
		{
			[XmlElement(ElementName = "Location")]
			public Location Location { get; set; }
			[XmlElement(ElementName = "WorkstationID")]
			public string WorkstationID { get; set; }
			[XmlElement(ElementName = "SequenceNumber")]
			public string SequenceNumber { get; set; }
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
