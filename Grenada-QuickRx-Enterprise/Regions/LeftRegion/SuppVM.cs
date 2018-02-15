using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using RMSDataAccessLayer;
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SalesRegion;
using SimpleMvvmToolkit;


namespace LeftRegion
{
    public class SuppVM : ViewModelBase<SuppVM>
    {
         private static readonly SuppVM _instance;
         static SuppVM()
        {
            _instance = new SuppVM();
            SalesVM.Instance.PropertyChanged += SalesVm_PropertyChanged;
        }

        private static void SalesVm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TransactionData")
            {
                if (SalesVM.Instance.TransactionData is Prescription p)
                {
                    if (p.ParentPrescriptionId > 0 )
                    {
                        //if(Instance.SearchResults.All(x => x.TransactionId != p.ParentPrescriptionId)) SuppVM.Instance.SearchResults = new ObservableCollection<Prescription>(){p.ParentPrescription};
                        //if(SuppVM.Instance?.TransactionData?.TransactionId != p?.ParentPrescription?.TransactionId) SuppVM.Instance.TransactionData = p.ParentPrescription;
                    }
                }
            }
        }

        public static SuppVM Instance
        {
            get { return _instance; }
        }

        string _searchText;
        
        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                try
                {
                    _searchText = value;
                    NotifyPropertyChanged(x => x.SearchText);
                   
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        
        public void AutoRepeat(TransactionBase transactionData)
        {
            SalesVM.Instance.AutoRepeat(transactionData);
        }

        
        public void CopyPrescription()
        {
            var t = SalesVM.Instance.CopyCurrentTransaction(false);
            SalesVM.Instance.TransactionData = t;
            //if (t != null) SalesVM.Instance.GoToTransaction(t.TransactionId);
        }

        public void SearchPrescriptions()
        {
            SearchPrescriptions(SearchText);
        }

        public void SearchPrescriptions(string searchTxt)
        {
            try
            {
               
                    var lst = new ConcurrentQueue<List<SearchView>>();
                   //"m:asprin tabs, p:john doe, d:marryshow"
                    var layers = searchTxt.Split(',');

                    
                if (layers.Any() && searchTxt.Contains(":"))
                    {
                        //cut up and process filter
                        var s = "";
                       
                        foreach (var itm in layers)
                        {
                            if (itm != null)
                            {
                                var slst = itm.Split(':');
                                if (slst.Count() != 2) continue;
                                var s1 = slst[1];
                                if (s1 != null)
                                {
                                    var st = s1.Trim().ToLower();
                                    var s2 = slst[0];
                                    if (s2 != null)
                                        switch (s2.Trim().ToLower())
                                        {
                                            case "m":
                                                //s += string.Format("and ItemInfo like '%{0}%'", slst[1].Trim());
                                                if (lst.Count > 0)
                                                {
                                                    var res = lst.SelectMany(x => x).Where(x => x.ItemInfo.ToLower().Contains(st));
                                                    lst = new ConcurrentQueue<List<SearchView>>();
                                                    lst.Enqueue(res.ToList());
                                                }
                                                else
                                                {
                                                    using (var ctx = new RMSModel())
                                                    {
                                                        lst.Enqueue(
                                                            ctx.SearchViews.Where(x => x.ItemInfo.ToLower().Contains(st))
                                                                .Include(x => x.Prescription)
                                                                .Include(x => x.Prescription.Prescriptions)
                                                                .Include("Prescription.ParentPrescription.Prescriptions.TransactionEntries.TransactionEntryItem")
                                                                .Include("Prescription.Prescriptions.TransactionEntries.TransactionEntryItem")
                                                                .Include(x => x.Prescription.TransactionEntries)
                                                                .Include("Prescription.TransactionEntries.TransactionEntryItem")
                                                                .Include(x => x.Prescription.Patient)
                                                                .Include(x => x.Prescription.Doctor)
                                                                .OrderByDescending(x => x.TransactionId)
                                                                
                                                                .Take(listCount).ToList());
                                                    }
                                                }

                                                break;
                                            case "p":
                                                //s += string.Format("and PatientInfo like '%{0}%'", slst[1].Trim());
                                                if (lst.Count > 0)
                                                {
                                                    var res = lst.SelectMany(x => x).Where(x => x != null && x.PatientInfo != null && (x.PatientInfo.ToLower().Contains(st)));
                                                    lst = new ConcurrentQueue<List<SearchView>>();
                                                    lst.Enqueue(res.ToList());
                                                }
                                                else
                                                {
                                                    using (var ctx = new RMSModel())
                                                    {
                                                        lst.Enqueue(
                                                            ctx.SearchViews.Where(x => x.PatientInfo.ToLower().Contains(st))
                                                                .Include(x => x.Prescription)
                                                                .Include(x => x.Prescription.Prescriptions)
                                                                .Include("Prescription.ParentPrescription.Prescriptions.TransactionEntries.TransactionEntryItem")
                                                                .Include("Prescription.Prescriptions.TransactionEntries.TransactionEntryItem")
                                                                .Include(x => x.Prescription.TransactionEntries)
                                                                .Include("Prescription.TransactionEntries.TransactionEntryItem")
                                                                .Include(x => x.Prescription.Patient)
                                                                .Include(x => x.Prescription.Doctor)
                                                                .OrderByDescending(x => x.TransactionId)
                                                                
                                                                .Take(listCount).ToList());
                                                    }
                                                }
                                                break;
                                            case "d":
                                                //s += string.Format("and DoctorInfo like '%{0}%'", slst[1].Trim());
                                                if (lst.Count > 0)
                                                {
                                                    var res = lst.SelectMany(x => x).Where(x => x != null && x.DoctorInfo != null && ( x.DoctorInfo.ToLower().Contains(st)));
                                                    lst = new ConcurrentQueue<List<SearchView>>();
                                                    lst.Enqueue(res.ToList());
                                                }
                                                else
                                                {
                                                    using (var ctx = new RMSModel())
                                                    {
                                                        lst.Enqueue(
                                                            ctx.SearchViews.Where(x => x.DoctorInfo.ToLower().Contains(st))
                                                                .Include(x => x.Prescription)
                                                                .Include(x => x.Prescription.Prescriptions)
                                                                .Include("Prescription.ParentPrescription.Prescriptions.TransactionEntries.TransactionEntryItem")
                                                                .Include("Prescription.Prescriptions.TransactionEntries.TransactionEntryItem")
                                                                .Include(x => x.Prescription.TransactionEntries)
                                                                .Include("Prescription.TransactionEntries.TransactionEntryItem")
                                                                .Include(x => x.Prescription.Patient)
                                                                .Include(x => x.Prescription.Doctor)
                                                                .OrderByDescending(x => x.TransactionId)
                                                                
                                                                .Take(listCount).ToList());
                                                    }
                                                }
                                                break;
                                        }
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(searchTxt))
                        {
                            // do basic search
                            using (var ctx = new RMSModel())
                            {
                                ctx.Database.CommandTimeout = 0;
                                lst.Enqueue(
                                    ctx.SearchViews.Where(x => x.SearchInfo.Contains(searchTxt) )
                                       .Include(x => x.Prescription)
                                        .Include(x => x.Prescription.Prescriptions)
                                        .Include("Prescription.ParentPrescription.Prescriptions.TransactionEntries.TransactionEntryItem")
                                        .Include("Prescription.Prescriptions.TransactionEntries.TransactionEntryItem")
                                        .Include(x => x.Prescription.TransactionEntries)
                                       .Include("Prescription.TransactionEntries.TransactionEntryItem")
                                        .Include(x => x.Prescription.Patient)
                                        .Include(x => x.Prescription.Doctor)
                                        .OrderByDescending(x => x.TransactionId)
                                        .Where(x => x.Prescription.ParentPrescriptionId == null)
                                        .Take(listCount).ToList());

                                
                        }
                        }
                        else
                        {
                            using (var ctx = new RMSModel())
                            {
                                lst.Enqueue(
                                    ctx.SearchViews
                                        .Include(x => x.Prescription)
                                        .Include(x => x.Prescription.Prescriptions)
                                        .Include("Prescription.ParentPrescription.Prescriptions.TransactionEntries.TransactionEntryItem")
                                        .Include("Prescription.Prescriptions.TransactionEntries.TransactionEntryItem")
                                        .Include(x => x.Prescription.TransactionEntries)
                                      .Include("Prescription.TransactionEntries.TransactionEntryItem")
                                        .Include(x => x.Prescription.Patient)
                                        .Include(x => x.Prescription.Doctor)
                                        .OrderByDescending(x => x.TransactionId)
                                        .Where(x => x.Prescription.ParentPrescriptionId == null)
                                        .Take(25).ToList());
                            }
                        }
                    }


                    SearchResults = new ObservableCollection<Prescription>(lst.SelectMany(x => x).Where((x => x?.Prescription != null)).Select(z => z.Prescription).OrderByDescending(x => x.Time));
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static ObservableCollection<Prescription> _searchResults = new ObservableCollection<Prescription>();
        
        public ObservableCollection<Prescription> SearchResults
        {
            get
            {
                return _searchResults;
            }
            set
            {
                _searchResults = value;
                NotifyPropertyChanged(x => x.SearchResults);
            }
        }

        //+ ToDo: Replace this with your own data fields
        private RMSDataAccessLayer.TransactionBase transactionData;
        private int listCount = 20;
        
        public RMSDataAccessLayer.TransactionBase TransactionData
        {
            get { return transactionData; }
            set
            {
                try
                {
                    if (!object.Equals(transactionData, value))
                    {
                        transactionData = value;
                        if (value != null)
                        {
                            if (SalesVM.Instance.TransactionData is Prescription trans)
                            {
                                //if (trans.TransactionId != transactionData.TransactionId && trans.Prescriptions.All(x => x.TransactionId != transactionData.TransactionId))
                                    SalesVM.Instance.GoToTransaction(transactionData.TransactionId);
                            }
                            else
                            {
                                SalesVM.Instance.GoToTransaction(transactionData.TransactionId);
                            }
                            
                        }

                        //if(this.regionManager.Regions["HeaderRegion"] != null) this.regionManager.Regions["HeaderRegion"].Context = transactionData;
                       //if(this.regionManager.Regions["CenterRegion"] != null) this.regionManager.Regions["CenterRegion"].Context = transactionData;
                        NotifyPropertyChanged(x => x.TransactionData);

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
