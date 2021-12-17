using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using RMSDataAccessLayer;

namespace QuickBooks
{
    public class CustomerViewModel
    {
        private static XmlElement MakeSimpleElem(XmlDocument doc, string tagName, string tagVal)
        {
            XmlElement elem = doc.CreateElement(tagName);
            elem.InnerText = tagVal;
            return elem;
        }

        //public void DoCustomerAdd(Customer customer)
        //{
        //    bool sessionBegun = false;
        //    bool connectionOpen = false;
        //    RequestProcessor2 rp = null;

        //    try
        //    {
        //        //Create the Request Processor object
        //        rp = new RequestProcessor2();

        //        //Create the XML document to hold our request
        //        XmlDocument requestXmlDoc = new XmlDocument();
        //        //Add the prolog processing instructions
        //        requestXmlDoc.AppendChild(requestXmlDoc.CreateXmlDeclaration("1.0", null, null));
        //        requestXmlDoc.AppendChild(requestXmlDoc.CreateProcessingInstruction("qbxml", "version=\"3.0\""));

        //        //Create the outer request envelope tag
        //        XmlElement outer = requestXmlDoc.CreateElement("QBXML");
        //        requestXmlDoc.AppendChild(outer);

        //        //Create the inner request envelope & any needed attributes
        //        XmlElement inner = requestXmlDoc.CreateElement("QBXMLMsgsRq");
        //        outer.AppendChild(inner);
        //        inner.SetAttribute("onError", "stopOnError");
        //        BuildCustomerAddRq(requestXmlDoc, inner);

        //        //Connect to QuickBooks and begin a session
        //        rp.OpenConnection2("", "Sample Code from OSR", localQBD);
        //        connectionOpen = true;
        //        string ticket = rp.BeginSession("", QBFileMode.qbFileOpenDoNotCare);
        //        sessionBegun = true;

        //        //Send the request and get the response from QuickBooks
        //        string responseStr = rp.ProcessRequest(ticket, requestXmlDoc.OuterXml);

        //        //End the session and close the connection to QuickBooks
        //        rp.EndSession(ticket);
        //        sessionBegun = false;
        //        rp.CloseConnection();
        //        connectionOpen = false;

        //        WalkCustomerAddRs(responseStr);

        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message, "Error");
        //        if (sessionBegun)
        //        {
        //            sessionManager.EndSession();
        //        }
        //        if (connectionOpen)
        //        {
        //            sessionManager.CloseConnection();
        //        }
        //    }
        //}


        public static XmlDocument BuildCustomerAddRq(Patient patient)
        {
            //Create the XML document to hold our request
            XmlDocument doc = new XmlDocument();
            //Add the prolog processing instructions
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", null, null));
            doc.AppendChild(doc.CreateProcessingInstruction("qbposxml", "version=\"1.0\""));

            //Create the outer request envelope tag
            XmlElement outer = doc.CreateElement("QBPOSXML");
            doc.AppendChild(outer);

            //Create the inner request envelope & any needed attributes
            XmlElement inner = doc.CreateElement("QBPOSXMLMsgsRq");
            outer.AppendChild(inner);
            inner.SetAttribute("onError", "stopOnError");

            //Create CustomerAddRq aggregate and fill in field values for it
            XmlElement CustomerAddRq = doc.CreateElement("CustomerAddRq");
            inner.AppendChild(CustomerAddRq);

            //Create CustomerAdd aggregate and fill in field values for it
            XmlElement CustomerAdd = doc.CreateElement("CustomerAdd");
            CustomerAddRq.AppendChild(CustomerAdd);
            
            //Set field value for CustomerID <!-- optional -->
            CustomerAdd.AppendChild(MakeSimpleElem(doc, "CustomerID", patient.IDCardInfo.FirstOrDefault()?.CardId.ToString()??"Unknown"));
            
            //Set field value for FirstName <!-- optional -->
            CustomerAdd.AppendChild(MakeSimpleElem(doc, "FirstName", patient.FirstName));
            //Set field value for LastName <!-- required -->
            CustomerAdd.AppendChild(MakeSimpleElem(doc, "LastName", patient.LastName));
            //Set field value for Phone <!-- optional -->
            CustomerAdd.AppendChild(MakeSimpleElem(doc, "Phone", patient.PhoneNumber??"000-0000"));
           
            
            
            
            //Set field value for PriceLevelNumber <!-- optional -->
            //CustomerAdd.AppendChild(MakeSimpleElem(doc, "PriceLevelNumber", "1"));
            CustomerAdd.AppendChild(MakeSimpleElem(doc, "IsRewardsMember", "1"));
            //Set field value for CustomerDiscPercent <!-- optional -->
            CustomerAdd.AppendChild(MakeSimpleElem(doc, "CustomerDiscPercent", patient.PatientMemberships.Select(x => x.MembershipType.Discount.ToString(CultureInfo.InvariantCulture)).DefaultIfEmpty("0").First()));
            //Set field value for CustomerDiscType <!-- optional -->
            CustomerAdd.AppendChild(MakeSimpleElem(doc, "CustomerDiscType", "Percentage"));

            //CustomerAdd.AppendChild(MakeSimpleElem(doc, "LastSale", "0"));

            //CustomerAdd.AppendChild(MakeSimpleElem(doc, "PriceLevelNumber", "1"));


            //Create BillAddress aggregate and fill in field values for it
            XmlElement BillAddress = doc.CreateElement("BillAddress");
            CustomerAdd.AppendChild(BillAddress);
            BillAddress.AppendChild(MakeSimpleElem(doc, "Street", patient.Address??"Unknown"));
            
            ////Set field value for City <!-- optional -->
            //BillAddress.AppendChild(MakeSimpleElem(doc, "City", patient.));
            ////Set field value for Country <!-- optional -->
            //BillAddress.AppendChild(MakeSimpleElem(doc, "Country", "ab"));
            ////Set field value for PostalCode <!-- optional -->
            //BillAddress.AppendChild(MakeSimpleElem(doc, "PostalCode", "ab"));
            ////Set field value for State <!-- optional -->


            return doc;

        }

        public static XmlDocument BuildCustomerQueryRq(Patient patient)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", null, null));
            doc.AppendChild(doc.CreateProcessingInstruction("qbposxml", "version=\"3.0\""));
            XmlElement qbXML = doc.CreateElement("QBPOSXML");
            doc.AppendChild(qbXML);
            XmlElement qbXMLMsgsRq = doc.CreateElement("QBPOSXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");

            
            //Create CustomerQueryRq aggregate and fill in field values for it
            XmlElement CustomerQueryRq = doc.CreateElement("CustomerQueryRq");
            qbXMLMsgsRq.AppendChild(CustomerQueryRq);


            //XmlElement itemNumberRange = inputXMLDoc.CreateElement("ItemNumberRangeFilter");
            //itemNumberRange.AppendChild(inputXMLDoc.CreateElement("FromItemNumber")).InnerText = FromItemNumber.ToString();
            //itemNumberRange.AppendChild(inputXMLDoc.CreateElement("ToItemNumber")).InnerText = ToItemNumber.ToString();
            XmlElement FirstNameFilter = doc.CreateElement("FirstNameFilter");
            CustomerQueryRq.AppendChild(FirstNameFilter);
            //Set field value for MatchStringCriterion <!-- required -->
            FirstNameFilter.AppendChild(MakeSimpleElem(doc, "MatchStringCriterion", "Equal"));
            //Set field value for CustomerID <!-- required -->
            FirstNameFilter.AppendChild(MakeSimpleElem(doc, "FirstName", patient.FirstName));

            XmlElement LastNameFilter = doc.CreateElement("LastNameFilter");
            CustomerQueryRq.AppendChild(LastNameFilter);
            //Set field value for MatchStringCriterion <!-- required -->
            LastNameFilter.AppendChild(MakeSimpleElem(doc, "MatchStringCriterion", "Equal"));
            //Set field value for CustomerID <!-- required -->
            LastNameFilter.AppendChild(MakeSimpleElem(doc, "LastName", patient.LastName));

            XmlElement PhoneFilter = doc.CreateElement("PhoneFilter");
            CustomerQueryRq.AppendChild(PhoneFilter);
            //Set field value for MatchStringCriterion <!-- required -->
            PhoneFilter.AppendChild(MakeSimpleElem(doc, "MatchStringCriterion", "Equal"));
            //Set field value for CustomerID <!-- required -->
            PhoneFilter.AppendChild(MakeSimpleElem(doc, "Phone", patient.PhoneNumber??"000-0000"));


            return doc;
        }




        void WalkCustomerAddRs(string response)
        {
            //Parse the response XML string into an XmlDocument
            XmlDocument responseXmlDoc = new XmlDocument();
            responseXmlDoc.LoadXml(response);

            //Get the response for our request
            XmlNodeList CustomerAddRsList = responseXmlDoc.GetElementsByTagName("CustomerAddRs");
            if (CustomerAddRsList.Count == 1) //Should always be true since we only did one request in this sample
            {
                XmlNode responseNode = CustomerAddRsList.Item(0);
                //Check the status code, info, and severity
                XmlAttributeCollection rsAttributes = responseNode.Attributes;
                string statusCode = rsAttributes.GetNamedItem("statusCode").Value;
                string statusSeverity = rsAttributes.GetNamedItem("statusSeverity").Value;
                string statusMessage = rsAttributes.GetNamedItem("statusMessage").Value;

                //status code = 0 all OK, > 0 is warning
                if (Convert.ToInt32(statusCode) >= 0)
                {
                    XmlNodeList CustomerRetList = responseNode.SelectNodes("//CustomerRet");//XPath Query
                    for (int i = 0; i < CustomerRetList.Count; i++)
                    {
                        XmlNode CustomerRet = CustomerRetList.Item(i);
                        WalkCustomerRet(CustomerRet);
                    }
                }
            }
        }



        void WalkCustomerRet(XmlNode CustomerRet)
        {
            if (CustomerRet == null) return;

            //Go through all the elements of CustomerRet
            //Get value of ListID
            if (CustomerRet.SelectSingleNode("./ListID") != null)
            {
                string ListID = CustomerRet.SelectSingleNode("./ListID").InnerText;
            }
            //Get value of TimeCreated
            if (CustomerRet.SelectSingleNode("./TimeCreated") != null)
            {
                string TimeCreated = CustomerRet.SelectSingleNode("./TimeCreated").InnerText;
            }
            //Get value of TimeModified
            if (CustomerRet.SelectSingleNode("./TimeModified") != null)
            {
                string TimeModified = CustomerRet.SelectSingleNode("./TimeModified").InnerText;
            }
            //Get value of AccountBalance
            if (CustomerRet.SelectSingleNode("./AccountBalance") != null)
            {
                string AccountBalance = CustomerRet.SelectSingleNode("./AccountBalance").InnerText;
            }
            //Get value of AccountLimit
            if (CustomerRet.SelectSingleNode("./AccountLimit") != null)
            {
                string AccountLimit = CustomerRet.SelectSingleNode("./AccountLimit").InnerText;
            }
            //Get value of AmountPastDue
            if (CustomerRet.SelectSingleNode("./AmountPastDue") != null)
            {
                string AmountPastDue = CustomerRet.SelectSingleNode("./AmountPastDue").InnerText;
            }
            //Get value of CompanyName
            if (CustomerRet.SelectSingleNode("./CompanyName") != null)
            {
                string CompanyName = CustomerRet.SelectSingleNode("./CompanyName").InnerText;
            }
            //Get value of CustomerID
            if (CustomerRet.SelectSingleNode("./CustomerID") != null)
            {
                string CustomerID = CustomerRet.SelectSingleNode("./CustomerID").InnerText;
            }
            //Get value of CustomerDiscPercent
            if (CustomerRet.SelectSingleNode("./CustomerDiscPercent") != null)
            {
                string CustomerDiscPercent = CustomerRet.SelectSingleNode("./CustomerDiscPercent").InnerText;
            }
            //Get value of CustomerDiscType
            if (CustomerRet.SelectSingleNode("./CustomerDiscType") != null)
            {
                string CustomerDiscType = CustomerRet.SelectSingleNode("./CustomerDiscType").InnerText;
            }
            //Get value of CustomerType
            if (CustomerRet.SelectSingleNode("./CustomerType") != null)
            {
                string CustomerType = CustomerRet.SelectSingleNode("./CustomerType").InnerText;
            }
            //Get value of EMail
            if (CustomerRet.SelectSingleNode("./EMail") != null)
            {
                string EMail = CustomerRet.SelectSingleNode("./EMail").InnerText;
            }
            //Get value of IsOkToEMail
            if (CustomerRet.SelectSingleNode("./IsOkToEMail") != null)
            {
                string IsOkToEMail = CustomerRet.SelectSingleNode("./IsOkToEMail").InnerText;
            }
            //Get value of FirstName
            if (CustomerRet.SelectSingleNode("./FirstName") != null)
            {
                string FirstName = CustomerRet.SelectSingleNode("./FirstName").InnerText;
            }
            //Get value of FullName
            if (CustomerRet.SelectSingleNode("./FullName") != null)
            {
                string FullName = CustomerRet.SelectSingleNode("./FullName").InnerText;
            }
            //Get value of IsAcceptingChecks
            if (CustomerRet.SelectSingleNode("./IsAcceptingChecks") != null)
            {
                string IsAcceptingChecks = CustomerRet.SelectSingleNode("./IsAcceptingChecks").InnerText;
            }
            //Get value of IsUsingChargeAccount
            if (CustomerRet.SelectSingleNode("./IsUsingChargeAccount") != null)
            {
                string IsUsingChargeAccount = CustomerRet.SelectSingleNode("./IsUsingChargeAccount").InnerText;
            }
            //Get value of IsUsingWithQB
            if (CustomerRet.SelectSingleNode("./IsUsingWithQB") != null)
            {
                string IsUsingWithQB = CustomerRet.SelectSingleNode("./IsUsingWithQB").InnerText;
            }
            //Get value of IsRewardsMember
            if (CustomerRet.SelectSingleNode("./IsRewardsMember") != null)
            {
                string IsRewardsMember = CustomerRet.SelectSingleNode("./IsRewardsMember").InnerText;
            }
            //Get value of IsNoShipToBilling
            if (CustomerRet.SelectSingleNode("./IsNoShipToBilling") != null)
            {
                string IsNoShipToBilling = CustomerRet.SelectSingleNode("./IsNoShipToBilling").InnerText;
            }
            //Get value of LastName
            if (CustomerRet.SelectSingleNode("./LastName") != null)
            {
                string LastName = CustomerRet.SelectSingleNode("./LastName").InnerText;
            }
            //Get value of LastSale
            if (CustomerRet.SelectSingleNode("./LastSale") != null)
            {
                string LastSale = CustomerRet.SelectSingleNode("./LastSale").InnerText;
            }
            //Get value of Notes
            if (CustomerRet.SelectSingleNode("./Notes") != null)
            {
                string Notes = CustomerRet.SelectSingleNode("./Notes").InnerText;
            }
            //Get value of Phone
            if (CustomerRet.SelectSingleNode("./Phone") != null)
            {
                string Phone = CustomerRet.SelectSingleNode("./Phone").InnerText;
            }
            //Get value of Phone2
            if (CustomerRet.SelectSingleNode("./Phone2") != null)
            {
                string Phone2 = CustomerRet.SelectSingleNode("./Phone2").InnerText;
            }
            //Get value of Phone3
            if (CustomerRet.SelectSingleNode("./Phone3") != null)
            {
                string Phone3 = CustomerRet.SelectSingleNode("./Phone3").InnerText;
            }
            //Get value of Phone4
            if (CustomerRet.SelectSingleNode("./Phone4") != null)
            {
                string Phone4 = CustomerRet.SelectSingleNode("./Phone4").InnerText;
            }
            //Get value of PriceLevelNumber
            if (CustomerRet.SelectSingleNode("./PriceLevelNumber") != null)
            {
                string PriceLevelNumber = CustomerRet.SelectSingleNode("./PriceLevelNumber").InnerText;
            }
            //Get value of Salutation
            if (CustomerRet.SelectSingleNode("./Salutation") != null)
            {
                string Salutation = CustomerRet.SelectSingleNode("./Salutation").InnerText;
            }
            //Get value of StoreExchangeStatus
            if (CustomerRet.SelectSingleNode("./StoreExchangeStatus") != null)
            {
                string StoreExchangeStatus = CustomerRet.SelectSingleNode("./StoreExchangeStatus").InnerText;
            }
            //Get value of TaxCategory
            if (CustomerRet.SelectSingleNode("./TaxCategory") != null)
            {
                string TaxCategory = CustomerRet.SelectSingleNode("./TaxCategory").InnerText;
            }
            //Get value of WebNumber
            if (CustomerRet.SelectSingleNode("./WebNumber") != null)
            {
                string WebNumber = CustomerRet.SelectSingleNode("./WebNumber").InnerText;
            }
            //Get all field values for BillAddress aggregate
            XmlNode BillAddress = CustomerRet.SelectSingleNode("./BillAddress");
            if (BillAddress != null)
            {
                //Get value of City
                if (CustomerRet.SelectSingleNode("./BillAddress/City") != null)
                {
                    string City = CustomerRet.SelectSingleNode("./BillAddress/City").InnerText;
                }
                //Get value of Country
                if (CustomerRet.SelectSingleNode("./BillAddress/Country") != null)
                {
                    string Country = CustomerRet.SelectSingleNode("./BillAddress/Country").InnerText;
                }
                //Get value of PostalCode
                if (CustomerRet.SelectSingleNode("./BillAddress/PostalCode") != null)
                {
                    string PostalCode = CustomerRet.SelectSingleNode("./BillAddress/PostalCode").InnerText;
                }
                //Get value of State
                if (CustomerRet.SelectSingleNode("./BillAddress/State") != null)
                {
                    string State = CustomerRet.SelectSingleNode("./BillAddress/State").InnerText;
                }
                //Get value of Street
                if (CustomerRet.SelectSingleNode("./BillAddress/Street") != null)
                {
                    string Street = CustomerRet.SelectSingleNode("./BillAddress/Street").InnerText;
                }
                //Get value of Street2
                if (CustomerRet.SelectSingleNode("./BillAddress/Street2") != null)
                {
                    string Street2 = CustomerRet.SelectSingleNode("./BillAddress/Street2").InnerText;
                }
            }
            //Done with field values for BillAddress aggregate

            //Get value of DefaultShipAddress
            if (CustomerRet.SelectSingleNode("./DefaultShipAddress") != null)
            {
                string DefaultShipAddress = CustomerRet.SelectSingleNode("./DefaultShipAddress").InnerText;
            }
            //Walk list of ShipAddress aggregates
            XmlNodeList ShipAddressList = CustomerRet.SelectNodes("./ShipAddress");
            if (ShipAddressList != null)
            {
                for (int i = 0; i < ShipAddressList.Count; i++)
                {
                    XmlNode ShipAddress = ShipAddressList.Item(i);
                    //Get value of AddressName
                    if (ShipAddress.SelectSingleNode("./AddressName") != null)
                    {
                        string AddressName = ShipAddress.SelectSingleNode("./AddressName").InnerText;
                    }
                    //Get value of CompanyName
                    if (ShipAddress.SelectSingleNode("./CompanyName") != null)
                    {
                        string CompanyName = ShipAddress.SelectSingleNode("./CompanyName").InnerText;
                    }
                    //Get value of FullName
                    if (ShipAddress.SelectSingleNode("./FullName") != null)
                    {
                        string FullName = ShipAddress.SelectSingleNode("./FullName").InnerText;
                    }
                    //Get value of City
                    if (ShipAddress.SelectSingleNode("./City") != null)
                    {
                        string City = ShipAddress.SelectSingleNode("./City").InnerText;
                    }
                    //Get value of Country
                    if (ShipAddress.SelectSingleNode("./Country") != null)
                    {
                        string Country = ShipAddress.SelectSingleNode("./Country").InnerText;
                    }
                    //Get value of PostalCode
                    if (ShipAddress.SelectSingleNode("./PostalCode") != null)
                    {
                        string PostalCode = ShipAddress.SelectSingleNode("./PostalCode").InnerText;
                    }
                    //Get value of State
                    if (ShipAddress.SelectSingleNode("./State") != null)
                    {
                        string State = ShipAddress.SelectSingleNode("./State").InnerText;
                    }
                    //Get value of Street
                    if (ShipAddress.SelectSingleNode("./Street") != null)
                    {
                        string Street = ShipAddress.SelectSingleNode("./Street").InnerText;
                    }
                    //Get value of Street2
                    if (ShipAddress.SelectSingleNode("./Street2") != null)
                    {
                        string Street2 = ShipAddress.SelectSingleNode("./Street2").InnerText;
                    }
                }
            }

            //Walk list of Reward aggregates
            XmlNodeList RewardList = CustomerRet.SelectNodes("./Reward");
            if (RewardList != null)
            {
                for (int i = 0; i < RewardList.Count; i++)
                {
                    XmlNode Reward = RewardList.Item(i);
                    //Get value of RewardAmount
                    if (Reward.SelectSingleNode("./RewardAmount") != null)
                    {
                        string RewardAmount = Reward.SelectSingleNode("./RewardAmount").InnerText;
                    }
                    //Get value of RewardPercent
                    if (Reward.SelectSingleNode("./RewardPercent") != null)
                    {
                        string RewardPercent = Reward.SelectSingleNode("./RewardPercent").InnerText;
                    }
                    //Get value of EarnedDate
                    if (Reward.SelectSingleNode("./EarnedDate") != null)
                    {
                        string EarnedDate = Reward.SelectSingleNode("./EarnedDate").InnerText;
                    }
                    //Get value of MatureDate
                    if (Reward.SelectSingleNode("./MatureDate") != null)
                    {
                        string MatureDate = Reward.SelectSingleNode("./MatureDate").InnerText;
                    }
                    //Get value of ExpirationDate
                    if (Reward.SelectSingleNode("./ExpirationDate") != null)
                    {
                        string ExpirationDate = Reward.SelectSingleNode("./ExpirationDate").InnerText;
                    }
                }
            }

            //Walk list of DataExtRet aggregates
            XmlNodeList DataExtRetList = CustomerRet.SelectNodes("./DataExtRet");
            if (DataExtRetList != null)
            {
                for (int i = 0; i < DataExtRetList.Count; i++)
                {
                    XmlNode DataExtRet = DataExtRetList.Item(i);
                    //Get value of OwnerID
                    string OwnerID = DataExtRet.SelectSingleNode("./OwnerID").InnerText;
                    //Get value of DataExtName
                    string DataExtName = DataExtRet.SelectSingleNode("./DataExtName").InnerText;
                    //Get value of DataExtType
                    string DataExtType = DataExtRet.SelectSingleNode("./DataExtType").InnerText;
                    //Get value of DataExtValue
                    string DataExtValue = DataExtRet.SelectSingleNode("./DataExtValue").InnerText;
                }
            }

        }

    }
}
////The following sample code is generated as an illustration of
////Creating requests and parsing responses ONLY
////This code is NOT intended to show best practices or ideal code
////Use at your most careful discretion

//using System;
//using System.Net;
//using System.Drawing;
//using System.Collections;
//using System.ComponentModel;
//using System.Windows.Forms;
//using System.Data;
//using System.IO;
//using System.Xml;
//using Interop.QBXMLRP2;

//namespace com.intuit.idn.samples
//{
//    public class Sample
//    {
//        private XmlElement MakeSimpleElem(XmlDocument doc, string tagName, string tagVal)
//        {
//            XmlElement elem = doc.CreateElement(tagName);
//            elem.InnerText = tagVal;
//            return elem;
//        }

//        public void DoCustomerAdd()
//        {
//            bool sessionBegun = false;
//            bool connectionOpen = false;
//            RequestProcessor2 rp = null;

//            try
//            {
//                //Create the Request Processor object
//                rp = new RequestProcessor2();

//                //Create the XML document to hold our request
//                XmlDocument requestXmlDoc = new XmlDocument();
//                //Add the prolog processing instructions
//                requestXmlDoc.AppendChild(requestXmlDoc.CreateXmlDeclaration("1.0", null, null));
//                requestXmlDoc.AppendChild(requestXmlDoc.CreateProcessingInstruction("qbxml", "version=\"3.0\""));

//                //Create the outer request envelope tag
//                XmlElement outer = requestXmlDoc.CreateElement("QBXML");
//                requestXmlDoc.AppendChild(outer);

//                //Create the inner request envelope & any needed attributes
//                XmlElement inner = requestXmlDoc.CreateElement("QBXMLMsgsRq");
//                outer.AppendChild(inner);
//                inner.SetAttribute("onError", "stopOnError");
//                BuildCustomerAddRq(requestXmlDoc, inner);

//                //Connect to QuickBooks and begin a session
//                rp.OpenConnection2("", "Sample Code from OSR", localQBD);
//                connectionOpen = true;
//                string ticket = rp.BeginSession("", QBFileMode.qbFileOpenDoNotCare);
//                sessionBegun = true;

//                //Send the request and get the response from QuickBooks
//                string responseStr = rp.ProcessRequest(ticket, requestXmlDoc.OuterXml);

//                //End the session and close the connection to QuickBooks
//                rp.EndSession(ticket);
//                sessionBegun = false;
//                rp.CloseConnection();
//                connectionOpen = false;

//                WalkCustomerAddRs(responseStr);

//            }
//            catch (Exception e)
//            {
//                MessageBox.Show(e.Message, "Error");
//                if (sessionBegun)
//                {
//                    sessionManager.EndSession();
//                }
//                if (connectionOpen)
//                {
//                    sessionManager.CloseConnection();
//                }
//            }
//        }


//        void BuildCustomerAddRq(XmlDocument doc, XmlElement parent)
//        {

//            //Create CustomerAddRq aggregate and fill in field values for it
//            XmlElement CustomerAddRq = doc.CreateElement("CustomerAddRq");
//            parent.AppendChild(CustomerAddRq);

//            //Create CustomerAdd aggregate and fill in field values for it
//            XmlElement CustomerAdd = doc.CreateElement("CustomerAdd");
//            CustomerAddRq.AppendChild(CustomerAdd);
//            //Set field value for CompanyName <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "CompanyName", "ab"));
//            //Set field value for CustomerID <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "CustomerID", "ab"));
//            //Set field value for CustomerDiscPercent <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "CustomerDiscPercent", ""FLOATTYPE""));
//            //Set field value for CustomerDiscType <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "CustomerDiscType", "None"));
//            //Set field value for CustomerType <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "CustomerType", "ab"));
//            //Set field value for EMail <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "EMail", "ab"));
//            //Set field value for IsOkToEMail <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "IsOkToEMail", "1"));
//            //Set field value for FirstName <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "FirstName", "ab"));
//            //Set field value for IsAcceptingChecks <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "IsAcceptingChecks", "1"));
//            //Set field value for IsUsingChargeAccount <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "IsUsingChargeAccount", "1"));
//            //Set field value for IsUsingWithQB <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "IsUsingWithQB", "1"));
//            //Set field value for IsRewardsMember <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "IsRewardsMember", "1"));
//            //Set field value for IsNoShipToBilling <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "IsNoShipToBilling", "1"));
//            //Set field value for LastName <!-- required -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "LastName", "ab"));
//            //Set field value for Notes <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "Notes", "ab"));
//            //Set field value for Phone <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "Phone", "ab"));
//            //Set field value for Phone2 <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "Phone2", "ab"));
//            //Set field value for Phone3 <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "Phone3", "ab"));
//            //Set field value for Phone4 <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "Phone4", "ab"));
//            //Set field value for PriceLevelNumber <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "PriceLevelNumber", "1"));
//            //Set field value for Salutation <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "Salutation", "ab"));
//            //Set field value for TaxCategory <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "TaxCategory", "ab"));

//            //Create BillAddress aggregate and fill in field values for it
//            XmlElement BillAddress = doc.CreateElement("BillAddress");
//            CustomerAdd.AppendChild(BillAddress);
//            //Set field value for City <!-- optional -->
//            BillAddress.AppendChild(MakeSimpleElem(doc, "City", "ab"));
//            //Set field value for Country <!-- optional -->
//            BillAddress.AppendChild(MakeSimpleElem(doc, "Country", "ab"));
//            //Set field value for PostalCode <!-- optional -->
//            BillAddress.AppendChild(MakeSimpleElem(doc, "PostalCode", "ab"));
//            //Set field value for State <!-- optional -->
//            BillAddress.AppendChild(MakeSimpleElem(doc, "State", "ab"));
//            //Set field value for Street <!-- optional -->
//            BillAddress.AppendChild(MakeSimpleElem(doc, "Street", "ab"));
//            //Set field value for Street2 <!-- optional -->
//            BillAddress.AppendChild(MakeSimpleElem(doc, "Street2", "ab"));
//            //Done creating BillAddress aggregate

//            //Set field value for DefaultShipAddress <!-- optional -->
//            CustomerAdd.AppendChild(MakeSimpleElem(doc, "DefaultShipAddress", "ab"));

//            //Create ShipAddress aggregate and fill in field values for it
//            // May create more than one of these aggregates if needed
//            XmlElement ShipAddress = doc.CreateElement("ShipAddress");
//            CustomerAdd.AppendChild(ShipAddress);
//            //Set field value for AddressName <!-- optional -->
//            ShipAddress.AppendChild(MakeSimpleElem(doc, "AddressName", "ab"));
//            //Set field value for CompanyName <!-- optional -->
//            ShipAddress.AppendChild(MakeSimpleElem(doc, "CompanyName", "ab"));
//            //Set field value for FullName <!-- optional -->
//            ShipAddress.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//            //Set field value for City <!-- optional -->
//            ShipAddress.AppendChild(MakeSimpleElem(doc, "City", "ab"));
//            //Set field value for Country <!-- optional -->
//            ShipAddress.AppendChild(MakeSimpleElem(doc, "Country", "ab"));
//            //Set field value for PostalCode <!-- optional -->
//            ShipAddress.AppendChild(MakeSimpleElem(doc, "PostalCode", "ab"));
//            //Set field value for State <!-- optional -->
//            ShipAddress.AppendChild(MakeSimpleElem(doc, "State", "ab"));
//            //Set field value for Street <!-- optional -->
//            ShipAddress.AppendChild(MakeSimpleElem(doc, "Street", "ab"));
//            //Set field value for Street2 <!-- optional -->
//            ShipAddress.AppendChild(MakeSimpleElem(doc, "Street2", "ab"));
//            //Done creating ShipAddress aggregate

//            //Done creating CustomerAdd aggregate

//            //Set field value for IncludeRetElement <!-- optional, may repeat -->
//            CustomerAddRq.AppendChild(MakeSimpleElem(doc, "IncludeRetElement", "ab"));
//            //Done creating CustomerAddRq aggregate

//        }




//        void WalkCustomerAddRs(string response)
//        {
//            //Parse the response XML string into an XmlDocument
//            XmlDocument responseXmlDoc = new XmlDocument();
//            responseXmlDoc.LoadXml(response);

//            //Get the response for our request
//            XmlNodeList CustomerAddRsList = responseXmlDoc.GetElementsByTagName("CustomerAddRs");
//            if (CustomerAddRsList.Count == 1) //Should always be true since we only did one request in this sample
//            {
//                XmlNode responseNode = CustomerAddRsList.Item(0);
//                //Check the status code, info, and severity
//                XmlAttributeCollection rsAttributes = responseNode.Attributes;
//                string statusCode = rsAttributes.GetNamedItem("statusCode").Value;
//                string statusSeverity = rsAttributes.GetNamedItem("statusSeverity").Value;
//                string statusMessage = rsAttributes.GetNamedItem("statusMessage").Value;

//                //status code = 0 all OK, > 0 is warning
//                if (Convert.ToInt32(statusCode) >= 0)
//                {
//                    XmlNodeList CustomerRetList = responseNode.SelectNodes("//CustomerRet");//XPath Query
//                    for (int i = 0; i < CustomerRetList.Count; i++)
//                    {
//                        XmlNode CustomerRet = CustomerRetList.Item(i);
//                        WalkCustomerRet(CustomerRet);
//                    }
//                }
//            }
//        }



//        void WalkCustomerRet(XmlNode CustomerRet)
//        {
//            if (CustomerRet == null) return;

//            //Go through all the elements of CustomerRet
//            //Get value of ListID
//            if (CustomerRet.SelectSingleNode("./ListID") != null)
//            {
//                string ListID = CustomerRet.SelectSingleNode("./ListID").InnerText;
//            }
//            //Get value of TimeCreated
//            if (CustomerRet.SelectSingleNode("./TimeCreated") != null)
//            {
//                string TimeCreated = CustomerRet.SelectSingleNode("./TimeCreated").InnerText;
//            }
//            //Get value of TimeModified
//            if (CustomerRet.SelectSingleNode("./TimeModified") != null)
//            {
//                string TimeModified = CustomerRet.SelectSingleNode("./TimeModified").InnerText;
//            }
//            //Get value of AccountBalance
//            if (CustomerRet.SelectSingleNode("./AccountBalance") != null)
//            {
//                string AccountBalance = CustomerRet.SelectSingleNode("./AccountBalance").InnerText;
//            }
//            //Get value of AccountLimit
//            if (CustomerRet.SelectSingleNode("./AccountLimit") != null)
//            {
//                string AccountLimit = CustomerRet.SelectSingleNode("./AccountLimit").InnerText;
//            }
//            //Get value of AmountPastDue
//            if (CustomerRet.SelectSingleNode("./AmountPastDue") != null)
//            {
//                string AmountPastDue = CustomerRet.SelectSingleNode("./AmountPastDue").InnerText;
//            }
//            //Get value of CompanyName
//            if (CustomerRet.SelectSingleNode("./CompanyName") != null)
//            {
//                string CompanyName = CustomerRet.SelectSingleNode("./CompanyName").InnerText;
//            }
//            //Get value of CustomerID
//            if (CustomerRet.SelectSingleNode("./CustomerID") != null)
//            {
//                string CustomerID = CustomerRet.SelectSingleNode("./CustomerID").InnerText;
//            }
//            //Get value of CustomerDiscPercent
//            if (CustomerRet.SelectSingleNode("./CustomerDiscPercent") != null)
//            {
//                string CustomerDiscPercent = CustomerRet.SelectSingleNode("./CustomerDiscPercent").InnerText;
//            }
//            //Get value of CustomerDiscType
//            if (CustomerRet.SelectSingleNode("./CustomerDiscType") != null)
//            {
//                string CustomerDiscType = CustomerRet.SelectSingleNode("./CustomerDiscType").InnerText;
//            }
//            //Get value of CustomerType
//            if (CustomerRet.SelectSingleNode("./CustomerType") != null)
//            {
//                string CustomerType = CustomerRet.SelectSingleNode("./CustomerType").InnerText;
//            }
//            //Get value of EMail
//            if (CustomerRet.SelectSingleNode("./EMail") != null)
//            {
//                string EMail = CustomerRet.SelectSingleNode("./EMail").InnerText;
//            }
//            //Get value of IsOkToEMail
//            if (CustomerRet.SelectSingleNode("./IsOkToEMail") != null)
//            {
//                string IsOkToEMail = CustomerRet.SelectSingleNode("./IsOkToEMail").InnerText;
//            }
//            //Get value of FirstName
//            if (CustomerRet.SelectSingleNode("./FirstName") != null)
//            {
//                string FirstName = CustomerRet.SelectSingleNode("./FirstName").InnerText;
//            }
//            //Get value of FullName
//            if (CustomerRet.SelectSingleNode("./FullName") != null)
//            {
//                string FullName = CustomerRet.SelectSingleNode("./FullName").InnerText;
//            }
//            //Get value of IsAcceptingChecks
//            if (CustomerRet.SelectSingleNode("./IsAcceptingChecks") != null)
//            {
//                string IsAcceptingChecks = CustomerRet.SelectSingleNode("./IsAcceptingChecks").InnerText;
//            }
//            //Get value of IsUsingChargeAccount
//            if (CustomerRet.SelectSingleNode("./IsUsingChargeAccount") != null)
//            {
//                string IsUsingChargeAccount = CustomerRet.SelectSingleNode("./IsUsingChargeAccount").InnerText;
//            }
//            //Get value of IsUsingWithQB
//            if (CustomerRet.SelectSingleNode("./IsUsingWithQB") != null)
//            {
//                string IsUsingWithQB = CustomerRet.SelectSingleNode("./IsUsingWithQB").InnerText;
//            }
//            //Get value of IsRewardsMember
//            if (CustomerRet.SelectSingleNode("./IsRewardsMember") != null)
//            {
//                string IsRewardsMember = CustomerRet.SelectSingleNode("./IsRewardsMember").InnerText;
//            }
//            //Get value of IsNoShipToBilling
//            if (CustomerRet.SelectSingleNode("./IsNoShipToBilling") != null)
//            {
//                string IsNoShipToBilling = CustomerRet.SelectSingleNode("./IsNoShipToBilling").InnerText;
//            }
//            //Get value of LastName
//            if (CustomerRet.SelectSingleNode("./LastName") != null)
//            {
//                string LastName = CustomerRet.SelectSingleNode("./LastName").InnerText;
//            }
//            //Get value of LastSale
//            if (CustomerRet.SelectSingleNode("./LastSale") != null)
//            {
//                string LastSale = CustomerRet.SelectSingleNode("./LastSale").InnerText;
//            }
//            //Get value of Notes
//            if (CustomerRet.SelectSingleNode("./Notes") != null)
//            {
//                string Notes = CustomerRet.SelectSingleNode("./Notes").InnerText;
//            }
//            //Get value of Phone
//            if (CustomerRet.SelectSingleNode("./Phone") != null)
//            {
//                string Phone = CustomerRet.SelectSingleNode("./Phone").InnerText;
//            }
//            //Get value of Phone2
//            if (CustomerRet.SelectSingleNode("./Phone2") != null)
//            {
//                string Phone2 = CustomerRet.SelectSingleNode("./Phone2").InnerText;
//            }
//            //Get value of Phone3
//            if (CustomerRet.SelectSingleNode("./Phone3") != null)
//            {
//                string Phone3 = CustomerRet.SelectSingleNode("./Phone3").InnerText;
//            }
//            //Get value of Phone4
//            if (CustomerRet.SelectSingleNode("./Phone4") != null)
//            {
//                string Phone4 = CustomerRet.SelectSingleNode("./Phone4").InnerText;
//            }
//            //Get value of PriceLevelNumber
//            if (CustomerRet.SelectSingleNode("./PriceLevelNumber") != null)
//            {
//                string PriceLevelNumber = CustomerRet.SelectSingleNode("./PriceLevelNumber").InnerText;
//            }
//            //Get value of Salutation
//            if (CustomerRet.SelectSingleNode("./Salutation") != null)
//            {
//                string Salutation = CustomerRet.SelectSingleNode("./Salutation").InnerText;
//            }
//            //Get value of StoreExchangeStatus
//            if (CustomerRet.SelectSingleNode("./StoreExchangeStatus") != null)
//            {
//                string StoreExchangeStatus = CustomerRet.SelectSingleNode("./StoreExchangeStatus").InnerText;
//            }
//            //Get value of TaxCategory
//            if (CustomerRet.SelectSingleNode("./TaxCategory") != null)
//            {
//                string TaxCategory = CustomerRet.SelectSingleNode("./TaxCategory").InnerText;
//            }
//            //Get value of WebNumber
//            if (CustomerRet.SelectSingleNode("./WebNumber") != null)
//            {
//                string WebNumber = CustomerRet.SelectSingleNode("./WebNumber").InnerText;
//            }
//            //Get all field values for BillAddress aggregate
//            XmlNode BillAddress = CustomerRet.SelectSingleNode("./BillAddress");
//            if (BillAddress != null)
//            {
//                //Get value of City
//                if (CustomerRet.SelectSingleNode("./BillAddress/City") != null)
//                {
//                    string City = CustomerRet.SelectSingleNode("./BillAddress/City").InnerText;
//                }
//                //Get value of Country
//                if (CustomerRet.SelectSingleNode("./BillAddress/Country") != null)
//                {
//                    string Country = CustomerRet.SelectSingleNode("./BillAddress/Country").InnerText;
//                }
//                //Get value of PostalCode
//                if (CustomerRet.SelectSingleNode("./BillAddress/PostalCode") != null)
//                {
//                    string PostalCode = CustomerRet.SelectSingleNode("./BillAddress/PostalCode").InnerText;
//                }
//                //Get value of State
//                if (CustomerRet.SelectSingleNode("./BillAddress/State") != null)
//                {
//                    string State = CustomerRet.SelectSingleNode("./BillAddress/State").InnerText;
//                }
//                //Get value of Street
//                if (CustomerRet.SelectSingleNode("./BillAddress/Street") != null)
//                {
//                    string Street = CustomerRet.SelectSingleNode("./BillAddress/Street").InnerText;
//                }
//                //Get value of Street2
//                if (CustomerRet.SelectSingleNode("./BillAddress/Street2") != null)
//                {
//                    string Street2 = CustomerRet.SelectSingleNode("./BillAddress/Street2").InnerText;
//                }
//            }
//            //Done with field values for BillAddress aggregate

//            //Get value of DefaultShipAddress
//            if (CustomerRet.SelectSingleNode("./DefaultShipAddress") != null)
//            {
//                string DefaultShipAddress = CustomerRet.SelectSingleNode("./DefaultShipAddress").InnerText;
//            }
//            //Walk list of ShipAddress aggregates
//            XmlNodeList ShipAddressList = CustomerRet.SelectNodes("./ShipAddress");
//            if (ShipAddressList != null)
//            {
//                for (int i = 0; i < ShipAddressList.Count; i++)
//                {
//                    XmlNode ShipAddress = ShipAddressList.Item(i);
//                    //Get value of AddressName
//                    if (ShipAddress.SelectSingleNode("./AddressName") != null)
//                    {
//                        string AddressName = ShipAddress.SelectSingleNode("./AddressName").InnerText;
//                    }
//                    //Get value of CompanyName
//                    if (ShipAddress.SelectSingleNode("./CompanyName") != null)
//                    {
//                        string CompanyName = ShipAddress.SelectSingleNode("./CompanyName").InnerText;
//                    }
//                    //Get value of FullName
//                    if (ShipAddress.SelectSingleNode("./FullName") != null)
//                    {
//                        string FullName = ShipAddress.SelectSingleNode("./FullName").InnerText;
//                    }
//                    //Get value of City
//                    if (ShipAddress.SelectSingleNode("./City") != null)
//                    {
//                        string City = ShipAddress.SelectSingleNode("./City").InnerText;
//                    }
//                    //Get value of Country
//                    if (ShipAddress.SelectSingleNode("./Country") != null)
//                    {
//                        string Country = ShipAddress.SelectSingleNode("./Country").InnerText;
//                    }
//                    //Get value of PostalCode
//                    if (ShipAddress.SelectSingleNode("./PostalCode") != null)
//                    {
//                        string PostalCode = ShipAddress.SelectSingleNode("./PostalCode").InnerText;
//                    }
//                    //Get value of State
//                    if (ShipAddress.SelectSingleNode("./State") != null)
//                    {
//                        string State = ShipAddress.SelectSingleNode("./State").InnerText;
//                    }
//                    //Get value of Street
//                    if (ShipAddress.SelectSingleNode("./Street") != null)
//                    {
//                        string Street = ShipAddress.SelectSingleNode("./Street").InnerText;
//                    }
//                    //Get value of Street2
//                    if (ShipAddress.SelectSingleNode("./Street2") != null)
//                    {
//                        string Street2 = ShipAddress.SelectSingleNode("./Street2").InnerText;
//                    }
//                }
//            }

//            //Walk list of Reward aggregates
//            XmlNodeList RewardList = CustomerRet.SelectNodes("./Reward");
//            if (RewardList != null)
//            {
//                for (int i = 0; i < RewardList.Count; i++)
//                {
//                    XmlNode Reward = RewardList.Item(i);
//                    //Get value of RewardAmount
//                    if (Reward.SelectSingleNode("./RewardAmount") != null)
//                    {
//                        string RewardAmount = Reward.SelectSingleNode("./RewardAmount").InnerText;
//                    }
//                    //Get value of RewardPercent
//                    if (Reward.SelectSingleNode("./RewardPercent") != null)
//                    {
//                        string RewardPercent = Reward.SelectSingleNode("./RewardPercent").InnerText;
//                    }
//                    //Get value of EarnedDate
//                    if (Reward.SelectSingleNode("./EarnedDate") != null)
//                    {
//                        string EarnedDate = Reward.SelectSingleNode("./EarnedDate").InnerText;
//                    }
//                    //Get value of MatureDate
//                    if (Reward.SelectSingleNode("./MatureDate") != null)
//                    {
//                        string MatureDate = Reward.SelectSingleNode("./MatureDate").InnerText;
//                    }
//                    //Get value of ExpirationDate
//                    if (Reward.SelectSingleNode("./ExpirationDate") != null)
//                    {
//                        string ExpirationDate = Reward.SelectSingleNode("./ExpirationDate").InnerText;
//                    }
//                }
//            }

//            //Walk list of DataExtRet aggregates
//            XmlNodeList DataExtRetList = CustomerRet.SelectNodes("./DataExtRet");
//            if (DataExtRetList != null)
//            {
//                for (int i = 0; i < DataExtRetList.Count; i++)
//                {
//                    XmlNode DataExtRet = DataExtRetList.Item(i);
//                    //Get value of OwnerID
//                    string OwnerID = DataExtRet.SelectSingleNode("./OwnerID").InnerText;
//                    //Get value of DataExtName
//                    string DataExtName = DataExtRet.SelectSingleNode("./DataExtName").InnerText;
//                    //Get value of DataExtType
//                    string DataExtType = DataExtRet.SelectSingleNode("./DataExtType").InnerText;
//                    //Get value of DataExtValue
//                    string DataExtValue = DataExtRet.SelectSingleNode("./DataExtValue").InnerText;
//                }
//            }

//        }



//    }
//}