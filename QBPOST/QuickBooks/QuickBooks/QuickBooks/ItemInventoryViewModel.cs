using System;
using System.Xml;

namespace QuickBooks
{
    public class ItemInventoryViewModel
    {
        
        public static XmlDocument BuildModifiedItemInventoryQuery(int days)
        {
            
            XmlDocument inputXMLDoc = new XmlDocument();
            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbposxml", "version=\"1.0\""));
            XmlElement qbXML = inputXMLDoc.CreateElement("QBPOSXML");
            inputXMLDoc.AppendChild(qbXML);
            XmlElement qbXMLMsgsRq = inputXMLDoc.CreateElement("QBPOSXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
            XmlElement inventoryQueryRq = inputXMLDoc.CreateElement("ItemInventoryQueryRq");
            qbXMLMsgsRq.AppendChild(inventoryQueryRq);
            inventoryQueryRq.SetAttribute("requestID", "1");


            XmlElement itemNumberRange = inputXMLDoc.CreateElement("TimeModifiedRangeFilter");
            itemNumberRange.AppendChild(inputXMLDoc.CreateElement("FromTimeModified")).InnerText = DateTime.Now.AddMinutes(-45).ToString("yyyy-MM-dd");
            itemNumberRange.AppendChild(inputXMLDoc.CreateElement("ToTimeModified")).InnerText = DateTime.Now.ToString("yyyy-MM-dd");
            inventoryQueryRq.AppendChild(itemNumberRange);

            return inputXMLDoc;
        }

        public static XmlDocument BuildModifiedItemInventoryQuery(string itemNumber)
        {

            XmlDocument inputXMLDoc = new XmlDocument();
            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbposxml", "version=\"1.0\""));
            XmlElement qbXML = inputXMLDoc.CreateElement("QBPOSXML");
            inputXMLDoc.AppendChild(qbXML);
            XmlElement qbXMLMsgsRq = inputXMLDoc.CreateElement("QBPOSXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
            XmlElement inventoryQueryRq = inputXMLDoc.CreateElement("ItemInventoryQueryRq");
            qbXMLMsgsRq.AppendChild(inventoryQueryRq);
            inventoryQueryRq.SetAttribute("requestID", "1");


            //Create ItemNumberFilter aggregate and fill in field values for it
            XmlElement ItemNumberFilter = inputXMLDoc.CreateElement("ItemNumberFilter");

            
            ItemNumberFilter.AppendChild(inputXMLDoc.CreateElement("ItemNumber")).InnerText = itemNumber; 
            ItemNumberFilter.AppendChild(inputXMLDoc.CreateElement("MatchNumericCriterion")).InnerText = "Equal";
            inventoryQueryRq.AppendChild(ItemNumberFilter);

            return inputXMLDoc;
        }

        public static XmlDocument BuildCreatedItemInventoryQuery(int days)
        {
            try
            {


                XmlDocument inputXMLDoc = new XmlDocument();
                inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
                inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbposxml", "version=\"1.0\""));
                XmlElement qbXML = inputXMLDoc.CreateElement("QBPOSXML");
                inputXMLDoc.AppendChild(qbXML);
                XmlElement qbXMLMsgsRq = inputXMLDoc.CreateElement("QBPOSXMLMsgsRq");
                qbXML.AppendChild(qbXMLMsgsRq);
                qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
                XmlElement inventoryQueryRq = inputXMLDoc.CreateElement("ItemInventoryQueryRq");
                qbXMLMsgsRq.AppendChild(inventoryQueryRq);
                inventoryQueryRq.SetAttribute("requestID", "1");


                XmlElement itemNumberRange = inputXMLDoc.CreateElement("TimeCreatedRangeFilter");
                itemNumberRange.AppendChild(inputXMLDoc.CreateElement("FromTimeCreated")).InnerText =
                    DateTime.Now.AddMinutes(-45).ToString("yyyy-MM-dd");
                itemNumberRange.AppendChild(inputXMLDoc.CreateElement("ToTimeCreated")).InnerText =
                    DateTime.Now.ToString("yyyy-MM-dd");
                inventoryQueryRq.AppendChild(itemNumberRange);

                return inputXMLDoc;
            }
            catch (Exception)
            {

                throw;
            }

        }



        public static XmlDocument BuildItemInventoryQueryRq(string listId)
        {
            XmlDocument inputXMLDoc = new XmlDocument();
            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbposxml", "version=\"1.0\""));
            XmlElement qbXML = inputXMLDoc.CreateElement("QBPOSXML");
            inputXMLDoc.AppendChild(qbXML);
            XmlElement qbXMLMsgsRq = inputXMLDoc.CreateElement("QBPOSXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
            XmlElement inventoryQueryRq = inputXMLDoc.CreateElement("ItemInventoryQueryRq");
            qbXMLMsgsRq.AppendChild(inventoryQueryRq);
            inventoryQueryRq.SetAttribute("requestID", "1");

            inventoryQueryRq.AppendChild(MakeSimpleElem(inputXMLDoc, "ListID", listId));

            //XmlElement itemNumberRange = inputXMLDoc.CreateElement("ItemNumberRangeFilter");
            //itemNumberRange.AppendChild(inputXMLDoc.CreateElement("FromItemNumber")).InnerText = FromItemNumber.ToString();
            //itemNumberRange.AppendChild(inputXMLDoc.CreateElement("ToItemNumber")).InnerText = ToItemNumber.ToString();
            //inventoryQueryRq.AppendChild(itemNumberRange);

            return inputXMLDoc;
        }

        public static XmlDocument BuildCustomerQueryRq(string FromCustomerNumber, string ToCustomerNumber)
        {
            XmlDocument inputXMLDoc = new XmlDocument();
            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbposxml", "version=\"1.0\""));
            XmlElement qbXML = inputXMLDoc.CreateElement("QBPOSXML");
            inputXMLDoc.AppendChild(qbXML);
            XmlElement qbXMLMsgsRq = inputXMLDoc.CreateElement("QBPOSXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
            XmlElement inventoryQueryRq = inputXMLDoc.CreateElement("CustomerQueryRq");
            qbXMLMsgsRq.AppendChild(inventoryQueryRq);
            inventoryQueryRq.SetAttribute("requestID", "1");


            XmlElement itemNumberRange = inputXMLDoc.CreateElement("PhoneRangeFilter");
            itemNumberRange.AppendChild(inputXMLDoc.CreateElement("FromPhone")).InnerText = FromCustomerNumber.ToString();
            itemNumberRange.AppendChild(inputXMLDoc.CreateElement("ToPhone")).InnerText = ToCustomerNumber.ToString();
            inventoryQueryRq.AppendChild(itemNumberRange);

            return inputXMLDoc;
        }

        private static XmlElement MakeSimpleElem(XmlDocument doc, string tagName, string tagVal)
        {
            XmlElement elem = doc.CreateElement(tagName);
            elem.InnerText = tagVal;
            return elem;
        }


    }
}
