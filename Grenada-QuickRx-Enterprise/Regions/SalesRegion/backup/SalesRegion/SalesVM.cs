﻿
using RMSDataAccessLayer;

using System.Windows;
using System;
using System.Collections.Concurrent;
using System.Data.Entity.Core.Objects;
using System.Windows.Data;
using System.Runtime.CompilerServices;
using System.Windows.Ink;
using Common.Core.Logging;
using log4netWrapper;
using SimpleMvvmToolkit;
using TrackableEntities.Client;
using System.Linq;

using RMSDataAccessLayer;

using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Immutable;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Printing;
using System.Threading.Tasks;
using System.Transactions;
using SUT.PrintEngine.Utils;
using System.Windows.Media;
using Common.Core.Logging;
using log4netWrapper;
using QuickBooks;
using SalesRegion.Messages;
using SimpleMvvmToolkit;
using TrackableEntities;
using TrackableEntities.Common;
using TrackableEntities.EF6;
using System.Windows.Xps;
using System.IO;
using System.Windows.Xps.Packaging;
using System.IO.Packaging;
using System.Text.RegularExpressions;
using SUT.PrintEngine;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using Common.Core;

namespace SalesRegion
{
    public partial class SalesVM : ViewModelBase<SalesVM>
    {


        private static readonly SalesVM _instance;

        static SalesVM()
        {
            _instance = new SalesVM();
        }

        public static SalesVM Instance
        {
            get { return _instance; }
        }


        private static Cashier _currentLogin;

        public Cashier CurrentLogin
        {
            get { return _currentLogin; }
            set
            {
                if (_currentLogin != value)
                {
                    _currentLogin = value;
                    NotifyPropertyChanged(x => x.CurrentLogin);
                }
            }
        }

        public SalesVM()
        {

        }


        public void CloseTransaction()
        {
            try
            {
                Logger.Log(LoggingLevel.Info, "Close Transaction");
                if (batch == null)
                {
                    Logger.Log(LoggingLevel.Warning, "Batch is null");
                    MessageBox.Show("Batch is null");
                    return;
                }

                if (TransactionData != null && TransactionData.OpenClose == true)
                {
                    TransactionData.CloseBatchId = Batch.BatchId;
                    TransactionData.OpenClose = false;


                    SaveTransaction();

                }


                TransactionData = null;
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error,
                    GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        public void Transaction2Pdf(ref Grid fwe)
        {
            if (TransactionData == null) return;
            if (!Directory.Exists($@"{AppDomain.CurrentDomain.BaseDirectory}\Archieve"))
                Directory.CreateDirectory($@"{AppDomain.CurrentDomain.BaseDirectory}\Archieve");
            var fileName = $@"{AppDomain.CurrentDomain.BaseDirectory}\Archieve\{TransactionData.TransactionId}";
            //WPF2PDF.CreatePDF(ref fwe,fileName);




            Size visualSize;

            visualSize = new Size(11*92, 8*92); // paper size

            DrawingVisual visual =
                PrintControlFactory.CreateDrawingVisual(fwe, fwe.ActualWidth, fwe.ActualHeight);


            SUT.PrintEngine.Paginators.VisualPaginator page = new SUT.PrintEngine.Paginators.VisualPaginator(
                visual, visualSize, new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, 0));
            page.Initialize(false);

            var ms = new MemoryStream();
            var package = Package.Open(ms, FileMode.Create);
            var doc = new XpsDocument(package);
            var writer = XpsDocument.CreateXpsDocumentWriter(doc);
            writer.Write(page);
            doc.Close();
            package.Close();

            // Get XPS file bytes
            var bytes = ms.ToArray();
            ms.Dispose();

            // Print to PDF

            PdfFilePrinter.PrintXpsToPdf(bytes, fileName + ".pdf", "");
        }


        //public void CreateNewPrescription()
        //{
        //    try
        //    {

        //        Logger.Log(LoggingLevel.Info, "Create New Prescription");
        //        if (doctor == null)
        //        {
        //            Logger.Log(LoggingLevel.Warning, "Doctor is Missing");
        //            this.Status = "Doctor is Missing";
        //            return;
        //        }

        //        if (patient == null)
        //        {
        //            Logger.Log(LoggingLevel.Warning, "Patient is Missing");
        //            this.Status = "Patient is Missing";
        //            return;
        //        }

        //        if (Store == null)
        //        {
        //            Logger.Log(LoggingLevel.Warning, "Store is Missing");
        //            this.Status = "Store is Missing";
        //            return;
        //        }

        //        if (Batch == null)
        //        {
        //            Logger.Log(LoggingLevel.Warning, "Batch is Missing");
        //            this.Status = "Batch is Missing";
        //            return;
        //        }

        //        if (CurrentLogin == null)
        //        {
        //            Logger.Log(LoggingLevel.Warning, "Cashier is Missing");
        //            this.Status = "CurrentLogin is Missing";
        //            return;
        //        }

        //        if (Station == null)
        //        {
        //            Logger.Log(LoggingLevel.Warning, "Station is Missing");
        //            this.Status = "Station is Missing";
        //            return;
        //        }
        //        Prescription txn = new Prescription()
        //        {
        //            BatchId = Batch.BatchId,
        //            StationId = Station.StationId,
        //            Time = DateTime.Now,
        //            CashierId = CurrentLogin.Id,
        //            PharmacistId = (CurrentLogin.Role == "Pharmacist" ? CurrentLogin.Id : null as int?),
        //            StoreCode = Store.StoreCode,
        //            OpenClose = true,
        //            DoctorId = doctor.Id,
        //            PatientId = patient.Id,
        //            Patient = patient,
        //            Doctor = doctor,
        //            Cashier = CurrentLogin,
        //            Pharmacist = CurrentLogin.Role == "Pharmacist" ? CurrentLogin : null,
        //            TrackingState = TrackingState.Added
        //        };
        //        txn.StartTracking();
        //        Logger.Log(LoggingLevel.Info, "Prescription Created");
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
        //        throw ex;
        //    }
        //}


        //+ ToDo: Replace this with your own data fields

        private Doctor doctor = null;

        public Doctor Doctor
        {
            get { return doctor; }
            set
            {
                if (doctor != value)
                {
                    doctor = value;
                    NotifyPropertyChanged(x => x.Doctor);
                }
            }
        }

        private Patient patient = null;

        public Patient Patient
        {
            get
            {
                if (patient == null && TransactionData != null && ((dynamic) TransactionData).Patient != null)
                    Patient = ((dynamic) TransactionData).Patient;

                return patient;
            }
            set
            {
                if (patient != value)
                {
                    patient = value;

                    NotifyPropertyChanged(x => x.Patient);
                }
            }
        }

        public AgeRange PatientAgeRange
        {
            get
            {
                return Patient?.DOB == null
                    ? null //AgeRange.FirstOrDefault(x => x.MaxAge == 0)
                    : AgeRange.FirstOrDefault(x => x.MaxAge > (DateTime.Now.Year - Patient.DOB.Value.Year));
            }
            set
            {
                if (value != null && Patient != null)
                {
                    Patient.DOB = new DateTime(DateTime.Now.Year - value.MinAge, 1, 1);
                    using (var ctx = new RMSModel())
                    {
                        var p = ctx.Persons.First(x => x.Id == Patient.Id);
                        p.DOB = Patient.DOB;
                        ctx.SaveChanges();
                    }
                }
            }
        }

        private Cashier transactionCashier = null;

        public Cashier TransactionCashier
        {
            get { return transactionCashier; }
            set
            {
                if (transactionCashier != value)
                {
                    transactionCashier = value;
                    NotifyPropertyChanged(x => x.TransactionCashier);
                }
            }
        }

        private Cashier _transactionPharmacist = null;

        public Cashier TransactionPharmacist
        {
            get { return _transactionPharmacist; }
            set
            {
                if (_transactionPharmacist != value)
                {
                    _transactionPharmacist = value;
                    if (TransactionData != null)
                    {
                        if (value != null)
                        {
                            TransactionData.PharmacistId = value.Id;
                            TransactionData.Pharmacist = value;
                        }
                    }

                    NotifyPropertyChanged(x => x.TransactionPharmacist);
                }
            }
        }

        private string status = null;

        public string Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    NotifyPropertyChanged(x => x.Status);
                }
            }
        }


        public TransactionBase transactionData;

        public TransactionBase TransactionData
        {
            get { return transactionData; }
            set
            {
                if (!object.Equals(transactionData, value))
                {
                    Set_TransactionData(value);

                }
            }
        }

        private void Set_TransactionData(TransactionBase value)
        {
            transactionData = value;

            SendMessage(MessageToken.TransactionDataChanged,
                new NotificationEventArgs<TransactionBase>(MessageToken.TransactionDataChanged, transactionData));
            if (transactionData != null) transactionData.PropertyChanged += TransactionData_PropertyChanged;

            NotifyPropertyChanged(x => x.TransactionData);
        }

        private void TransactionData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentTransactionEntry")
            {
                if (transactionData != null)
                    if (transactionData.CurrentTransactionEntry != null)
                        if (transactionData.CurrentTransactionEntry.TransactionEntryItem != null)
                            SetCurrentItemDosage(transactionData.CurrentTransactionEntry.TransactionEntryItem);
            }
        }

        private ObservableCollection<object> _csv;

        public ObservableCollection<object> SearchList
        {
            get { return _csv; }

        }

        private ObservableCollection<AgeRange> _ageRange = null;

        public ObservableCollection<AgeRange> AgeRange
        {
            get
            {
                if (_ageRange == null)
                {
                    using (var ctx = new RMSModel())
                    {
                        _ageRange =
                            new ObservableCollection<AgeRange>(
                                ctx.AgeRanges1); //x.Role == "Pharmacist" &&

                    }
                }

                return _ageRange;
            }
        }

        private ObservableCollection<Cashier> _pharmacists = null;

        public ObservableCollection<Cashier> Pharmacists
        {
            get
            {
                if (_pharmacists == null)
                {
                    using (var ctx = new RMSModel())
                    {
                        _pharmacists =
                            new ObservableCollection<Cashier>(
                                ctx.Persons.OfType<Cashier>()
                                    .Where(x => x.IsActive == true)); //x.Role == "Pharmacist" &&
                        _pharmacists.ToList().ForEach(x => x.StartTracking());
                    }
                }

                return _pharmacists;
            }
        }

        private ObservableCollection<PresetDosage> _presetDosage = null;

        public ObservableCollection<PresetDosage> PresetDosage
        {
            get
            {
                if (_presetDosage == null)
                {
                    using (var ctx = new RMSModel())
                    {
                        _presetDosage =
                            new ObservableCollection<PresetDosage>(
                                ctx.PresetDosages);

                    }
                }

                return _presetDosage;
            }
        }


        //private Cashier _currentPharmacist = null;

        //public Cashier CurrentPharmacist
        //{
        //    get { return _currentPharmacist; }
        //    set
        //    {
        //        if (_currentPharmacist != value)
        //        {
        //            _currentPharmacist = value;
        //            NotifyPropertyChanged(x => CurrentPharmacist);
        //        }
        //    }
        //}


        public void UpdateSearchList(string filterText)
        {
            try
            {
                Logger.Log(LoggingLevel.Info,
                    string.Format("Update SearchList -filter Text [{0}] - StartTime:{1}", filterText, DateTime.Now));
                var lst = CreateSearchList(filterText);


                _csv = new ObservableCollection<Object>();
                foreach (var item in lst)
                {
                    _csv.Add(item);
                }

                NotifyPropertyChanged(x => x.SearchList);
                Logger.Log(LoggingLevel.Info,
                    string.Format("Finish Update SearchList - filter Text [{0}] - EndTime:{1}", filterText,
                        DateTime.Now));
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error,
                    GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        public void GetSearchResults(string filterText)
        {
            UpdateSearchList(filterText);
        }



        private List<dynamic> CreateSearchList(string filterText)
        {
            try
            {
                //todo: make parallel
                Logger.Log(LoggingLevel.Info,
                    string.Format("Start Create SearchList -filter Text [{0}] - StartTime:{1}", filterText,
                        DateTime.Now));

                if (string.IsNullOrEmpty(filterText)) return new List<dynamic>();

                var searchItemsBag = new List<dynamic>();
                var patientsBag = new List<dynamic>();
                var doctorsBag = new List<dynamic>();
                var inventoryBag = new List<dynamic>();
                var transactionBag = new List<dynamic>();
                var qtransactionBag = new List<dynamic>();
                var taskLst = new List<Task>();
                taskLst.Add(Task.Run(() => { searchItemsBag.AddRange(AddSearchItems()); }));

                taskLst.Add(Task.Run(() => { patientsBag.AddRange(GetPatients(filterText)); }));

                taskLst.Add(Task.Run(() => { doctorsBag.AddRange(GetDoctors(filterText)); }));
                taskLst.Add(Task.Run(() => { inventoryBag.AddRange(AddInventory(filterText)); }));


                double t = 0;
                if (double.TryParse(filterText, out t))
                {
                    taskLst.Add(Task.Run(() => { transactionBag.AddRange(AddTransactions(filterText)); }));

                    taskLst.Add(Task.Run(() => { qtransactionBag.AddRange(AddQuickTransactions(filterText)); }));

                }

                Task.WaitAll(taskLst.ToArray());

                Logger.Log(LoggingLevel.Info,
                    string.Format("Finish Create SearchList -filter Text [{0}] - StartTime:{1}", filterText,
                        DateTime.Now));
                transactionBag.AddRange(qtransactionBag);
                inventoryBag.AddRange(transactionBag);
                doctorsBag.AddRange(inventoryBag);
                patientsBag.AddRange(doctorsBag);
                searchItemsBag.AddRange(patientsBag);

                return searchItemsBag;
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error,
                    GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }


        private List<dynamic> AddSearchItems()
        {
            try
            {
                var cc = new List<dynamic>();

                SearchItem p = new SearchItem();
                p.SearchObject = null;
                p.SearchCriteria = "Add Patient";
                p.DisplayName = "Add Patient";
                cc.Add(p);

                SearchItem d = new SearchItem();
                d.SearchObject = null;
                d.SearchCriteria = "Add Doctor";
                d.DisplayName = "Add Doctor";
                cc.Add(d);

                SearchItem i = new SearchItem();
                i.SearchObject = null;
                i.SearchCriteria = "Add Drug";
                i.DisplayName = "Add Drug";
                cc.Add(i);


                return cc;
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error,
                    GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }


        private List<Prescription> AddTransactions(string filterText)
        {

            try
            {
                using (var ctx = new RMSModel())
                {
                    return
                        ctx.TransactionBase.OfType<Prescription>()
                            .Where(x => x.TransactionId.ToString().StartsWith(filterText))
                            .OrderBy(t => t.Time)
                            .Take(listCount).ToList();

                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error,
                    GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private List<QuickPrescription> AddQuickTransactions(string filterText)
        {

            try
            {
                using (var ctx = new RMSModel())
                {
                    // right now any Transactions
                    return
                        ctx.TransactionBase.OfType<QuickPrescription>()
                            .Where(x => x.TransactionId.ToString().StartsWith(filterText))
                            .OrderBy(t => t.Time)
                            .Take(listCount).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error,
                    GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }






        private List<Doctor> GetDoctors(string filterText)
        {
            try
            {
                using (var ctx = new RMSModel())
                {
                    return ctx.Persons.OfType<Doctor>()
                        .Where(
                            x =>
                                ("Dr. " + " " +
                                 x.FirstName.Trim().Replace(".", "").Replace(" ", "").Replace("Dr", "Dr. ") + " " +
                                 x.LastName.Trim() +
                                 " " + x.Code).Contains(filterText))
                        .Take(listCount).ToList();

                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error,
                    GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private List<Patient> GetPatients(string filterText)
        {
            try
            {
                using (var ctx = new RMSModel())
                {
                    return ctx.Persons.OfType<Patient>() //.Include(x => x.Photo).Include(x => x.AvailableRewards)
                        .Where(x => 
                            (x.CardId.Contains(filterText)) ||
                            (x.FirstName.Trim() + " " + x.LastName.Trim()).Contains(filterText) ||
                            (x.PhoneNumber.Replace("-", "").Contains(filterText.Replace("-", ""))) ||
                            (x.FirstName.Trim().Substring(0,1) + x.LastName.Trim().Substring(0, 1) + x.PhoneNumber.Trim().Replace("-", "").Replace("473","").Substring(0, 7) == filterText ))
                        .Take(listCount).ToList();

                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error,
                    GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        private int listCount = 40;

        private List<Medicine> AddInventory(string filterText)
        {
            try
            {
                //todo: make parallel
                using (var ctx = new RMSModel())
                {
                    ctx.Database.CommandTimeout = 0;

                    return ctx.Item.OfType<Medicine>().Where(x =>
                            ((x.ItemName ?? x.Description).Contains(filterText) ||
                             (x.ItemNumber.ToString().StartsWith(filterText)) ||
                             (x.ItemLookupCode.ToString().Contains(filterText)))
                            // && x.QBItemListID != null
                            // && x.Quantity > 0                           && 
                            && x.QBActive == true
                            && (x.Inactive == null ||
                                (x.Inactive != null && x.Inactive == _showInactiveItems)))
                        .OrderBy(x => x.ItemName)
                        .Take(listCount * 2).ToList();


                }


            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error,
                    GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }


        }


        private bool _showInactiveItems = false;

        public bool ShowInactiveItems
        {
            get { return _showInactiveItems; }
            set
            {
                _showInactiveItems = value;
                NotifyPropertyChanged(x => x.ShowInactiveItems);
            }

        }






        public void ProcessSearchListItem(object SearchItem)
        {
            try
            {
                if (SearchItem == null) return;
                if (TransactionData != null && TransactionData.ChangeTracker == null) TransactionData.StartTracking();
                if (typeof(RMSDataAccessLayer.SearchItem) == SearchItem.GetType())
                {
                    DoSearchItem(SearchItem as RMSDataAccessLayer.SearchItem);
                }

                if (typeof(RMSDataAccessLayer.Doctor) == SearchItem.GetType())
                {
                    AddDoctorToTransaction(SearchItem as Doctor);
                }

                if (typeof(RMSDataAccessLayer.Patient) == SearchItem.GetType())
                {
                    AddPatientToTransaction(SearchItem as Patient);
                }

                var searchItem = SearchItem as Item;
                if (searchItem != null)
                {

                    var itm = searchItem;
                    //  if (CheckDuplicateItem(itm)) return;
                    if (itm.Quantity < 0)
                    {
                        var res = MessageBox.Show("Item may not be in stock! Do you want to continue?",
                            "Negative Stock",
                            MessageBoxButton.YesNo);
                        if (res == MessageBoxResult.No) return;
                    }

                    SetCurrentItemDosage(itm);

                    if (TransactionData != null)
                    {
                        InsertItemTransactionEntry(itm);
                    }
                    else
                    {
                        NewItemTransaction(itm);
                    }

                }

                var trn = SearchItem as TransactionBase;
                if (trn != null)
                {
                    GoToTransaction(trn);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error,
                    GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
                throw ex;
            }
        }

        public void SetCurrentItemDosage(TransactionEntryItem itm)
        {
            if (itm.Item == null)
            {
                using (var ctx = new RMSModel())
                {
                    itm.Item = ctx.Item.FirstOrDefault(x => x.ItemNumber == itm.ItemNumber);
                    itm.TrackingState = TrackingState.Unchanged;
                }
            }

            SetCurrentItemDosage(itm.Item);
        }

        public void SetCurrentItemDosage(Item itm)
        {
            if (itm == null) return;
            using (var ctx = new RMSModel())
            {
                var res = new List<string>();

                var presetDosageLst = ctx.ItemPresetDosages.Where(x => x.ItemId == itm.ItemId)
                    .OrderByDescending(x => x.Id)
                    .Take(5)
                    .Select(x => x.Dosage)
                    .ToList();

                //var dosageLst = ctx.Database.SqlQuery<ItemDosage>($"Select * from ItemDosage where itemid = {itm.ItemId}")
                //    .OrderByDescending(x => x.Count)
                //    .Take(5 - presetDosageLst.Count)
                //    .Select(x => x.Dosage)
                //    .ToList();

                var dosageLst = ctx.Database.SqlQuery<string>($@"SELECT TOP (5)  Dosage
                FROM    ItemDosage
                where itemid = {itm.ItemId}
                ORDER BY Count DESC").ToList();

                res.AddRange(presetDosageLst);
                res.AddRange(dosageLst);
                itm.DosageList =
                    res;
                itm.TrackingState = TrackingState.Unchanged;
            }
        }

        //private bool CheckDuplicateItem(Item itm)
        //{
        //    if (TransactionData != null &&
        //        TransactionData.TransactionEntries.FirstOrDefault(x => x.TransactionEntryItem.ItemId == itm.ItemId) != null)
        //    {
        //        MessageBox.Show("Can't add same item twice!");
        //        return true;
        //    }
        //    return false;
        //}

        private void DoSearchItem(SearchItem searchItem)
        {
            throw new NotImplementedException();
        }


        private void AddPatientToTransaction(Patient patient)
        {
            if (patient == null) return;
            if (string.IsNullOrEmpty(patient.PhoneNumber))
            {
                var res = MessageBox.Show(
                    $"This patient's PhoneNumber is Missing. Which is necessary for Rewards Program." +
                    $"Ask Customer if they would like to participate. If so please update the Patient Details." +
                    $"Click OK Button to continue.", "Patient Phone Number Missing", MessageBoxButton.OK);
                //if (res == MessageBoxResult.Yes) return;
            }

            SetPatientFields(ref patient);

            Patient = patient;

            //if (TransactionData is Prescription == false)
            //{
            //    var t = NewPrescription();
            //    CopyTransactionDetails(t, TransactionData);
            //    DeleteTransactionData();
            //    TransactionData = t;
            //}
            dynamic prescription = TransactionData;
            if (prescription == null)
            {

                TransactionData = CreateNewQuickPrescription();
                TransactionData.StartTracking();
                prescription = TransactionData;

            }

            prescription.PatientId = patient.Id;
            prescription.Patient = patient;
            prescription.Patient.StartTracking();
            //if(this.patient.Discount > 0)
            foreach (var t in transactionData.TransactionEntries)
            {
                t.Discount = patient.Discount ?? 0;
            }

        }

    




private void AddDoctorToTransaction(Doctor doctor)
    {
        if (doctor == null) return;
        Doctor = doctor;
        if (TransactionData is Prescription == false)
        {
            var t = NewPrescription();
            if (TransactionData != null)
            {
                CopyTransactionDetails(t, TransactionData);
                if (((dynamic) TransactionData).Patient != null)
                {
                    t.Patient = ((dynamic) TransactionData).Patient;
                    t.PatientId = ((dynamic) TransactionData).PatientId;
                }

                DeleteTransactionData();
            }

            TransactionData = t;
        }

        var prescription = TransactionData as Prescription;
        if (prescription != null)
        {
            prescription.DoctorId = doctor.Id;
            prescription.Doctor = doctor;
            prescription.Doctor.StartTracking();
        }

    }


    private void GoToTransaction(TransactionBase trn)
    {
        GoToTransaction(trn.TransactionId);
    }


    public TransactionBase GoToTransaction(int TransactionId)
    {
        try
        {
            if (TransactionId == 0) return null;

            using (var ctx = new RMSModel())
            {
                TransactionBase ntrn;
                //ntrn = (from t in ctx.TransactionBase
                //        .Include(x => x.TransactionEntries)

                //        .Include(x => x.Cashier)
                //        .Include(x => x.TransactionLocation)
                //        .Include("TransactionEntries.TransactionEntryItem")
                //        .Include("TransactionEntries.TransactionEntryItem.Item")
                //        .Include("ParentTransaction.TransactionEntries.TransactionEntryItem.Item")
                //        .Include(x => x.ParentTransaction.Transactions)
                //        .Include("ParentTransaction.Transactions.TransactionEntries.TransactionEntryItem.Item")

                //            //.Include("TransactionEntries.Item.ItemDosages")
                //        where t.TransactionId == TransactionId
                //        orderby t.Time descending
                //        select t).FirstOrDefault();
                ntrn = GetDBTransaction(TransactionId);
                if (ntrn == null)
                {
                    return null;
                }
                ntrn.StartTracking();

                if (ntrn != null)
                {
                    IncludePrecriptionProperties(ctx, ntrn);
                    Item = null;
                    NotifyPropertyChanged(x => x.Item.DosageList);
                    TransactionData = ntrn;
                    NotifyPropertyChanged(x => x.TransactionData.TransactionLocation);
                    NotifyPropertyChanged(x => x.TransactionData.Position);
                }

                return ntrn;

            }
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error,
                GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }


    public void GoToPreviousTransaction()
    {
        try
        {
            using (var ctx = new RMSModel())
            {

                TransactionBase ptrn;

                if (TransactionData == null || TransactionData.TransactionId == 0)
                {
                    ptrn = GetDBTransaction(ctx).FirstOrDefault();
                }
                else
                {
                    ptrn = GetDBTransaction(ctx)
                        .FirstOrDefault(t => t.TransactionId < TransactionData.TransactionId);
                }

                if (ptrn != null) ptrn = GetDBTransaction(ptrn.TransactionId);
                NotifyPropertyChanged(x => x.TransactionData.TransactionLocation);
                NotifyPropertyChanged(x => x.TransactionData.Position);
                if (ptrn != null)
                {
                    ptrn.StartTracking();
                    IncludePrecriptionProperties(ctx, ptrn);
                    Item = null;
                    NotifyPropertyChanged(x => x.Item.DosageList);
                    NotifyPropertyChanged(x => x.Patient.Photo);
                        //  IncludeInventoryProperties(ctx, ptrn);
                        TransactionData = ptrn;
                    this.Item = null;
                }
                else
                {
                    MessageBox.Show("No previous transaction");
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error,
                GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }


    private TransactionBase GetDBTransaction(int transactionId)
    {
        try
        {
            //return (from t in ctx.TransactionBase
            //    .Include(x => x.TransactionEntries)
            //   .Include(x => x.Cashier)
            //    .Include(x => x.TransactionLocation)

            //    .Include("TransactionEntries.TransactionEntryItem")
            //     .Include("TransactionEntries.TransactionEntryItem.Item")
            //     .Include("ParentTransaction.TransactionEntries.TransactionEntryItem.Item")
            //      .Include(x => x.ParentTransaction.Transactions)
            if (transactionId == 0) return null;

            using (var ctx = new RMSModel())
            {
                //if (type == null)
                //{
                //    var trn = ctx.TransactionBase.FirstOrDefault(x => x.TransactionId == transactionId);
                //    type = trn.GetType().Name;
                //}

                var tds = ctx.TransactionDatas.Where(x => x.TransactionId == transactionId).ToList();
                if (!tds.Any()) return null; //tds = ctx.TransactionDatas.OrderByDescending(x => x.TransactionId).Take(1).ToList();
               
                return GetTransactions(tds).FirstOrDefault();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public List<TransactionBase> GetTransactions(List<TransactionData> data)
    {
        try
        {

            var transGrps = data.GroupBy(x => x.TransactionId).ToList();
            var reslst = new List<TransactionBase>();
            foreach (var tds in transGrps)
            {


                var header = tds.First();
                //int transactionId;
                //RMSModel ctx;
                //List<TransactionData> tds;
                var res = header.DoctorId != null //type == "Prescription"
                    ? (TransactionBase)new Prescription()
                    {
                        DoctorId = header.DoctorId.GetValueOrDefault(),
                        Doctor = tds.GroupBy(x => new
                            { x.DoctorId, x.DoctorFirstName, x.DoctorLastName }).Select(x =>
                            new Doctor()
                            {
                                Id = x.Key.DoctorId.GetValueOrDefault(),
                                FirstName = x.Key.DoctorFirstName,
                                LastName = x.Key.DoctorLastName,

                            }).FirstOrDefault(),
                        PatientId = header.PatientId.GetValueOrDefault(),
                        PrescriptionImage =
                            new RMSModel().PrescriptionImages.FirstOrDefault(x =>
                                x.TransactionId == header.TransactionId),
                        Patient = tds.GroupBy(x => new
                            { x.PatientId, x.PatientFirstName, x.PatientLastName, x.PatientPhoneNumber }).Select(x =>
                            new Patient()
                            {
                                Id = x.Key.PatientId.GetValueOrDefault(),
                                FirstName = x.Key.PatientFirstName,
                                LastName = x.Key.PatientLastName,
                                PhoneNumber = x.Key.PatientPhoneNumber,
                            }).FirstOrDefault()
                    }
                    : new QuickPrescription()
                    {
                        PatientId = header?.PatientId.GetValueOrDefault(),
                        Patient = tds.GroupBy(x => new
                            { x.PatientId, x.PatientFirstName, x.PatientLastName, x.PatientPhoneNumber }).Select(x =>
                            new Patient()
                            {
                                Id = x.Key.PatientId.GetValueOrDefault(),
                                FirstName = x.Key.PatientFirstName,
                                LastName = x.Key.PatientLastName,
                                PhoneNumber = x.Key.PatientPhoneNumber,
                            }).FirstOrDefault()
                    };

                res.TransactionId = header.TransactionId;
                res.Time = header.TransactionTime.GetValueOrDefault();
                res.BatchId = header.BatchId;
                res.CashierId = header.CashierId;
                res.CloseBatchId = header.CloseBatchId;
                res.StationId = header.StationId;
                res.PharmacistId = header.PharmacistId;
                res.Comment = header.Comment;
                res.Status = header.Status;
                res.ParentTransactionId = header.ParentTransactionId;
                res.DeliveryType = header.DeliveryType;





                res.Cashier = tds.GroupBy(x => new
                    { x.CashierId, x.LoginName, x.Initials, x.Role, x.CashierFirstName, x.CashierLastName }).Select(x =>
                    new Cashier()
                    {
                        LoginName = x.Key.LoginName,
                        Initials = x.Key.Initials,
                        Id = x.Key.CashierId,
                        Role = x.Key.Role,
                        FirstName = x.Key.CashierFirstName,
                        LastName = x.Key.CashierLastName
                    }).FirstOrDefault();
                res.Batch = new Batch();
                res.CloseBatch = new Batch();
                res.Pharmacist = tds.GroupBy(x => new { x.PharmacistId, x.PharmacistFirstName, x.PharmacistLastName })
                    .Select(x =>
                        new Cashier()
                        {
                            Id = x.Key.PharmacistId.GetValueOrDefault(),
                            FirstName = x.Key.PharmacistFirstName,
                            LastName = x.Key.PharmacistLastName
                        }).FirstOrDefault();
                res.TransactionLocation = tds.Where(x => x.Latitude.HasValue)
                    .GroupBy(x => new { x.TransactionId, x.Latitude, x.Longitude })
                    .Select(x => new TransactionLocation()
                    {
                        TransactionId = x.Key.TransactionId,
                        TransactionBase = res,
                        Latitude = x.Key.Latitude.GetValueOrDefault(),
                        Longitude = x.Key.Longitude.GetValueOrDefault()
                    }).FirstOrDefault();


                var trans = tds.GroupBy(x => new
                    {
                        x.TransactionEntryId,
                        x.Price,
                        x.Quantity,
                        x.Taxable,
                        x.SalesTaxPercent,
                        x.Discount,
                        x.EntryNumber,
                        x.ItemId,
                        x.ItemNumber,
                        x.Description,
                        x.ItemName,
                        x.Dosage,
                        x.Repeat,
                        x.RepeatQuantity,
                        x.isExtension,
                        x.QBItemListID
                    })
                    .Select(x => new PrescriptionEntry()
                    {
                        TransactionEntryId = x.Key.TransactionEntryId,
                        Transaction = res,
                        Price = x.Key.Price,
                        Quantity = x.Key.Quantity,
                        Repeat = x.Key.Repeat,
                        RepeatQuantity = x.Key.RepeatQuantity,
                        Taxable = x.Key.Taxable,
                        SalesTaxPercent = x.Key.SalesTaxPercent,
                        Discount = x.Key.Discount,
                        Dosage = x.Key.Dosage,
                        isExtension = x.Key.isExtension,
                        TransactionEntryItem = new TransactionEntryItem()
                        {
                            ItemId = x.Key.ItemId,
                            QBItemListID = x.Key.QBItemListID,
                            ItemNumber = x.Key.ItemNumber,
                            ItemName = x.Key.ItemName,
                            Item = new Medicine()
                            {
                                ItemId = x.Key.ItemId.GetValueOrDefault(),
                                ItemNumber = x.Key.ItemNumber,
                                ItemName = x.Key.ItemName,
                                Description = x.Key.Description
                            }
                        }
                    });

                res.TransactionEntries = new ObservableCollection<TransactionEntryBase>(trans);


                if (res != null && res?.ParentTransactionId != null)
                {
                    res.ParentTransaction = GetDBTransaction(res.ParentTransactionId.Value);
                    res.ParentTransaction.Transactions = new ObservableCollection<TransactionBase>(new RMSModel()
                        .TransactionBase
                        .Include("TransactionEntries.TransactionEntryItem.Item")
                        .Where(x => x.ParentTransactionId == res.ParentTransactionId.Value).ToList());
                }

                reslst.Add(res);
            }

            return reslst;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private IOrderedQueryable<TransactionBase> GetDBTransaction(RMSModel ctx)
    {
        try
        {

            TransactionBase ptrn;
            return (from t in ctx.TransactionBase
                        //    .Include(x => x.TransactionEntries)
                        //   .Include(x => x.Cashier)
                        //    .Include(x => x.TransactionLocation)

                        //    .Include("TransactionEntries.TransactionEntryItem")
                        //     .Include("TransactionEntries.TransactionEntryItem.Item")
                        //     .Include("ParentTransaction.TransactionEntries.TransactionEntryItem.Item")
                        //      .Include(x => x.ParentTransaction.Transactions)

                    orderby t.TransactionId  descending
                    select t);

            //return ptrn;
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error,
                GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }
    //private IOrderedQueryable<TransactionBase> GetDBTransaction(RMSModel ctx)
    //{
    //    try
    //    {
    //        TransactionBase ptrn;
    //        return (from t in ctx.TransactionBase
    //                .Include(x => x.TransactionEntries)
    //                .Include(x => x.Cashier)
    //                .Include(x => x.TransactionLocation)
    //                //  .Include(x => x.OldPrescription)
    //                //  .Include("OldPrescription.TransactionEntries")
    //                // .Include(x => x.Repeats)
    //                //  .Include("Repeats.TransactionEntries")
    //                .Include("TransactionEntries.TransactionEntryItem")
    //                .Include("TransactionEntries.TransactionEntryItem.Item")
    //                .Include("ParentTransaction.TransactionEntries.TransactionEntryItem.Item")
    //                .Include(x => x.ParentTransaction.Transactions)
    //                    //.Include("TransactionEntries.Item.ItemDosages")
    //                orderby t.Time descending
    //            select t);

    //        //return ptrn;
    //    }
    //    catch (Exception ex)
    //    {
    //        Logger.Log(LoggingLevel.Error,
    //            GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
    //        throw ex;
    //    }
    //}

    public void IncludePrecriptionProperties(TransactionBase ptrn)
    {
        try
        {
            using (var ctx = new RMSModel())
            {
                IncludePrecriptionProperties(ctx, ptrn);
            }
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error,
                GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }

    public void IncludePrecriptionProperties(RMSModel ctx, TransactionBase ptrn)
    {
        try
        {
            var tasklst = new List<Task>();
            if (ptrn is Prescription pc)
            {
                tasklst.Add(Task.Run(() => { SetOtherFields(pc); }));
                tasklst.Add(Task.Run(() => { SetTransactions(pc); }));
                // tasklst.Add(Task.Run(() => { SetParentTransactions(pc); }));
            }

            if (ptrn is QuickPrescription qp)
            {
                tasklst.Add(Task.Run(() => { SetOtherFields(qp); }));
            }

            tasklst.Add(Task.Run(() => { SetOtherTransactionFields(ptrn); }));
            Task.WaitAll(tasklst.ToArray());
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error,
                GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }

    private void SetOtherTransactionFields(TransactionBase ptrn)
    {
        using (var ctx = new RMSModel())
        {
            this.TransactionCashier = ctx.Persons.OfType<Cashier>().FirstOrDefault(x => x.Id == ptrn.CashierId);

            this.TransactionPharmacist =
                ctx.Persons.OfType<Cashier>().FirstOrDefault(x => x.Id == ptrn.PharmacistId);
        }


    }


    private static void SetTransactions(Prescription pc)
    {
        using (var ctx = new RMSModel())
        {
            pc.Transactions = ctx.TransactionBase.OfType<Prescription>().Include(x => x.Transactions)
                .Include("Transactions.TransactionEntries.TransactionEntryItem")
                .Include(x => x.TransactionEntries)
                .First(x => x.TransactionId == pc.TransactionId).Transactions;
        }
    }

    private static void SetParentTransactions(Prescription pc)
    {
        using (var ctx = new RMSModel())
        {
            pc.ParentTransaction = ctx.TransactionBase.OfType<Prescription>()
                .Include(x => x.Transactions)
                .Include("Transactions.TransactionEntries.TransactionEntryItem")
                .Include("TransactionEntries.TransactionEntryItem")
                .FirstOrDefault(x => x.TransactionId == pc.ParentTransactionId);
        }
    }
    private static void SetOtherFields(Prescription pc)
    {
        using (var ctx = new RMSModel())
        {
            pc.Doctor = ctx.Persons.OfType<Doctor>().FirstOrDefault(x => x.Id == pc.DoctorId);
            pc.Doctor.StartTracking();
            pc.Pharmacist = ctx.Persons.OfType<Cashier>().FirstOrDefault(x => x.Id == pc.PharmacistId);
            pc.Pharmacist?.StartTracking();
            pc.Patient = ctx.Persons.OfType<Patient>().Include(x => x.Photo).Include(x => x.AvailableRewards).Include(x => x.Membership).FirstOrDefault(x => x.Id == pc.PatientId);
            pc.Patient.StartTracking();
        }
    }

        private static void SetOtherFields(QuickPrescription qp)
        {
            using (var ctx = new RMSModel())
            {
            
                qp.Pharmacist = ctx.Persons.OfType<Cashier>().FirstOrDefault(x => x.Id == qp.PharmacistId);
                qp.Pharmacist?.StartTracking();
                qp.Patient = ctx.Persons.OfType<Patient>().Include(x => x.Photo).Include(x => x.AvailableRewards).Include(x => x.Membership).FirstOrDefault(x => x.Id == qp.PatientId);
                qp.Patient?.StartTracking();
                //Instance.Patient = qp.Patient; // Not working
                Instance.NotifyPropertyChanged(x => x.PatientAgeRange);

            }
        }

        private static void SetPatientFields(ref Patient qp )
        {
            using (var ctx = new RMSModel())
            {
                var id = qp.Id;
                qp = ctx.Persons.OfType<Patient>().Include(x => x.Photo).Include(x => x.AvailableRewards).Include(x => x.Membership).First(x => x.Id == id );
                qp.StartTracking();
                
               

            }
        }




        private void InsertItemTransactionEntry(RMSDataAccessLayer.Item itm)
    {
        try
        {
            var medicine = itm as Medicine;
            if (TransactionData.CurrentTransactionEntry == null)
            {

                PrescriptionEntry p = new PrescriptionEntry()
                {
                    StoreID = Store.StoreId,
                    TransactionId = TransactionData.TransactionId,
                    TransactionEntryItem = CreateTransactionEntryItem(itm),

                    Price = itm.Price,
                    Dosage = "Take as directed by your doctor ONLY!",//medicine == null?"":medicine.SuggestedDosage,
                    Taxable = itm.SalesTax != 0,
                    SalesTaxPercent = itm.SalesTax.GetValueOrDefault(),
                    TransactionTime = DateTime.Now,
                    EntryNumber =
                        TransactionData.TransactionEntries == null
                            ? 1
                            : (short?)TransactionData.TransactionEntries.Count,
                    // Transaction = TransactionData,

                    TrackingState = TrackingState.Added
                };
                p.StartTracking();

                TransactionData.TransactionEntries.Add(p);
                NotifyPropertyChanged(x => TransactionData.TransactionEntries);
                this.TransactionData.CurrentTransactionEntry = p;

            }
            else
            {
                var item = this.TransactionData.CurrentTransactionEntry;
                if (item != null)
                {
                    SetTransactionEntryItem(itm, item);

                    item.Price = itm.Price;

                    //if (medicine != null) item.Dosage = medicine.SuggestedDosage;
                    item.Dosage = "Take as directed by your doctor ONLY!";


                    this.TransactionData.UpdatePrices();
                }


                this.Item = itm;
            }





            NotifyPropertyChanged(x => x.TransactionData);
            //NotifyPropertyChanged(x => x.CurrentTransactionEntry);
            NotifyPropertyChanged(x => x.Item);
            return;
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }

    private TransactionEntryItem CreateTransactionEntryItem(Item itm)
    {
        if (itm == null) return null;
        return new TransactionEntryItem()
        {
            ItemId = itm.ItemId,
            ItemName = itm.ItemName ?? itm.Description,
            ItemNumber = itm.ItemNumber,
            QBItemListID = itm.QBItemListID,
            Item = itm,
            TrackingState = TrackingState.Added
        };
    }

    private TransactionEntryItem CreateTransactionEntryItem(TransactionEntryItem itm)
    {
        var newitm = GetCurrentQBInventoryItem(itm);
        if (newitm == null)
        {
            MessageBox.Show(
                $"Item --'{itm.ItemName}' Not Found In current Inventory. Please create this Prescription Entry Manually!");
            return null;
        }
        var ti = new TransactionEntryItem()
        {
            ItemId = newitm.ItemId,
            ItemName = newitm.ItemName,
            ItemNumber = newitm.ItemNumber,
            QBItemListID = newitm.QBItemListID,
            Item = newitm.Item,
            TrackingState = TrackingState.Added
        };

        return ti;
    }

    private TransactionEntryItem GetCurrentQBInventoryItem(TransactionEntryItem oldEntryItem)
    {
        using (var ctx = new RMSModel())
        {
            //if item exist and is qbactive return it.
            var eitm = ctx.QBInventoryItems
                 .FirstOrDefault(
                     x => x.ListID.ToString() == oldEntryItem.QBItemListID && x.Items.Any(z => z.ItemNumber == oldEntryItem.ItemNumber && z.QBActive.Value == true));
            if (eitm != null) return oldEntryItem;

            var res =
                ctx.QBInventoryItems
                .Include(x => x.Items)
                .FirstOrDefault(
                    x => x.ItemNumber.ToString() == oldEntryItem.ItemNumber && x.Items.Any(z => z.ItemNumber == oldEntryItem.ItemNumber && z.QBActive.Value == true));

            if (res != null)
            {
                var itm = res.Items.OrderByDescending(x => x.Inactive).FirstOrDefault();
                MessageBox.Show(
                    $"Existing Item {oldEntryItem.ItemName} don't exist in QBInventory, it will be replaced with {itm.ItemName}");
                return new TransactionEntryItem() { Item = itm, ItemNumber = itm.ItemNumber, ItemName = itm.ItemName, QBItemListID = res.ListID, ItemId = itm.ItemId, TrackingState = oldEntryItem.TrackingState, TransactionEntryId = oldEntryItem.TransactionEntryId, TransactionEntryBase = oldEntryItem.TransactionEntryBase };

            }
            return null;
        }
    }

    private static void SetTransactionEntryItem(Item itm, PrescriptionEntry item)
    {
        if (itm == null) return;
        if (item?.TransactionEntryItem == null) return;
        item.TransactionEntryItem.TransactionEntryId = item.TransactionEntryId;
        item.TransactionEntryItem.TrackingState = TrackingState.Modified;
        item.TransactionEntryItem.ItemId = itm.ItemId;
        item.TransactionEntryItem.ItemName = itm.ItemName ?? itm.Description;
        item.TransactionEntryItem.ItemNumber = itm.ItemNumber;
        item.TransactionEntryItem.QBItemListID = itm.QBItemListID;
        item.TransactionEntryItem.Item = itm;
    }


    private bool AutoCreateOldTransactions()
    {
        try
        {
            if (TransactionData == null) return false;
            if (TransactionData.Time.Date != DateTime.Now.Date)
            {
                MessageBox.Show(
                      "Modifying old transactions is not allowed! Do you want to create a New Transaction?",
                      "Can't Modify Old Transaction", MessageBoxButton.OK);
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }


    public void DeleteTransactionEntry<T>(TransactionEntryBase dtrn) where T : TransactionEntryBase
    {
        try
        {
            if (dtrn == null)
            {

                return;
            }
            if (AutoCreateOldTransactions() == false) return;

            using (var ctx = new RMSModel())
            {
                var d = ctx.TransactionEntryBase.FirstOrDefault(x => x.TransactionEntryId == dtrn.TransactionEntryId);
                if (d != null)
                {
                    d.TrackingState = TrackingState.Deleted;
                    ctx.ApplyChanges(d);
                    ctx.SaveChanges();
                    d.AcceptChanges();
                }
                //else
                //{
                    TransactionData.TransactionEntries.Remove(dtrn);
                //}

                //NotifyPropertyChanged(x => TransactionData.TransactionEntries);
                //NotifyPropertyChanged(x => TransactionData);
                //TransactionData.UpdatePrices();

            }
            GoToTransaction(TransactionData.TransactionId);
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }


    public void DeleteCurrentTransaction()
    {
        try
        {


            Logger.Log(LoggingLevel.Info,
                string.Format("Start DeleteCurrentTransaction: StartTime:{0}", DateTime.Now));
            if (
                MessageBox.Show("Are you sure you want to delete?", "Delete Current Transaction",
                    MessageBoxButton.YesNo) ==
                MessageBoxResult.Yes)
            {
                if (TransactionData != null && TransactionData.TransactionId > 0 && TransactionData.Time.Date != DateTime.Now.Date)
                {
                    MessageBox.Show("Modifying old transactions is not allowed!",
                        "Can't Modify Old Transaction");
                    return;
                }

                //if (TransactionData.Repeats.Any())
                //{
                //    MessageBox.Show("This Prescription has been repeated! ");
                //    return;
                //}
                DeleteTransactionData();
                GoToPreviousTransaction();

            }
            Logger.Log(LoggingLevel.Info,
                string.Format("Finish DeleteCurrentTransaction: EndTime:{0}", DateTime.Now));
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }

    private void DeleteTransactionData()
    {
        if (TransactionData != null)
        {
            using (var ctx = new RMSModel())
            {
                var t = ctx.TransactionBase.FirstOrDefault(x => x.TransactionId == TransactionData.TransactionId);
                if (t != null && TransactionData != null)
                {
                    t.TrackingState = TrackingState.Deleted;
                    ctx.ApplyChanges(t);
                    ctx.SaveChanges();
                }
                TransactionData.TrackingState = TrackingState.Deleted;
                // TransactionData.AcceptChanges();
            }
        }
        TransactionData = null;
    }


    public TransactionBase CopyCurrentTransaction(TransactionBase transactionData, bool copydetails = true)
    {
        try
        {
            using (var t = new TransactionScope())
            {
                dynamic newt = null;
                if (transactionData is Prescription)
                {
                    var p = NewPrescription();
                    p.StartTracking();

                    var doc = ((Prescription)transactionData).Doctor;
                    if (doc != null)
                    {
                        p.Doctor = doc;
                        p.DoctorId = (int)(p.Doctor.Id == 0? GetDoctors($"{p.Doctor.FirstName} {p.Doctor.LastName}").FirstOrDefault()?.Id??0: p.Doctor.Id);
                        p.Doctor.StartTracking();
                    }

                    var pat = ((Prescription)transactionData).Patient;
                    if (pat != null)
                    {
                        p.Patient = pat;
                        p.Patient.StartTracking();
                        p.PatientId = (int)(p.Patient.Id == 0 ? GetPatients(pat.DisplayName).FirstOrDefault()?.Id??0: p.Patient.Id);
                    }

                    var pPhoto = ((Prescription)transactionData).PrescriptionImage;
                    if (pPhoto != null)
                    {
                        p.PrescriptionImage = new PrescriptionImage() {TrackingState = TrackingState.Added};
                        p.PrescriptionImage.Image = pPhoto.Image;
                        p.PrescriptionImage.Path = pPhoto.Path;
                        p.PrescriptionImage.TransactionId = p.TransactionId;
                        //p.PrescriptionImage.StartTracking();

                    }

                        newt = p;

                    if (copydetails)
                    {
                        newt.Patient = ((Prescription)transactionData).Patient;
                        newt.PatientId = ((Prescription)transactionData).PatientId;
                        

                        }
                }

                if (TransactionData is QuickPrescription)
                {
                    newt = CreateNewQuickPrescription();

                    newt.StartTracking();

                    if (copydetails)
                    {
                        newt.Patient = ((QuickPrescription)transactionData).Patient;
                        newt.PatientId = ((QuickPrescription)transactionData).PatientId;
                        
                    }
                }
                CopyTransactionDetails(newt, transactionData);
                newt.UpdatePrices();
                t.Complete();
                return newt;
            }
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error, ex.Message + " | " + ex.StackTrace);
            throw ex;
        }
    }

    private void CopyTransactionDetails(dynamic newt, TransactionBase t)
    {
        if (newt == null || t == null) return;

        var entries = t.TransactionEntries.OfType<PrescriptionEntry>();
        //VerifyInventory(entries);

        foreach (var itm in entries)
        {
            if (CopyTransactionDetail(itm, out var te)) continue;

            newt.TransactionEntries.Add(te);
        }
    }

        private bool CopyTransactionDetail(PrescriptionEntry itm, out PrescriptionEntry te)
        {
            var ti = CreateTransactionEntryItem(itm.TransactionEntryItem);
            if (ti == null)
            {
                te = null;
                return true;
            }
            te = new PrescriptionEntry()
            {
                Dosage = itm.Dosage,
                TransactionEntryItem = ti,
                Repeat = itm.Repeat,
                RepeatQuantity = itm.RepeatQuantity, //?? Convert.ToInt32(itm.Quantity),
                Quantity = itm.RepeatQuantity.GetValueOrDefault() != 0
                    ? itm.RepeatQuantity.GetValueOrDefault()
                    : Convert.ToInt32(itm.Quantity),
                SalesTaxPercent = itm.SalesTaxPercent,
                Price = itm.Price,
                ExpiryDate = itm.ExpiryDate,
                Comment = itm.Comment,

                TrackingState = TrackingState.Added
            };


            te.StartTracking();
            return false;
        }

        private void VerifyInventory(IEnumerable<PrescriptionEntry> entries)
    {
        using (var ctx = new RMSModel())
        {
            foreach (var itm in entries)
            {
                var inv = ctx.Item.FirstOrDefault(x => x.QBItemListID == itm.TransactionEntryItem.QBItemListID && x.QBActive == true);
                if (inv == null)
                    MessageBox.Show(
                        $"{itm.TransactionEntryItem.ItemName}-{itm.TransactionEntryItem.ItemNumber} is not Availible in QuickBooks! please Re-Create item.");
            }
        }
    }


    public void AutoRepeat(TransactionBase data = null)
    {
        var myTransactionData = data ?? TransactionData;
        try
        {
                //var pres = myTransactionData as Prescription;
                //if (pres == null)
                //{
                //    MessageBox.Show("Only Transactions can be repeated.");
                //    return;
                //}
                var pres = GoToParentIfChild(myTransactionData);

                var newt = CopyingCurrentTransaction();


                newt.ParentTransactionId = pres.ParentTransactionId ?? pres.TransactionId;
            newt.ParentTransaction = pres;


            RemoveFilledTransactions(newt, pres);


            if (newt.TransactionEntries.Any())
            {
                TransactionData = newt;

                // relink items based on item codes because was deleted
                //var missingitems = TransactionData.TransactionEntries.Any(x => x.TransactionEntryItem.ItemId == null);
                //if (missingitems)
                //{
                    RelinkTransactions();
                //}

                
                if (AutoRepeatSaveTransactions(newt)) return;
            }
            else
            {
                MessageBox.Show("Prescription is completely filled.");
                return;
            }
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw;
        }

    }

    private bool AutoRepeatSaveTransactions(TransactionBase newt)
    {
        Logger.Log(LoggingLevel.Info, $"Start: AutoRepeatSaveTransactions - {DateTime.Now:O}");
            if (!SaveTransaction()) return true;
        SalesVM.Instance.GoToTransaction(newt.TransactionId);
        Logger.Log(LoggingLevel.Info, $"End: AutoRepeatSaveTransactions - {DateTime.Now:O}");
            return false;

    }

    private void RelinkTransactions()
    {
        Logger.Log(LoggingLevel.Info, $"Start: RelinkTransactions - {DateTime.Now:O}");
        using (var ctx = new RMSModel())
        {
            foreach (var trn in TransactionData.TransactionEntries //.Where(x => x.TransactionEntryItem.ItemId == null)
                    )
            {
                var itm = ctx.Item.FirstOrDefault(x =>
                    x.ItemNumber == trn.TransactionEntryItem.ItemNumber &&
                    x.ItemName == trn.TransactionEntryItem.ItemName);
                if (itm == null) continue;
                trn.TransactionEntryItem.ItemId = itm.ItemId;
                trn.TransactionEntryItem.QBItemListID = itm.QBItemListID;
                trn.TransactionEntryItem.Item = itm;
                trn.Price = itm.Price;
            }
        }

        Logger.Log(LoggingLevel.Info, $"End: RelinkTransactions - {DateTime.Now:O}");
    }

    private static void RemoveFilledTransactions(TransactionBase newt, TransactionBase pres)
    {
        Logger.Log(LoggingLevel.Info, $"Start: RemoveFilledTransactions - {DateTime.Now:O}");
        foreach (PrescriptionEntry item in newt.TransactionEntries.ToList())
        {
            item.Transaction = newt;
            if (item.Remaining == 0 && pres is Prescription)
            {
                newt.TransactionEntries.Remove(item);
                continue;
            }
            // item.Quantity = item.Remainder > 0 ? item.Remainder : item.RepeatQuantity.GetValueOrDefault();
        }

        Logger.Log(LoggingLevel.Info, $"End: RemoveFilledTransactions - {DateTime.Now:O}");
    }

    private TransactionBase CopyingCurrentTransaction()
    {
        Logger.Log(LoggingLevel.Info, $"Start: CopyCurrentTransaction - {DateTime.Now:O}");

        var newt = CopyCurrentTransaction(TransactionData);

        Logger.Log(LoggingLevel.Info, $"End: CopyCurrentTransaction - {DateTime.Now:O}");
        return newt;
    }

    private TransactionBase GoToParentIfChild(TransactionBase myTransactionData)
    {
        Logger.Log(LoggingLevel.Info, $"Start: GoToParentIfChild - {DateTime.Now:O}");
        var pres = myTransactionData;
        if (pres.ParentTransactionId != null)
        {
            GoToTransaction(pres.ParentTransactionId.GetValueOrDefault());
        }

        Logger.Log(LoggingLevel.Info, $"End: GoToParentIfChild - {DateTime.Now:O}");
        return pres;
    }


    private void NewItemTransaction(Item SearchItem)
    {
        try
        {
            //  if (CheckDuplicateItem(SearchItem)) return;
            if (TransactionData == null)
            {
                TransactionData = CreateNewQuickPrescription();
                TransactionData.StartTracking();
            }
            InsertItemTransactionEntry(SearchItem as Item);
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }

    private QuickPrescription CreateNewQuickPrescription()
    {
        try
        {
            if (TransactionPharmacist?.Id != CurrentLogin.Id) TransactionPharmacist = null;
            return new QuickPrescription()
            {
                BatchId = Batch.BatchId,
                StationId = Station.StationId,
                Time = DateTime.Now,
                CashierId = CurrentLogin.Id,
                //PharmacistId = (CurrentLogin.Role == "Pharmacist" ? CurrentLogin.Id : null as int?),
                StoreCode = Store.StoreCode,
                OpenClose = true,
                TrackingState = TrackingState.Added
            };

        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }




    public void Print(ref FrameworkElement fwe, PrescriptionEntry prescriptionEntry)
    {
        try
        {

            PrintServer printserver = Station.PrintServer.StartsWith("\\")
                                          ? new PrintServer(Station.PrintServer)
                                          : new LocalPrintServer();
            this.Print(fwe, printserver);

        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Print error! Please check prints and reprint and also tell joseph you saw this error in SalesVM.");
            Instance.UpdateTransactionEntry(ex, prescriptionEntry);
            Logger.Log(
                LoggingLevel.Error,
                GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            //  throw ex;
        }
    }

    private void Print(FrameworkElement fwe, PrintServer printserver)
    {
        Size visualSize;

        visualSize = new Size(288, 2 * 96); // paper size

        DrawingVisual visual =
            PrintControlFactory.CreateDrawingVisual(fwe, fwe.ActualWidth, fwe.ActualHeight);//


        SUT.PrintEngine.Paginators.VisualPaginator page = new SUT.PrintEngine.Paginators.VisualPaginator(
            visual, visualSize, new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, 0));
        page.Initialize(false);

        PrintDialog pd = new PrintDialog();
        pd.PrintQueue = printserver.GetPrintQueue(this.Station.ReceiptPrinterName);

        pd.PrintDocument(page, "");
    }

    public void Print(ref FrameworkElement fwe)
    {
        try
        {

            if (Station.PrintServer.StartsWith("\\"))
            {
                PrintServer printserver = new PrintServer(Station.PrintServer);


                Size visualSize;

                visualSize = new Size(288, 2 * 96); // paper size

                DrawingVisual visual =
                    PrintControlFactory.CreateDrawingVisual(fwe, fwe.ActualWidth, fwe.ActualHeight);//


                SUT.PrintEngine.Paginators.VisualPaginator page = new SUT.PrintEngine.Paginators.VisualPaginator(
                    visual, visualSize, new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, 0));
                page.Initialize(false);

                PrintDialog pd = new PrintDialog();
                pd.PrintQueue = printserver.GetPrintQueue(Station.ReceiptPrinterName);

                pd.PrintDocument(page, "");
            }
            else
            {
                LocalPrintServer printserver = new LocalPrintServer();


                Size visualSize;

                visualSize = new Size(288, 2 * 96); // paper size

                DrawingVisual visual =
                    PrintControlFactory.CreateDrawingVisual(fwe, fwe.ActualWidth, fwe.ActualHeight);


                SUT.PrintEngine.Paginators.VisualPaginator page = new SUT.PrintEngine.Paginators.VisualPaginator(
                    visual, visualSize, new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, 0));
                page.Initialize(false);

                PrintDialog pd = new PrintDialog();
                pd.PrintQueue = printserver.GetPrintQueue(Station.ReceiptPrinterName);

                pd.PrintDocument(page, "");
            }



        }
        catch (Exception ex)
        {
            MessageBox.Show("Print error! Please check prints and reprint and also tell Joseph you saw this error in SalesVM.");
            //Instance.UpdateTransactionEntry(ex, prescriptionEntry);
            Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            //  throw ex;
        }
    }



    public void PostQBSale()
    {

        try
        {

            if (TransactionData == null || string.IsNullOrEmpty(TransactionData.TransactionNumber))
            {
                MessageBox.Show("Invalid Transaction Please Try again");
                return;
            }
            if (TransactionData.ChangeTracker == null) TransactionData.StartTracking();
            TransactionData.Status = "ToBePosted";
            if (!SaveTransaction())
            {
                MessageBox.Show("Post failed to Save! Please Check that all fields are entered and try again.");
                return;
            }
            if (ServerMode != true)
            {
                Post(TransactionData);
            }

        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }



    private void Post(TransactionBase TransactionData)
    {
        try
        {

            IncludePrecriptionProperties(TransactionData);

            SalesReceipt s = new SalesReceipt();
            s.TxnDate = TransactionData.Time;
            s.TxnState = "1";
            s.Workstation = "02";
            s.StoreNumber = "1";
            s.SalesReceiptNumber = "123";
            s.Discount = "0";


            if (TransactionData is Prescription)
            {
                Prescription p = TransactionData as Prescription;
                string doctor = "";
                string patient = "";
                if (p.Doctor != null)
                {
                    doctor = p.Doctor.DisplayName;
                    s.Discount = p.Doctor.Discount == null ? "" : p.Doctor.Discount.ToString();
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
                s.Comments = "RX#:" + TransactionData.TransactionNumber;
            }

            if (TransactionData != null)
            {
                s.TrackingNumber = TransactionData.TransactionNumber;

            }
            s.Associate = "Dispensary";
            s.SalesReceiptType = "0";



            foreach (var item in TransactionData.TransactionEntries)
            {
                if (item.TransactionEntryItem?.QBItemListID != null)
                {

                    s.SalesReceiptDetails.Add(new SalesReceiptDetail
                    {
                        ItemListID = item.TransactionEntryItem.QBItemListID,
                        QtySold = (Decimal)item.Quantity
                    }); //340 
                }
                else
                {
                    ////MessageBox.Show("Please Link Quickbooks item to dispensary");
                    //TransactionData.Status = "Please Link Quickbooks item to dispensary";
                    //rms.SaveChanges();
                    continue;
                }
            }


            // QBPOS qb = new QBPOS();
            SalesReceiptRet result = QBPOS.Instance.AddSalesReceipt(s);
            if (result != null)
            {
                TransactionData.ReferenceNumber = "QB#" + result.SalesReceiptNumber;
                TransactionData.Status = "Posted";
                SaveTransaction(TransactionData);
                //using (var ctx = new RMSModel())
                //{
                //    TransactionData.ReferenceNumber = "QB#" + result.SalesReceiptNumber;
                //    TransactionData.Status = "Posted";

                //    //ctx.TransactionBase.AddOrUpdate(TransactionData);
                //    //ctx.SaveChanges();
                //}
            }
            else
            {
                // problem
            }
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }

    public async Task DownloadAllQBItems()
    {
        try
        {
            await Task.Run(() =>
            {
                var t = QBPOS.Instance.GetAllInventoryQuery().Result;
                ProcessQBItems(t);
            }).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }

    private async Task ProcessQBItems(List<ItemInventoryRet> itms)
    {
        try
        {
            if (itms != null)
            {
                var itmcnt = 0;
                List<Medicine> clst = null;
                using (var ctx = new RMSModel())
                {
                    clst = ctx.Item.OfType<Medicine>()
                        .Where(x => x.QBItemListID != null)
                        // .Where(x => x.ItemNumber == "6315")
                        .ToList();
                }
                Parallel.ForEach(itms, (item) => //.Where(x => x.ItemNumber == 6315)
                {
                    //if (itmcnt%100 == 0)
                    //{
                    //    ctx.SaveChanges(); //SaveDatabase();
                    //}
                    using (var ctx = new RMSModel())
                    {
                        QBInventoryItem i = ctx.QBInventoryItems.FirstOrDefault(x => x.ListID == item.ListID);
                        if (i == null)
                        {
                            i = new QBInventoryItem()
                            {
                                ListID = item.ListID,
                                ItemName = item.Desc1,
                                ItemDesc2 = item.Desc2,
                                Size = item.Size,
                                DepartmentCode = "MISC",
                                ItemNumber = System.Convert.ToInt16(item.ItemNumber),
                                TaxCode = item.TaxCode,
                                Price = System.Convert.ToDouble(item.Price1),
                                Quantity = System.Convert.ToDouble(item.QuantityOnHand),
                                UnitOfMeasure = item.UnitOfMeasure
                            };

                            ctx.QBInventoryItems.Add(i);
                        }

                        i.ItemName = item.Desc1;
                        i.ItemDesc2 = item.Desc2;
                        i.ListID = item.ListID;
                        i.Size = item.Size;
                        i.UnitOfMeasure = item.UnitOfMeasure;
                        i.TaxCode = item.TaxCode;
                        i.ItemNumber = System.Convert.ToInt16(item.ItemNumber);
                        i.Price = System.Convert.ToDouble(item.Price1);
                        i.Quantity = System.Convert.ToDouble(item.QuantityOnHand);

                        ctx.QBInventoryItems.AddOrUpdate(i);

                        Medicine itm = clst.FirstOrDefault(x => x.QBItemListID == i.ListID);
                        if (itm == null)
                        {
                            itm = new Medicine()
                            {
                                DateCreated = DateTime.Now,
                                SuggestedDosage = "Take as Directed by your Doctor"
                            };

                            ctx.Item.Add(itm);
                        }

                        if (itm != null)
                        {
                            itm.Description = i.ItemDesc2;
                            itm.Price = i.Price.GetValueOrDefault();
                            itm.Quantity = Convert.ToDouble(i.Quantity);
                            itm.SalesTax = (i.TaxCode != null && i.TaxCode.ToUpper() == "VAT"
                                ? .15
                                : 0);
                            itm.QBItemListID = i.ListID;
                            itm.UnitOfMeasure = i.UnitOfMeasure;
                            itm.ItemName = i.ItemName;
                            itm.ItemNumber = i.ItemNumber.ToString();
                            itm.Size = i.Size;
                            ctx.Item.AddOrUpdate(itm);
                        }
                        ctx.SaveChanges();
                    }
                    // itmcnt += 1;
                });
                //SaveDatabase();
            }
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }

    public void Notify(string token, object sender, NotificationEventArgs e)
    {
        MessageBus.Default.Notify(token, sender, e);
    }






    private Item item = null;

    public Item Item
    {
        get { return item; }
        set
        {
            if (item != null)
            {
                item = value;
                NotifyPropertyChanged(x => x.Item);
            }
        }
    }

    private ObservableCollection<TransactionsView> transactionList = null;

    public ObservableCollection<TransactionsView> TransactionList
    {
        get { return transactionList; }
        set
        {
            if (transactionList != value)
            {
                transactionList = value;
                NotifyPropertyChanged(x => x.TransactionList);
            }
        }
    }

    public Patient CreateNewPatient(string searchtxt)
    {
        var p = CreateNewPatient();
        p.StartTracking();
        SetNames(searchtxt, p);
        return p;
    }

    private void SetNames(string searchtxt, Person p)
    {
        var strs = searchtxt.Split(' ');
        p.FirstName = strs.FirstOrDefault();
        p.LastName = strs.LastOrDefault();
    }

    public Patient CreateNewPatient()
    {
        return new Patient()
        {
            TotalSales = 0,
            StartingSales = 0,
            TrackingState = TrackingState.Added
        };
    }

    public bool SavePerson(Person patient)
    {
        
        if (string.IsNullOrEmpty(patient.PhoneNumber))
        {
            MessageBox.Show("Please enter PhoneNumber");
            return false;
        }
        try
        {
            
            using (var ctx = new RMSModel())
            {
                if (patient.Id == 0)
                {
                    var existingPatient = ctx.Persons.FirstOrDefault(x =>
                        x.FirstName.Trim().ToLower() == patient.FirstName.Trim().ToLower()
                        && x.LastName.Trim().ToLower() == patient.LastName.Trim().ToLower()
                        && (x.PhoneNumber ?? x.Address).Trim().ToLower() ==
                        (patient.PhoneNumber ?? patient.Address).Trim().ToLower());

                    if (existingPatient != null)
                    {
                        MessageBox.Show(
                            "Another patient already exists with same Name and Phone/Address! Please select existing Patient from list or Fill out PhoneNumber and Address correctly");
                        return false;
                    }
                }

                ctx.ApplyChanges(patient);
                ctx.SaveChanges();
                patient.AcceptChanges();
                //ctx.Persons.AddOrUpdate(patient);
                //ctx.SaveChanges();
            }
            
            return true;
        }
        catch (DbEntityValidationException vex)
        {
            var str = vex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Aggregate("", (current, er) => current + (er.PropertyName + ","));
            MessageBox.Show("Please Check the following fields before saving! - " + str);
            return false;
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }

    public List<TransactionBase> GetPatientTransactionList(Patient p)
    {
        using (var ctx = new RMSModel())
        {
            return
                new List<TransactionBase>(
                    ctx.TransactionBase.OfType<Prescription>().Where(x => x.PatientId == p.Id).ToList());
        }
    }

    public List<TransactionBase> GetDoctorTransactionList(Doctor d)
    {
        using (var ctx = new RMSModel())
        {
            return
                new List<TransactionBase>(
                    ctx.TransactionBase.OfType<Prescription>().Where(x => x.DoctorId == d.Id).ToList());
        }
    }


    public Doctor CreateNewDoctor(string searchtxt)
    {
        var d = CreateNewDoctor();
        d.StartTracking();
        SetNames(searchtxt, d);
        return d;
    }
    public Doctor CreateNewDoctor()
    {
        return new Doctor() { TrackingState = TrackingState.Added };
    }

    public bool SaveTransaction()
    {
        var res = SaveTransaction(TransactionData);
        NotifyPropertyChanged(x => x.TransactionData);
        return res;

    }

        private int QuickSaleDosagePrintLength = 195;
        private int QuickSalesDosageExtensionPrintLength = 260;

        private int PrescriptionDosagePrintLength = 170;
        private int PrescriptionDosageExtensionPrintLength = 260;

        public bool SaveTransaction(TransactionBase trans)
    {
        try
        {

            if (trans != null && trans.GetType() == typeof(Prescription))
            {
                var p = trans as Prescription;
                if (p.Doctor == null)
                {
                    MessageBox.Show("Please Select a doctor");
                    return false;
                }

                if (p.Patient == null)
                {
                    MessageBox.Show("Please Select a Patient");
                    return false;
                }

                if (string.IsNullOrEmpty(p.Patient.PhoneNumber))
                {
                    MessageBox.Show("Please Enter Patient's Phone Number");
                    return false;
                }


            }

            if (trans != null && trans.GetType() == typeof(QuickPrescription))
            {
                var p = trans as QuickPrescription;
              
                if (p?.Patient != null)
                    if (string.IsNullOrEmpty(p.Patient.PhoneNumber))
                    {
                        MessageBox.Show("Please Enter Patient's Phone Number");
                        return false;
                    }


            }



                if (trans != null)
                {
                    if (!trans.TransactionEntries.Any())
                    {
                        MessageBox.Show("Transaction must have items, please delete if no Items.");
                        return false;
                    }

                    var longdosage = trans.TransactionEntries.OfType<PrescriptionEntry>()
                        .Where(x => x.Dosage.Length > (trans is Prescription ? PrescriptionDosagePrintLength: QuickSaleDosagePrintLength)).ToList();
                
                    if (longdosage.Any())
                    {
                        var tlst = trans.TransactionEntries.ToList();
                        foreach (var longdose in longdosage)
                        {
                            var doselst = SplitSentences(longdose.Dosage, (trans is Prescription ? PrescriptionDosagePrintLength : QuickSaleDosagePrintLength));

                            longdose.isExtension = false;

                            PrescriptionEntry te = null;
                            for (int i = 0; i < doselst.Count ; i++)
                            {
                                string itm = doselst[i];
                                if (itm == longdose.Dosage) break; // unbroken so abort
                           
                                if (i > 0 && te?.Dosage.Length + itm.Length > (tlst.Count() == 1 ? (trans is Prescription ? PrescriptionDosagePrintLength : QuickSaleDosagePrintLength) : (trans is Prescription ? PrescriptionDosageExtensionPrintLength : QuickSalesDosageExtensionPrintLength)))
                                {
                                    if(te.Key != longdose.Key) tlst.Add(te);
                                    CopyTransactionDetail(longdose, out te);
                                    te.Quantity = 0;
                                    te.Dosage = "";
                                    te.isExtension = true;

                                    tlst.Add(te);
                                }
                                else
                                {
                                    if (te == null)
                                    {
                                        te = longdose;
                                        //if (te?.Dosage.Length > dosagePrintLength) i = 0;
                                        te.Dosage = "";
                                        
                                    }

                                }

                          
                                te.Dosage += itm;
                          

                            


                            }

                            if (te != null && !tlst.Contains(te))
                            {
                                tlst.Add(te);
                            }
                        }

                        trans.TransactionEntries = new ObservableCollection<TransactionEntryBase>(tlst);
                    }
                    else
                    {
                        
                            foreach (PrescriptionEntry transTransactionEntry in trans?.TransactionEntries)
                            {
                                transTransactionEntry.isExtension = null;
                            }
                    }

                }


                var saveTransactionToDb = SaveTransactionToDB(trans);
                if(trans!= null) trans = GetDBTransaction(trans.TransactionId);
                return saveTransactionToDb;

        }

        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error,
                GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw;
        }
    }

        private List<string> SplitSentences(string sSourceText, int dosagePrintLength)

        {

            // create a local string variable

            // set to contain the string passed it

            string sTemp = sSourceText;



            // create the array list that will

            // be used to hold the sentences

            List<string> al = new List<string>();



            // split the sentences with a regular expression

            string[] splitSentences = Regex.Split(sTemp, @"(?<=['""A-Za-z0-9\s][\,\.\!\?]+)");



            // loop the sentences

            for (int i = 0; i < splitSentences.Length; i++)

            {

                // clean up the sentence one more time, trim it,

                // and add it to the array list

                

                string sSingleSentence = splitSentences[i].Replace(Environment.NewLine, string.Empty);

                if (sSingleSentence.Length > dosagePrintLength)
                {
                    var rlength = sSingleSentence;
                    while (rlength.Length > dosagePrintLength)
                    {
                        var s = rlength.Substring(0, dosagePrintLength);
                        al.Add(s.Trim());
                        rlength = rlength.Substring(dosagePrintLength+1);
                    }
                    al.Add(rlength.Trim());
                }
                else
                {
                    al.Add(sSingleSentence.Trim());
                }
                

            }

            // return the arraylist with

            // all sentences added

            return al;

        }


        private static object _lockObject = new Object();
    private bool SaveTransactionToDB(TransactionBase trans)
    {
        if (trans == null) return true;
        using (var ctx = new RMSModel())
        {
            try
            {
                try
                {

                    if (DateTime.TryParse(trans.Time.ToString(), out DateTime transDate) == false) transDate = DateTime.Now;

                    var sql = "";
                    int res;
                    if (trans.TransactionId == 0)
                    {
                        sql += $@"  Insert into TransactionBase (StationId, ParentTransactionId, BatchId, CloseBatchId, Time, CustomerId, PharmacistId, CashierId, Comment, ReferenceNumber, StoreCode, OpenClose, Status, DeliveryType)
                                    OUTPUT Inserted.TransactionId
                        VALUES        ({trans.StationId},'{trans.ParentTransactionId}',{trans.BatchId},{trans.CloseBatchId},'{transDate}','{trans.CustomerId}',{trans.PharmacistId},{trans.CashierId},'{trans.Comment?.Replace("'", "''")}','{trans.ReferenceNumber}','{trans.StoreCode}',{trans.OpenClose},'{trans.Status?.Replace("'", "''")}','{trans.DeliveryType}')";
                        sql = CleanSql(sql);
                        res = ctx.Database.SqlQuery<int>(sql).FirstOrDefault();
                        sql = "";
                        trans.TransactionId = res;
                        if (trans is Prescription p)
                        {
                            sql = $@"  INSERT INTO TransactionBase_Prescription
                                                             (DoctorId, PatientId, TransactionId)
                                    VALUES        ({p.DoctorId},{p.PatientId},{p.TransactionId})";
                     

                        }

                        if (trans is QuickPrescription qp)
                            {
                            sql = $@"  INSERT INTO TransactionBase_QuickPrescription
                                                             (TransactionId,PatientId)
                                    VALUES        ({qp.TransactionId}, {(qp.PatientId == null || qp.PatientId == 0 ? "NULL" : qp.PatientId.ToString())})";
                        }
                    }
                    else
                    {
                        sql += $@"  Update TransactionBase Set StationId = {trans.StationId}, ParentTransactionId = '{trans.ParentTransactionId}', BatchId = {trans.BatchId}, CloseBatchId = '{trans.CloseBatchId}', Time = '{transDate}',
                                                            CustomerId = '{trans.CustomerId}', PharmacistId = '{trans.PharmacistId}', CashierId = '{trans.CashierId}', Comment = '{trans.Comment}',
                                                            ReferenceNumber = '{trans.ReferenceNumber}', StoreCode = '{trans.StoreCode}', OpenClose = '{trans.OpenClose}', Status = '{trans.Status}', DeliveryType = '{trans.DeliveryType}'
                                 Where TransactionId = {trans.TransactionId}";


                        if (trans is Prescription p)
                        {
                            sql += $@"  Update TransactionBase_Prescription Set DoctorId = {p.DoctorId}, PatientId = {p.PatientId}
                                    Where TransactionId = {trans.TransactionId}";
                            
                        }

                        if (trans is QuickPrescription qp)
                        {
                            sql += $@"  Update TransactionBase_QuickPrescription Set PatientId = {(qp.PatientId == null || qp.PatientId == 0 ? "NULL": qp.PatientId.ToString())}
                                    Where TransactionId = {trans.TransactionId}";
                            }

                    }

                    if (trans.TransactionLocation != null)
                    {
                        if (trans.TransactionLocation.TransactionId == 0)
                        {
                            sql += $@"  INSERT INTO       TransactionLocation(TransactionId,Longitude,  Latitude)
                                       VALUES        ({trans.TransactionId},{trans.TransactionLocation.Longitude},{trans.TransactionLocation.Latitude})";
                        }
                        else
                        {
                            sql += $@"  UPDATE       TransactionLocation
                                        SET                 Longitude = '{trans.TransactionLocation.Longitude}', Latitude = '{trans.TransactionLocation.Latitude}' 
                                        Where TransactionId = {trans.TransactionId}";
                        }

                    }

                    

                    foreach (var trn in trans.TransactionEntries.OfType<PrescriptionEntry>())
                    {
                        if (trn.TransactionEntryId == 0)
                        {
                            var trnsql =
                                $@"  INSERT INTO TransactionEntryBase  (StoreID,  Price, TransactionId, Quantity, Taxable, Comment, TransactionTime, SalesTaxPercent, Discount, EntryNumber)
                                        output INSERTED.TransactionEntryId
                                        VALUES        ({trn.StoreID},{trn.Price},{trans.TransactionId},{trn.Quantity},{trn.Taxable},'{trn.Comment}','{DateTime.Now}',{trn.SalesTaxPercent},{trn.Discount},'{trn.EntryNumber}')";
                            trnsql = CleanSql(trnsql);
                            var trnres = ctx.Database.SqlQuery<int>(trnsql).FirstOrDefault();
                            trn.TransactionEntryId = trnres;
                            sql += $@"  INSERT INTO TransactionEntryBase_PrescriptionEntry
                                        (Dosage, ExpiryDate, TransactionEntryId, Repeat, RepeatQuantity, isExtension)
                                        VALUES        ('{trn.Dosage.Replace("'", "''")}','{trn.ExpiryDate}',{trn.TransactionEntryId},'{trn.Repeat}','{trn.RepeatQuantity}','{trn.isExtension}')";

                            sql += $@"  INSERT INTO TransactionEntryItem
                                                                     (TransactionEntryId, QBItemListID, ItemNumber, ItemName, ItemId)
                                            VALUES        ({trn.TransactionEntryId},'{trn.TransactionEntryItem.QBItemListID}','{trn.TransactionEntryItem.ItemNumber}','{trn.TransactionEntryItem.ItemName.Replace("'", "''")}','{trn.TransactionEntryItem.ItemId}')";
                        }
                        else
                        {
                            sql += $@"  UPDATE       TransactionEntryBase
                                      SET                StoreID = {trn.StoreID}, Price = '{trn.Price}', Quantity = {trn.Quantity}, Taxable = '{trn.Taxable}', Comment = '{trn.Comment}', TransactionTime = '{DateTime.Now}', SalesTaxPercent = '{trn.SalesTaxPercent}', Discount = '{trn.Discount}', EntryNumber = '{trn.EntryNumber}'
                                        Where TransactionEntryId = {trn.TransactionEntryId}";

                            sql += $@"  UPDATE       TransactionEntryBase_PrescriptionEntry
                                            SET                Dosage = '{trn.Dosage.Replace("'", "''")}', ExpiryDate = '{trn.ExpiryDate}', Repeat = '{trn.Repeat}', RepeatQuantity = '{trn.RepeatQuantity}', IsExtension = '{trn.isExtension}'
                                        Where TransactionEntryId = {trn.TransactionEntryId}";
                            sql += $@"  UPDATE       TransactionEntryItem
                                        SET                 QBItemListID = '{trn.TransactionEntryItem.QBItemListID}', ItemNumber = '{trn.TransactionEntryItem.ItemNumber}', ItemName = '{trn.TransactionEntryItem.ItemName.Replace("'", "''")}', ItemId = '{trn.TransactionEntryItem.ItemId}'
                                        Where TransactionEntryId = {trn.TransactionEntryId}";
                        }
                    }

                    sql = CleanSql(sql);

                    ctx.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction, sql);
                    foreach (var t in trans.TransactionEntries)
                    {
                        t.TransactionId = trans.TransactionId;
                    }
                    TransactionData = trans;
                    ForceTransactionEntryNumberUpdate(TransactionData);

                    if (trans is Prescription pp)
                            if (pp.PrescriptionImage != null)
                            {
                                SavePrescriptionImageToDb(pp, pp.PrescriptionImage.Path, pp.PrescriptionImage.Image);
                            }

                        return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            catch (DbEntityValidationException vex)
            {
                var str = vex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                    .Aggregate("", (current, er) => current + (er.PropertyName + ","));
                MessageBox.Show("Please Check the following fields before saving! - " + str);
                return false;
            }
            catch (DbUpdateConcurrencyException dce)
            {
                // Get failed entry
                foreach (var itm in dce.Entries)
                {
                    if (itm.State != EntityState.Added)
                    {
                        var dv = itm.GetDatabaseValues();
                        if (dv != null) itm.OriginalValues.SetValues(dv);
                    }
                }
                return false;
            }
            catch (Exception ex1)
            {
                if (!ex1.Message.Contains("Object reference not set to an instance of an object")) throw;
            }

            // trans.TransactionId = trans.TransactionId;

            if (trans != null)
            {
                var dbEntry = ctx.Entry(trans);

                if (dbEntry != null && dbEntry.State != EntityState.Deleted)
                {
                    if (trans.TransactionEntries != null)
                        ForceTransactionEntryNumberUpdate(trans);
                }
            }
            return false;
        }
    }

        public void SavePhoto(dynamic patient , string imageName)
        {
            try
            {
                //Initialize a file stream to read the image file
                FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);

                //Initialize a byte array with size of stream
                byte[] imgByteArr = new byte[fs.Length];

                //Read data from the file stream and put into the byte array
                fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));

                //Close a file stream
                fs.Close();

                using (var ctx = new RMSModel())
                {
                    var sql = "";
                    if (patient.Photo != null)
                    {
                         sql = $"update Photos set Photo = @img where Id = {patient.Id}";
                    }
                    else
                    {
                        sql = $"insert into Photos(id,Photo) values({patient.Id}, @img)";
                    }

                    
                    ctx.Database.ExecuteSqlCommand(sql, new SqlParameter("img", imgByteArr));
                    if (patient.Photo == null) patient.Photo = new Photo();
                    patient.Photo.Image = imgByteArr;


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public void SavePrescriptionPhoto(dynamic prescription, string imageName)
        {
            try
            {
                
                //Initialize a file stream to read the image file
                FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);

                //Initialize a byte array with size of stream
                byte[] imgByteArr = new byte[fs.Length];

                //Read data from the file stream and put into the byte array
                fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));

                //Close a file stream
                fs.Close();
                if (prescription.TransactionId != 0)
                {
                    if (!SaveTransaction()) return;
                    SavePrescriptionImageToDb(prescription, imageName, imgByteArr);
                }

                prescription.PrescriptionImage.Image = imgByteArr;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private static void SavePrescriptionImageToDb(Prescription prescription, string imageName, byte[] imgByteArr)
        {
            using (var ctx = new RMSModel())
            {
                var sql = "";
                if (prescription.PrescriptionImage != null && prescription.PrescriptionImage.TransactionId == prescription.TransactionId)
                {
                    sql = $"update PrescriptionImage set Image = @img where TransactionId = {prescription.TransactionId}";
                }
                else
                {
                    sql =
                        $"insert into PrescriptionImage(TransactionId,Image, Path) values({prescription.TransactionId}, @img, '{imageName}')";
                }


                ctx.Database.ExecuteSqlCommand(sql, new SqlParameter("img", imgByteArr));
                if (prescription.PrescriptionImage == null) prescription.PrescriptionImage = new PrescriptionImage();
            }
        }


        private static string CleanSql(string sql)
    {
        return sql.Replace("= ,", "= NULL,")
            .Replace("= ''", "= NULL")
            .Replace(",,", ",NULL,")
            .Replace(",False,", ",0,")
            .Replace(",True,", ",1,")
            .Replace(",''", ",NULL")
            ;
    }

    private void ForceTransactionEntryNumberUpdate(TransactionBase transactionBase)
    {
        if (transactionBase == null) return;
        foreach (var te in transactionBase.TransactionEntries.OfType<PrescriptionEntry>())
        {
            var t = te.TransactionEntryNumber;
            if (transactionBase is Prescription)
            {
                var r = te.RepeatInfo;
                te.RepeatInfo = "";

            }

            te.TransactionEntryNumber = "";

        }
    }

    private void CleanTransactionNavProperties(TransactionBase titm, RMSModel ctx)
    {
        try
        {
            var itm = titm as Prescription;
            
            if (itm != null)
            {
                var dbEntityEntry = ctx.Entry(itm.Doctor);
                if (dbEntityEntry != null &&
                    (dbEntityEntry.State != EntityState.Unchanged && dbEntityEntry.State != EntityState.Detached))
                {
                    dbEntityEntry.State = EntityState.Unchanged;
                }
                var p = ctx.Entry(itm.Patient);
                if (p != null && (p.State != EntityState.Unchanged && p.State != EntityState.Detached))
                {
                    p.State = EntityState.Unchanged;
                }

            }
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }

    private static Batch batch;

    public Batch Batch
    {
        get { return batch; }
        set
        {
            if (batch != value)
            {
                batch = value;
                NotifyPropertyChanged(x => x.Batch);
            }
        }

    }

    private static Station station;

    public Station Station
    {
        get { return station; }
        set
        {
            if (station != value)
            {
                station = value;
                NotifyPropertyChanged(x => x.Station);
            }
        }

    }

    private static Store store;

    public Store Store
    {
        get { return store; }
        set
        {
            if (store != value)
            {
                store = value;
                NotifyPropertyChanged(x => x.Store);
            }
        }

    }



    internal Prescription NewPrescription()
    {
        try
        {
            var trn = new Prescription()
            {
                StationId = Station.StationId,
                BatchId = Batch.BatchId,
                Time = DateTime.Now,
                CashierId = CurrentLogin.Id,
                StoreCode = Store.StoreCode,
                TrackingState = TrackingState.Added
            };
            trn.StartTracking();
            if (TransactionPharmacist?.Id != CurrentLogin.Id) TransactionPharmacist = null;
            return trn;
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }

    public bool ServerMode { get; set; }



    public void UpdateTransactionEntry(Exception exception, PrescriptionEntry prescriptionEntry)
    {
        var d = TransactionData.TransactionEntries.IndexOf(prescriptionEntry) + 1;
        MessageBox.Show(($"Could Not Print No:{d} Item-'{prescriptionEntry.TransactionEntryItem.ItemName}'"));

    }

    public bool SavePatient(Patient patient)
    {
        var res = false;
        try
        {

            using (var ctx = new RMSModel())
            {
                var t = ctx.Persons.OfType<Patient>().FirstOrDefault(x => x.Id == patient.Id);
                if (t == null)
                {
                    t = new Patient();
                    t.StartTracking();
                    ctx.Persons.Add(t);
                }


                t.Address = patient.Address;
                t.PhoneNumber = patient.PhoneNumber;
                t.Discount = patient.Discount;
                t.FirstName = patient.FirstName;
                t.LastName = patient.LastName;
                t.Sex = patient.Sex;
                t.Allergies = patient.Allergies;
                t.Discount = patient.Discount;
                t.Guardian = patient.Guardian;




                ctx.SaveChanges();
                patient.Id = t.Id;
                patient.AcceptChanges();

                //ctx.Persons.AddOrUpdate(doctor);
                //ctx.SaveChanges();
            }
            res = true;
            return res;
        }
        catch (DbEntityValidationException vex)
        {
            var str = vex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Aggregate("", (current, er) => current + (er.PropertyName + ","));
            MessageBox.Show("Please Check the following fields before saving! - " + str);
            return res;
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }

    public bool SaveDoctor(Doctor doctor)
    {
        var res = false;
        try
        {

            using (var ctx = new RMSModel())
            {
                var t = ctx.Persons.OfType<Doctor>().FirstOrDefault(x => x.Id == doctor.Id);
                if (t == null)
                {
                    t = new Doctor();
                    t.StartTracking();
                    ctx.Persons.Add(t);
                }

                t.Code = doctor.Code;
                t.Address = doctor.Address;
                t.PhoneNumber = doctor.PhoneNumber;
                t.Discount = doctor.Discount;
                t.FirstName = doctor.FirstName;
                t.LastName = doctor.LastName;
                t.Code = doctor.Code;


                ctx.SaveChanges();
                doctor.Id = t.Id;
                doctor.AcceptChanges();
                //ctx.Persons.AddOrUpdate(doctor);
                //ctx.SaveChanges();
            }
            res = true;
            return res;
        }
        catch (DbEntityValidationException vex)
        {
            var str = vex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Aggregate("", (current, er) => current + (er.PropertyName + ","));
            MessageBox.Show("Please Check the following fields before saving! - " + str);
            return res;
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error, GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }
    }

    public bool SaveMedicine(Medicine medicine)
    {

        var res = false;
        try
        {


            using (var ctx = new RMSModel())
            {
                var t = ctx.Item.OfType<Medicine>().FirstOrDefault(x => x.ItemId == medicine.ItemId);
                if (t == null)
                {
                    t = new Medicine() { TrackingState = TrackingState.Added };
                    t.StartTracking();
                    ctx.Item.Add(t);
                }

                t.ItemName = medicine.ItemName;
                t.Description = medicine.Description;
                t.Price = medicine.Price;
                t.SalesTax = medicine.SalesTax;
                t.Quantity = medicine.Quantity;
                t.Inactive = medicine.Inactive;
                t.QBActive = medicine.QBActive;
                t.SuggestedDosage = medicine.SuggestedDosage;

                ctx.SaveChanges();
                medicine.ItemId = t.ItemId;
                medicine.AcceptChanges();
            }

            res = true;
            return res;
        }
        catch (DbEntityValidationException vex)
        {
            var str = vex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                .Aggregate("", (current, er) => current + (er.PropertyName + ","));
            MessageBox.Show("Please Check the following fields before saving! - " + str);
            return res;
        }
        catch (Exception ex)
        {
            Logger.Log(LoggingLevel.Error,
                GetCurrentMethodClass.GetCurrentMethod() + ": --- :" + ex.Message + ex.StackTrace);
            throw ex;
        }

    }

    private static ObservableCollection<RxAbbrevation> _RxAbbrevations = null;
    public ObservableCollection<RxAbbrevation> RxAbbrevations
    {
        get
        {
            if (_RxAbbrevations == null)
            {
                LoadRxAbbrevations();
            }
            return _RxAbbrevations;
        }
    }

    public void LoadRxAbbrevations(string boxText = "")
    {
        ObservableCollection<RxAbbrevation> observableCollection;
        using (var ctx = new RMSModel())
        {
            observableCollection = new ObservableCollection<RxAbbrevation>(ctx.RxAbbrevations.Where(x => x.Shortcut.Contains(boxText)).ToList());
            _RxAbbrevations = observableCollection;
        }
        NotifyPropertyChanged(x => x.RxAbbrevations);
    }

    private static RxAbbrevation _selectedRxAbbrevation = null;
       

        public RxAbbrevation SelectedRxAbbrevation
    {
        get => _selectedRxAbbrevation;
        set => _selectedRxAbbrevation = value;
    }

        public string PrescriptionPhotoFolder { get; set; }

        public void CreateNewTransaction(Prescription prescription)
        {
            var trans = CopyCurrentTransaction(prescription, true);
            SalesVM.Instance.TransactionData = trans;
        }
    }





}


