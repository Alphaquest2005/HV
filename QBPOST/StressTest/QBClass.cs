using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using log4netWrapper;
using QS2QBPost.Annotations;
using QS2QBPost.Properties;
using QuickBooks;
using RMSDataAccessLayer;
using Timer = System.Timers.Timer;

namespace QS2QBPost
{
    public class QBClass : INotifyPropertyChanged
    {
       
        private static volatile QBClass singleton;
        private static object syncRoot = new Object();
        private static Timer postingTimer;
        private static int _totalPost;
        private static int _post;
        private static string _status = "Service Started";

        static QBClass()
        {
            postingTimer = new System.Timers.Timer(Properties.Settings.Default.DownloadIntervalInSeconds * 1000);
            postingTimer.Elapsed += Singleton.OnTimeToPost;
            postingTimer.Enabled = true;
        }

    
        public static QBClass Singleton
        {
            get
            {
                if (singleton == null)
                {
                    lock (syncRoot)
                    {
                        if (singleton == null)
                            singleton = new QBClass();
                    }
                }

                return singleton;
            }
        }

        public static bool IsPosting => postingTimer.Enabled;
      
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
               OnPropertyChanged();
            }
        }

        public int TotalPost
        {
            get { return _totalPost; }
            private set
            {
                _totalPost = value;
                OnPropertyChanged();
            }
        }

        public int ProcessedPost
        {
            get { return _post; }
            set
            {
                _post = value;
                OnPropertyChanged();
            }
        }

        private void OnTimeToPost(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (postingTimer.Enabled == true)
                {
                    PostToQB();
                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public static void PostToQB()
        {
            try
            {
                //throw new Exception("test exception");
                postingTimer.Enabled = false;
                Singleton.Status = "Started Posting";
                var lst = new List<PostTransaction>();
                lst = GetUnpostedTransactions(lst);
                //while (keeprunning)
                Singleton.ProcessedPost = 0;
                Singleton.TotalPost = lst.Count - 1;
                foreach (var itm in lst)
                {
                    //Thread.Sleep(5000);
                    try
                    {
                        Post(itm);
                        Singleton.ProcessedPost += 1;
                    }
                    catch (Exception iex)
                    {
                        Logger.Log(LoggingLevel.Error, iex.Message + iex.StackTrace);
                        Singleton.Status = "Error: " + iex.Message;
                        UpdateTransactionDataStatus(itm, iex.Message);

                    }

                }
                Singleton.Status = "Stopped Posting waiting on timer";
                postingTimer.Enabled = true;
                
            }
            catch (Exception ex)
            {

                // do nothing
            }
           
          
        }

        private static void UpdateTransactionDataStatus(PostTransaction itm,string message)
        {
            using (var ctx = new RMSModel())
            {
                var trns = ctx.TransactionBase.FirstOrDefault(x => x.TransactionId == itm.TransactionData.TransactionId);
                trns.Status = message;
                ctx.TransactionBase.AddOrUpdate(trns);
                ctx.SaveChanges();
            }
        }


        private static List<PostTransaction> GetUnpostedTransactions(List<PostTransaction> lst)
        {
            try
            {
                using (var ctx = new RMSModel())
                {

                    //ctx.TransactionBase.Where(x =>
                    //      x.Status == "ToBePosted" &&
                    //      x.TransactionEntries.Any(z => z.TransactionEntryItem.Item == null))
                    //  .ForEachAsync(x => x.Status = "TransactionEntryItem.Item is null");
                    //ctx.SaveChanges();

                    ctx.Database.ExecuteSqlCommand(@"UPDATE       TransactionBase
                                                        SET                Status = 'TransactionEntryItem.Item is null'
                                                        FROM            TransactionBase INNER JOIN
                                                                                 TransactionEntryBase ON TransactionBase.TransactionId = TransactionEntryBase.TransactionId INNER JOIN
                                                                                 TransactionEntryItem ON TransactionEntryBase.TransactionEntryId = TransactionEntryItem.TransactionEntryId LEFT OUTER JOIN
                                                                                 Item ON TransactionEntryItem.ItemId = Item.ItemId
                                                        WHERE        (Item.ItemId IS NULL) AND (TransactionBase.Status = 'ToBePosted')");

                    //ctx.TransactionBase.Where(x =>
                    //      x.Status == "ToBePosted" &&
                    //      x.TransactionEntries.Any(z =>  z.TransactionEntryItem.Item.QBActive != true))
                    //  .ForEachAsync(x => x.Status = "Contains Items not in QuickBooks-QBACTIVE");
                    //ctx.SaveChanges();

                    ctx.Database.ExecuteSqlCommand(@"UPDATE       TransactionBase
                                                        SET                Status = 'Contains Items not in QuickBooks-QBACTIVE'
                                                        FROM            TransactionBase INNER JOIN
                                                                                 TransactionEntryBase ON TransactionBase.TransactionId = TransactionEntryBase.TransactionId INNER JOIN
                                                                                 TransactionEntryItem ON TransactionEntryBase.TransactionEntryId = TransactionEntryItem.TransactionEntryId LEFT OUTER JOIN
                                                                                 Item ON TransactionEntryItem.ItemId = Item.ItemId
                                                        WHERE        (Item.QBActive = 0 ) AND (TransactionBase.Status = 'ToBePosted')");


                    //ctx.TransactionBase.Where(x =>
                    //      x.Status == "ToBePosted" &&
                    //      x.TransactionEntries.Any(z => z.TransactionEntryItem.Item.QBItemListID == null))
                    //  .ForEachAsync(x => x.Status = "Contains Items Null ListID");
                    //ctx.SaveChanges();

                    ctx.Database.ExecuteSqlCommand(@"UPDATE       TransactionBase
                                                        SET                Status = 'Contains Items Null ListID'
                                                        FROM            TransactionBase INNER JOIN
                                                                                 TransactionEntryBase ON TransactionBase.TransactionId = TransactionEntryBase.TransactionId INNER JOIN
                                                                                 TransactionEntryItem ON TransactionEntryBase.TransactionEntryId = TransactionEntryItem.TransactionEntryId LEFT OUTER JOIN
                                                                                 Item ON TransactionEntryItem.ItemId = Item.ItemId
                                                        WHERE        ((Item.QBItemListID is null) or (LTRIM(RTRIM(item.QBItemListID)) ='') ) AND (TransactionBase.Status = 'ToBePosted')");

                    int i = 0;


                    lst = ctx.TransactionBase.Where(x => x.Status == "ToBePosted" && x.TransactionEntries.Any() && x.TransactionEntries.All(z => z.TransactionEntryItem.Item.QBActive == true))
                        .Select(x => new PostTransaction()
                        {
                            TransactionData = x,
                            PostEntries = x.TransactionEntries.Select(z => new PostEntry()
                            {
                                QBListId = z.TransactionEntryItem.QBItemListID,
                                ItemNumber = z.TransactionEntryItem.ItemNumber,
                                Quantity = (int)z.Quantity
                            }).ToList()
                        }).ToList();
             



                }
                return lst;
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, ex.Message + ex.StackTrace);
                Singleton.Status = "Error: " + ex.Message;
                return new List<PostTransaction>();
            }
        }

        private static void Post(PostTransaction pt)
        {
            try
            {
                
                IncludePrecriptionProperties(pt.TransactionData);

                SalesReceipt s = new SalesReceipt();
                s.TxnDate = pt.TransactionData.Time;
                s.TxnState = "1";
                s.Workstation = "02";
                s.StoreNumber = "1";
                s.SalesReceiptNumber = "123";
                s.Discount = "0";

                if (pt.TransactionData == null || string.IsNullOrEmpty(pt.TransactionData.TransactionNumber))
                {

                    //MessageBox.Show("Invalid Transaction Please Try again");
                    //TransactionData.Status = "Invalid Transaction Please Try again";
                    //rms.SaveChanges();
                    //return;
                }

                //TransPreZeroConverter tz = new TransPreZeroConverter();

                if (pt.TransactionData is Prescription)
                {
                    Prescription p = pt.TransactionData as Prescription;
                    string doctor = "";
                    string patient = "";
                    if (p.Doctor != null)
                    {
                        doctor = p.Doctor.DisplayName;
                    }
                    if (p.Patient != null)
                    {
                        patient = p.Patient.ContactInfo;
                        s.Discount = p.Patient.Discount == null ? "" : p.Patient.Discount.ToString();
                    }
                    s.Comments = String.Format("{0} \n RX#:{1} \n Doctor:{2}", patient,
                        p.TransactionNumber, doctor);
                }
                else
                {
                    s.Comments = "RX#:" + pt.TransactionData.TransactionNumber;
                }


                if (pt.TransactionData != null)
                {
                    s.TrackingNumber = pt.TransactionData.TransactionNumber;
                }
                s.Associate = "Dispensary";
                s.SalesReceiptType = "0";



                foreach (var item in pt.PostEntries)
                {

                    var qbitm = QBPOS.ValidateInventoryItemQuery(item.QBListId, Settings.Default.QBCompanyFile);
                    if (!qbitm.Any())
                    {
                        SetQBInactive(item.QBListId);
                        pt.TransactionData.Status = "Can't Post Because Item Not In QuickBooks";
                        UpdateTransactionDataFromSalesRet(pt, new QBResult() { Comments = "Item Not In QuickBooks" });
                        return;
                    }
                    else
                    {
                         UpdateInventoryItem(qbitm);
                    }

                    s.SalesReceiptDetails.Add(new SalesReceiptDetail
                        {
                            ItemListID = item.QBListId,
                            ItemNumber = item.ItemNumber,
                            QtySold = item.Quantity
                        }); //340 
                    
                }


                
                  var result = QBPOS.AddSalesReceipt(s, Settings.Default.QBCompanyFile);
                if (result != null)
                {
                    UpdateTransactionDataFromSalesRet(pt, result);
                }
            }
            catch (Exception ex)
            {
                var msg = CleanErrorMessage(ex.Message);
                Logger.Log(LoggingLevel.Error, "error found for #" + pt.TransactionData.TransactionNumber + ":---:" + msg + ex.StackTrace);
                Singleton.Status = "error found for #" + pt.TransactionData.TransactionNumber;
                UpdateTransactionDataStatus(pt, msg);
                if (msg.Contains("Can't connect to the database"))
                {
                    MessageBox.Show(msg);
                    postingTimer.Enabled = false;
                    throw ex;
                }
                 
            }
        }

        private static string CleanErrorMessage(string message)
        {
            if (message.EndsWith("not found") && message.Contains("Item"))
            {
                var startIndex = message.LastIndexOf("Item", StringComparison.Ordinal)+4;
                var endIndex = message.LastIndexOf("not found", StringComparison.Ordinal);
                var listId = message.Substring(startIndex, endIndex - startIndex).Trim();
                using (var ctx = new RMSModel())
                {
                    var trns = ctx.QBInventoryItems.FirstOrDefault(x => x.ListID == listId);
                    if (trns != null) return $"{trns.ItemName}-ItemNumber:{trns.ItemNumber} Not in QB Inventory. Please use Subsitute!"; 
                 }
            }
            return message;
        }

        private static void SetQBInactive(string listID)
        {
            try
            {
                using (var ctx = new RMSModel())
                {

                    var r = ctx.Item.FirstOrDefault(x => x.QBItemListID == listID);
                    if (r == null) return;

                    r.QBActive = false;

                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private static void UpdateInventoryItem(List<ItemInventoryRet> qbitms)
        {
            try
            {
                using (var ctx = new RMSModel())
                {
                    foreach (var itm in qbitms)
                    {
                        var r = ctx.Item.FirstOrDefault(x => x.QBItemListID == itm.ListID);
                        if (r == null) continue;
                        r.Price = (double) itm.Price1;
                        r.Quantity = (double) itm.QuantityOnHand;
                        r.QBActive = false;

                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private static void UpdateTransactionDataFromSalesRet(PostTransaction pt, QBResult result)
        {
            try
            {
                using (var ctx = new RMSModel())
                {
                    if (string.IsNullOrEmpty(result.SalesReceiptNumber))
                    {

                        pt.TransactionData.Status = string.IsNullOrEmpty(result.Comments)
                            ? "QB Posting Error"
                            : result.Comments.Substring(0, result.Comments.Length > 49 ? 49 : result.Comments.Length);
                    }
                    else
                    {
                        pt.TransactionData.ReferenceNumber = "QB#" + result.SalesReceiptNumber;
                        pt.TransactionData.Status = "Posted";
                    }
                    ctx.TransactionBase.AddOrUpdate(pt.TransactionData);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, ex.Message + ex.StackTrace);
                Singleton.Status = "error found for #" + pt.TransactionData.TransactionNumber;
                throw ex;
            }
        }

        private class PostTransaction
        {
            public TransactionBase TransactionData { get; set; }
            public List<PostEntry> PostEntries { get; set; }

           
        }

        private class PostEntry
        {
            public string QBListId { get; set; }
            public int Quantity { get; set; }
            public string ItemNumber { get; set; }
        }

        private static void IncludePrecriptionProperties(TransactionBase ptrn)
        {
            try
            {
                if (ptrn is Prescription)
                {

                    var pc = (ptrn as Prescription);
                    using (var ctx = new RMSModel())
                    {
                        pc.Doctor = ctx.Persons.OfType<Doctor>().FirstOrDefault(x => x.Id == pc.DoctorId);
                        pc.Patient = ctx.Persons.OfType<Patient>().FirstOrDefault(x => x.Id == pc.PatientId);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, ex.Message + ex.StackTrace);
                throw ex;
            }
        }

     


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}