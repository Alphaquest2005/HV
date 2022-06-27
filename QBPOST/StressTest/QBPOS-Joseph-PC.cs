using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using log4netWrapper;
using QuickBooks;
using RMSDataAccessLayer;

namespace QS2QBPost
{
    public class QBPOS //: IDisposable
    {


        private static object syncRoot = new Object();
        // private static volatile QBPOS instance;

        public static List<ItemInventoryRet> GetInventoryItemQuery(string QBCompanyFile, int days = 1)
        {
            var modxml = ItemInventoryViewModel.BuildModifiedItemInventoryQuery(days);
            var createdxml = ItemInventoryViewModel.BuildCreatedItemInventoryQuery(days);

            var modres = QBPosContext.ProcessXML(modxml.OuterXml, QBCompanyFile);
            var createdres = QBPosContext.ProcessXML(createdxml.OuterXml, QBCompanyFile);
            //if(sessionBegun == true)

            var lst = new List<ItemInventoryRet>();
            lst.AddRange(GetQBInventoryItems(modres));

            lst.AddRange(GetQBInventoryItems(createdres));

            return lst.GroupBy(x => x.ListID).Select(grp => grp.First()).ToList();
        }

        public static List<ItemInventoryRet> ValidateInventoryItemQuery(string listId, string QBCompanyFile)
        {
            var modxml = ItemInventoryViewModel.BuildItemInventoryQueryRq(listId);


            var modres = QBPosContext.ProcessXML(modxml.OuterXml, QBCompanyFile);

            //if(sessionBegun == true)

            var lst = new List<ItemInventoryRet>();
            lst.AddRange(GetQBInventoryItems(modres));



            return lst.GroupBy(x => x.ListID).Select(grp => grp.First()).ToList();
        }

        public static CustomerRet GetOrAddCustomerQuery(Patient patient, string QBCompanyFile)
        {
            var modxml = CustomerViewModel.BuildCustomerQueryRq(patient);


            var modres = QBPosContext.ProcessXML(modxml.OuterXml, QBCompanyFile);

            //if(sessionBegun == true)

            var lst = new List<CustomerRet>();
            lst.AddRange(GetQBCustomers(modres));
            if (lst.FirstOrDefault() == null)
            {
                var addreq = CustomerViewModel.BuildCustomerAddRq(patient);


                var res = QBPosContext.ProcessXML(addreq.OuterXml, QBCompanyFile);
                if (res != null)
                {
                     modxml = CustomerViewModel.BuildCustomerQueryRq(patient);
                    modres = QBPosContext.ProcessXML(modxml.OuterXml, QBCompanyFile);
                    lst.AddRange(GetQBCustomers(modres));

                }
            }

            return lst.FirstOrDefault();
        }


        public static QBResult AddSalesReceipt(SalesReceipt salesreceipt, string QBCompanyFile)
        {
            var saleXml = SalesReceiptViewModel.BuildSalesReceiptAddRq(salesreceipt);
            if (saleXml != null)
            {
                var responseXml = QBPosContext.ProcessXML(saleXml.OuterXml, QBCompanyFile);

                var addSalesReceipt = GetQBResult(responseXml);
                addSalesReceipt.Comments = salesreceipt.Comments;
                return addSalesReceipt;
            }
            return null;
        }

        private static QBResult GetQBResult(string response)
        {
            try
            {


                if (string.IsNullOrEmpty(response)) return null;

                XmlDocument outputXMLDoc = new XmlDocument();
                outputXMLDoc.LoadXml(response);
                XmlNodeList qbXMLMsgsRsNodeList = outputXMLDoc.GetElementsByTagName("SalesReceiptAddRs");
                XmlAttributeCollection rsAttributes = qbXMLMsgsRsNodeList.Item(0).Attributes;
                GetXmlErrors(rsAttributes);

                XmlNodeList vendAddRsNodeList = qbXMLMsgsRsNodeList.Item(0).ChildNodes;
                XmlNodeList vendRetNodeList = vendAddRsNodeList.Item(0).ChildNodes;
                var res = new QBResult();
                foreach (XmlNode itm in vendRetNodeList)
                {
                   
                    if (itm.Name.Equals("TrackingNumber"))
                    {
                        res.TrackingNumber = itm.InnerText;
                        continue;
                    }
                    if (itm.Name.Equals("SalesReceiptNumber"))
                    {
                        res.SalesReceiptNumber = itm.InnerText;
                        continue;
                    }
                    if (itm.Name.Equals("Comments"))
                    {
                        res.Comments = itm.InnerText;
                        continue;
                    }
                    if (res.TrackingNumber != null && res.Comments != null) break;
                }
                return res;

            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, ex.Message + ":---:" + response);
                throw new Exception(ex.Message);
            }
        }





        //public static async Task<List<ItemInventoryRet>> GetAllInventoryQuery(string QBCompanyFile)
        //{


        //    int increment = 1000;
        //    int toItemNumber = 0;
        //    int fromItemNumber = 0;
        //    string errstring = null;


        //    var lst = new List<ItemInventoryRet>();

        //    while (errstring == null)
        //    {

        //        toItemNumber += increment;

        //        var requestXml = ItemInventoryViewModel.BuildItemInventoryQueryRq(fromItemNumber, toItemNumber);
        //        fromItemNumber = toItemNumber;
        //        var res = QBPosContext.ProcessXML(requestXml.OuterXml, QBCompanyFile);
        //        var itms = GetQBInventoryItems(res);
        //        if (!itms.Any() ) break;
        //        lst.AddRange(itms);
        //    }

        //    return lst;
        //}


        private static List<CustomerRet> GetQBCustomers(string response)
        {
            try
            {
                if (string.IsNullOrEmpty(response)) return new List<CustomerRet>();
                XmlDocument outputXMLDoc = new XmlDocument();
                outputXMLDoc.LoadXml(response);
                XmlNodeList qbXMLMsgsRsNodeList = outputXMLDoc.GetElementsByTagName("CustomerQueryRs");
                XmlAttributeCollection rsAttributes = qbXMLMsgsRsNodeList.Item(0).Attributes;
                GetXmlErrors(rsAttributes);



                XmlNodeList inventoryItemRsNodeList = qbXMLMsgsRsNodeList.Item(0).ChildNodes;
                if (inventoryItemRsNodeList.Count == 0) return new List<CustomerRet>();
                var res = new List<CustomerRet>();
                foreach (XmlElement itm in inventoryItemRsNodeList)
                {

                    XmlNodeList qbitms = itm.ChildNodes;
                    var i = new CustomerRet();
                    foreach (XmlNode xi in qbitms)
                    {
                        if (xi.Name.Equals("ListID"))
                        {
                            i.ListID = xi.InnerText;
                            continue;
                        }
                        if (xi.Name.Equals("FirstName"))
                        {
                            i.FirstName = xi.InnerText;
                            continue;
                        }
                        if (xi.Name.Equals("LastName"))
                        {
                            i.LastName = xi.InnerText;
                            continue;
                        }
                        if (xi.Name.Equals("Phone"))
                        {
                            i.Phone = xi.InnerText;
                            continue;
                        }
                        if (xi.Name.Equals("CustomerDiscPercent"))
                        {
                            i.CustomerDiscPercent = Convert.ToDecimal(xi.InnerText);
                            continue;
                        }
                        if (xi.Name.Equals("CustomerDiscType"))
                        {
                            i.CustomerDiscType = (xi.InnerText == "None" ? Convert.ToByte(0) : Convert.ToByte(xi.InnerText == "Percentage"? "1" : "2" ));
                            continue;
                        }
                        if (xi.Name.Equals("IsRewardsMember"))
                        {
                            i.IsRewardsMember = Convert.ToBoolean(xi.InnerText);
                            continue;
                        }
                        if (xi.Name.Equals("LastSale"))
                        {
                            i.LastSale = xi.InnerText;
                            continue;
                        }
                        if (xi.Name.Equals("PriceLevelNumber"))
                        {
                            i.PriceLevelNumber = Convert.ToByte(xi.InnerText);
                            continue;
                        }
                        XmlNodeList RewardList = xi.SelectNodes("./Reward");
                        if (RewardList != null)
                        {
                            for (int ii = 0; ii < RewardList.Count; ii++)
                            {
                            }
                        }

                    }
                    res.Add(i);
                }
                return res;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"{ex.Message}-----|||---- {response}");
            }
        }

        private static List<ItemInventoryRet> GetQBInventoryItems(string response)
        {
            try
            {
                if (string.IsNullOrEmpty(response)) return new List<ItemInventoryRet>();
                XmlDocument outputXMLDoc = new XmlDocument();
                outputXMLDoc.LoadXml(response);
                XmlNodeList qbXMLMsgsRsNodeList = outputXMLDoc.GetElementsByTagName("ItemInventoryQueryRs");
                XmlAttributeCollection rsAttributes = qbXMLMsgsRsNodeList.Item(0).Attributes;
                GetXmlErrors(rsAttributes);



                XmlNodeList inventoryItemRsNodeList = qbXMLMsgsRsNodeList.Item(0).ChildNodes;
                if (inventoryItemRsNodeList.Count == 0) return new List<ItemInventoryRet>();
                var res = new List<ItemInventoryRet>();
                foreach (XmlElement itm in inventoryItemRsNodeList)
                {

                    XmlNodeList qbitms = itm.ChildNodes;
                    var i = new ItemInventoryRet();
                    foreach (XmlNode xi in qbitms)
                    {
                        if (xi.Name.Equals("ListID"))
                        {
                            i.ListID = xi.InnerText;
                            continue;
                        }
                        if (xi.Name.Equals("ALU"))
                        {
                            i.ALU = xi.InnerText;
                            continue;
                        }
                        if (xi.Name.Equals("Attribute"))
                        {
                            i.Attribute = xi.InnerText;
                            continue;
                        }
                        if (xi.Name.Equals("Desc1"))
                        {
                            i.Desc1 = xi.InnerText;
                            continue;
                        }
                        if (xi.Name.Equals("Desc2"))
                        {
                            i.Desc2 = xi.InnerText;
                            continue;
                        }
                        if (xi.Name.Equals("ItemNumber"))
                        {
                            i.ItemNumber = Convert.ToInt32(xi.InnerText);
                            continue;
                        }
                        if (xi.Name.Equals("Price1"))
                        {
                            i.Price1 = Convert.ToDecimal(xi.InnerText);
                            continue;
                        }
                        if (xi.Name.Equals("QuantityOnHand"))
                        {
                            i.QuantityOnHand = Convert.ToDecimal(xi.InnerText);
                            continue;
                        }
                        if (xi.Name.Equals("Size"))
                        {
                            i.Size = xi.InnerText;
                            continue;
                        }
                        if (xi.Name.Equals("TaxCode"))
                        {
                            i.TaxCode = xi.InnerText;
                            continue;
                        }
                        if (xi.Name.Equals("UnitOfMeasure"))
                        {
                            i.UnitOfMeasure = xi.InnerText;
                            continue;
                        }

                    }
                    res.Add(i);
                }
                return res;
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, ex.Message);
                throw ex;
            }
        }

        private static void GetXmlErrors(XmlAttributeCollection rsAttributes)
        {
            //get the status Code, info and Severity

            if (!"0,1".Contains(rsAttributes.GetNamedItem("statusCode").Value))
            {
                string retStatusCode = rsAttributes.GetNamedItem("statusCode").Value;
                string retStatusSeverity = rsAttributes.GetNamedItem("statusSeverity").Value;
                string retStatusMessage = rsAttributes.GetNamedItem("statusMessage").Value;
                throw new ApplicationException(string.Format("statusCode = {0}, statusSeverity = {1}, statusMessage = {2}",
                    retStatusCode, retStatusSeverity, retStatusMessage));
            }
        }


    }
}


