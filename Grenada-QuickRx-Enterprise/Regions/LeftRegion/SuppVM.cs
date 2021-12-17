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
using System.Data.Entity.SqlServer;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism;
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
                    if (p.ParentTransactionId > 0)
                    {
                        //if(Instance.SearchResults.All(x => x.TransactionId != p.ParentTransactionId)) SuppVM.Instance.SearchResults = new ObservableCollection<Prescription>(){p.ParentTransaction};
                        //if(SuppVM.Instance?.TransactionData?.TransactionId != p?.ParentTransaction?.TransactionId) SuppVM.Instance.TransactionData = p.ParentTransaction;
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

        public void SearchTransactions()
        {
            SearchTransactions(SearchText);
        }

        public void SearchTransactions(string searchTxt)
        {
            try
            {

                SearchResults = new ObservableCollection<Prescription>(GetTransactions(searchTxt));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Prescription> GetTransactions(string searchTxt)
        {
           
             

                var plist = new List<Prescription>();
                var plistTask = Task.Run(() =>
                {
                    using (var ctx = new RMSModel())
                    {
                        IQueryable<Prescription> Transactions;
                        if (Int32.TryParse(searchTxt, out int num))
                        {
                            Transactions = ctx.TransactionBase.OfType<Prescription>().AsNoTracking()
                                .Where(x => x.TransactionId.ToString().Contains(searchTxt));

                        }
                        else
                        {
                            Transactions = ctx.TransactionBase.OfType<Prescription>().AsNoTracking()
                                .Where(x => (x.Patient.FirstName + " " + x.Patient.LastName).Contains(searchTxt));
                        }

                        var lst = Transactions.OfType<Prescription>()
                            .OrderByDescending(x => x.TransactionId)
                            .Where(x => x.ParentTransactionId == null)
                            .OrderByDescending(x => x.Time)
                            .Select(x => new
                            {
                                TransactionId = x.TransactionId,
                                Time = x.Time,

                                Patient = new
                                {
                                    FirstName = x.Patient.FirstName,
                                    LastName = x.Patient.LastName,
                                },
                                Doctor = new
                                {
                                    FirstName = x.Doctor.FirstName,
                                    LastName = x.Doctor.LastName,
                                },
                                TransactionEntries = x.TransactionEntries
                                    .Select(z => new
                                    {
                                        Quantity = z.Quantity,
                                        Price = z.Price,
                                        SalesTaxPercent = z.SalesTaxPercent,
                                        Discount = z.Discount,
                                        TransactionEntryItem = new
                                        {
                                            ItemName = z.TransactionEntryItem.ItemName
                                        }
                                    }).ToList(),
                                Transactions = x.Transactions.OfType<Prescription>().Select(pp => new
                                {
                                    TransactionId = pp.TransactionId,
                                    Time = pp.Time,

                                    Patient = new
                                    {
                                        FirstName = pp.Patient.FirstName,
                                        LastName = pp.Patient.LastName,
                                    },
                                    Doctor = new
                                    {
                                        FirstName = pp.Doctor.FirstName,
                                        LastName = pp.Doctor.LastName,
                                    },
                                    TransactionEntries = pp.TransactionEntries
                                        .Select(zz => new
                                        {
                                            Quantity = zz.Quantity,
                                            Price = zz.Price,
                                            SalesTaxPercent = zz.SalesTaxPercent,
                                            Discount = zz.Discount,
                                            TransactionEntryItem = new
                                            {
                                                ItemName = zz.TransactionEntryItem.ItemName
                                            }
                                        }),
                                })

                            })
                            .Take(listCount)
                            .ToList();

                        plist = lst
                            .Select(x => new Prescription()
                            {
                                TransactionId = x.TransactionId,
                                Time = x.Time,
                                Patient = new Patient()
                                {
                                    FirstName = x.Patient.FirstName,
                                    LastName = x.Patient.LastName,
                                },
                                Doctor = new Doctor()
                                {
                                    FirstName = x.Doctor.FirstName,
                                    LastName = x.Doctor.LastName,
                                },
                                TransactionEntries = new ObservableCollection<TransactionEntryBase>(
                                    x.TransactionEntries
                                        .Select(z => new PrescriptionEntry()
                                        {
                                            Quantity = z.Quantity,
                                            Price = z.Price,
                                            SalesTaxPercent = z.SalesTaxPercent,
                                            Discount = z.Discount,
                                            TransactionEntryItem = new TransactionEntryItem()
                                            {
                                                ItemName = z.TransactionEntryItem.ItemName
                                            }
                                        })),
                                Transactions = new ObservableCollection<TransactionBase>(x.Transactions.Select(pp =>
                                    new Prescription()
                                    {
                                        TransactionId = pp.TransactionId,
                                        Time = pp.Time,

                                        Patient = new Patient()
                                        {
                                            FirstName = pp.Patient.FirstName,
                                            LastName = pp.Patient.LastName,
                                        },
                                        Doctor = new Doctor()
                                        {
                                            FirstName = pp.Doctor.FirstName,
                                            LastName = pp.Doctor.LastName,
                                        },
                                        TransactionEntries = new ObservableCollection<TransactionEntryBase>(
                                            pp.TransactionEntries
                                                .Select(q => new PrescriptionEntry()
                                                {
                                                    Quantity = q.Quantity,
                                                    Price = q.Price,
                                                    SalesTaxPercent = q.SalesTaxPercent,
                                                    Discount = q.Discount,
                                                    TransactionEntryItem = new TransactionEntryItem()
                                                    {
                                                        ItemName = q.TransactionEntryItem.ItemName
                                                    }
                                                })),

                                    }).ToList())

                            }).ToList();
                    }
                });
                var qlist = new List<Prescription>();
                var qlistTask = Task.Run(() =>
                {
                    using (var ctx = new RMSModel())
                    {

                        IQueryable<QuickPrescription> qTransactions;
                        if (Int32.TryParse(searchTxt, out int num))
                        {

                            qTransactions = ctx.TransactionBase.OfType<QuickPrescription>().AsNoTracking()
                                .Where(x => x.TransactionId.ToString().Contains(searchTxt));
                        }
                        else
                        {

                            qTransactions = ctx.TransactionBase.OfType<QuickPrescription>().AsNoTracking()
                                .Where(x => x.Patient != null &&
                                            (x.Patient.FirstName + " " + x.Patient.LastName).Contains(searchTxt));
                        }


                        var lst = qTransactions.OfType<QuickPrescription>()
                            .OrderByDescending(x => x.TransactionId)
                            .Where(x => x.ParentTransactionId == null)
                            .OrderByDescending(x => x.Time)
                            .Select(x => new
                            {
                                TransactionId = x.TransactionId,
                                Time = x.Time,

                                Patient = new
                                {
                                    FirstName = x.Patient.FirstName,
                                    LastName = x.Patient.LastName,
                                },

                                TransactionEntries = x.TransactionEntries
                                    .Select(z => new
                                    {
                                        Quantity = z.Quantity,
                                        Price = z.Price,
                                        SalesTaxPercent = z.SalesTaxPercent,
                                        Discount = z.Discount,
                                        TransactionEntryItem = new
                                        {
                                            ItemName = z.TransactionEntryItem.ItemName
                                        }
                                    }).ToList(),
                                Transactions = x.Transactions.OfType<QuickPrescription>().Select(pp => new
                                {
                                    TransactionId = pp.TransactionId,
                                    Time = pp.Time,

                                    Patient = new
                                    {
                                        FirstName = pp.Patient.FirstName,
                                        LastName = pp.Patient.LastName,
                                    },

                                    TransactionEntries = pp.TransactionEntries
                                        .Select(zz => new
                                        {
                                            Quantity = zz.Quantity,
                                            Price = zz.Price,
                                            SalesTaxPercent = zz.SalesTaxPercent,
                                            Discount = zz.Discount,
                                            TransactionEntryItem = new
                                            {
                                                ItemName = zz.TransactionEntryItem.ItemName
                                            }
                                        }),
                                })
                            })
                            .Take(listCount)
                            .ToList();

                        qlist = lst
                            .Select(x => new Prescription()
                            {
                                TransactionId = x.TransactionId,
                                Time = x.Time,
                                Patient = new Patient()
                                {
                                    FirstName = x.Patient.FirstName,
                                    LastName = x.Patient.LastName,
                                },

                                TransactionEntries = new ObservableCollection<TransactionEntryBase>(
                                    x.TransactionEntries
                                        .Select(z => new PrescriptionEntry()
                                        {
                                            Quantity = z.Quantity,
                                            Price = z.Price,
                                            SalesTaxPercent = z.SalesTaxPercent,
                                            Discount = z.Discount,
                                            TransactionEntryItem = new TransactionEntryItem()
                                            {
                                                ItemName = z.TransactionEntryItem.ItemName
                                            }
                                        })),
                                Transactions = new ObservableCollection<TransactionBase>(x.Transactions.Select(pp =>
                                    new Prescription()
                                    {
                                        TransactionId = pp.TransactionId,
                                        Time = pp.Time,

                                        Patient = new Patient()
                                        {
                                            FirstName = pp.Patient.FirstName,
                                            LastName = pp.Patient.LastName,
                                        },
                                        //Doctor = new Doctor()
                                        //{
                                        //    FirstName = pp.Doctor.FirstName,
                                        //    LastName = pp.Doctor.LastName,
                                        //},
                                        TransactionEntries = new ObservableCollection<TransactionEntryBase>(
                                            pp.TransactionEntries
                                                .Select(q => new PrescriptionEntry()
                                                {
                                                    Quantity = q.Quantity,
                                                    Price = q.Price,
                                                    SalesTaxPercent = q.SalesTaxPercent,
                                                    Discount = q.Discount,
                                                    TransactionEntryItem = new TransactionEntryItem()
                                                    {
                                                        ItemName = q.TransactionEntryItem.ItemName
                                                    }
                                                })),

                                    }).ToList())

                            }).ToList();
                    }
                });

                Task.WhenAll(plistTask, qlistTask).Wait();


                var prescriptions = plist;
                prescriptions.AddRange(qlist);
                return prescriptions.OrderByDescending(x => x.TransactionId).ToList();
            
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

        private Patient _currentPatient;

        public Patient CurrentPatient
        {
            get { return _currentPatient; }
            set
            {
                _currentPatient = value;
                if (_currentPatient != null)
                {
                    //  _currentPatient.Prescription = new ObservableCollection<Prescription>();
                    _currentPatient.StartTracking();
                }


                NotifyPropertyChanged(x => x.CurrentPatient);
            }
        }

        private Doctor _currentDoctor;

        public Doctor CurrentDoctor
        {
            get { return _currentDoctor; }
            set
            {
                _currentDoctor = value;
                // _currentDoctor.Patients = new List<Patient>();

                _currentDoctor?.StartTracking();
                _currentDoctor?.ChangeTracker.ExcludedProperties.Add("Patients");
                NotifyPropertyChanged(x => x.CurrentDoctor);
            }
        }

        private Medicine _currentDrug;

        public Medicine CurrentDrug
        {
            get { return _currentDrug; }
            set
            {
                _currentDrug = value;
                _currentDrug?.StartTracking();
                NotifyPropertyChanged(x => x.CurrentDrug);
            }
        }

        //+ ToDo: Replace this with your own data fields
        private RMSDataAccessLayer.TransactionBase transactionData;
        private int listCount = 10;

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
                                //if (trans.TransactionId != transactionData.TransactionId && trans.Transactions.All(x => x.TransactionId != transactionData.TransactionId))
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

        private ObservableCollection<Patient> _patientSearchResults;

        public ObservableCollection<Patient> PatientSearchResults
        {
            get { return _patientSearchResults; }
            set
            {
                _patientSearchResults = value;
                NotifyPropertyChanged(x => x.PatientSearchResults);
            }
        }

        public string PatientSearchText { get; set; }
        private ObservableCollection<Doctor> _doctorSearchResults;

        public ObservableCollection<Doctor> DoctorSearchResults
        {
            get { return _doctorSearchResults; }
            set
            {
                _doctorSearchResults = value;
                NotifyPropertyChanged(x => x.DoctorSearchResults);
            }
        }

        public string DoctorSearchText { get; set; }
        private ObservableCollection<Medicine> _drugSearchResults;

        public ObservableCollection<Medicine> DrugSearchResults
        {
            get { return _drugSearchResults; }
            set
            {
                _drugSearchResults = value;
                NotifyPropertyChanged(x => x.DrugSearchResults);
            }
        }

        public string DrugSearchText { get; set; }

        public void SearchPatients()
        {
            SearchPatients(PatientSearchText);
        }

        private void SearchPatients(string patientSearchText)
        {
            try
            {

                PatientSearchResults = new ObservableCollection<Patient>(GetPatients(patientSearchText));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Patient> GetPatients(string patientSearchText)
        {
            using (var ctx = new RMSModel())
            {
                IQueryable<Prescription> Transactions;

                Transactions = ctx.TransactionBase.OfType<Prescription>().AsNoTracking()
                    .Where(x => (x.Patient.FirstName + " " + x.Patient.LastName).Contains(patientSearchText) ||
                                x.Patient.PhoneNumber.Contains(patientSearchText));




                var list = Transactions.OfType<Prescription>()
                    .OrderByDescending(x => x.TransactionId)
                    .Where(x => x.ParentTransactionId == null)
                    .OrderByDescending(x => x.Time)
                    .Select(x => new
                    {
                        TransactionId = x.TransactionId,
                        Time = x.Time,

                        Patient = new
                        {
                            FirstName = x.Patient.FirstName,
                            LastName = x.Patient.LastName,
                            Address = x.Patient.Address,
                            PhoneNumber = x.Patient.PhoneNumber,
                            x.Patient.Id,
                            x.Patient.Guardian,
                            x.Patient.Allergies,
                            x.Patient.Sex,
                            x.Patient.Discount

                        },
                        Doctor = new
                        {
                            FirstName = x.Doctor.FirstName,
                            LastName = x.Doctor.LastName,
                        },
                        TransactionEntries = x.TransactionEntries
                            .Select(z => new
                            {
                                Quantity = z.Quantity,
                                Price = z.Price,
                                SalesTaxPercent = z.SalesTaxPercent,
                                Discount = z.Discount,
                                TransactionEntryItem = new
                                {
                                    ItemName = z.TransactionEntryItem.ItemName
                                }
                            }).ToList(),
                        Transactions = x.Transactions.OfType<Prescription>().Select(pp => new
                        {
                            TransactionId = pp.TransactionId,
                            Time = pp.Time,
                            Patient = new
                            {
                                FirstName = pp.Patient.FirstName,
                                LastName = pp.Patient.LastName,
                                Address = pp.Patient.Address,
                                PhoneNumber = pp.Patient.PhoneNumber,
                                pp.Patient.Id,
                                pp.Patient.Guardian,
                                pp.Patient.Allergies,
                                pp.Patient.Sex,
                                pp.Patient.Discount
                            },
                            Doctor = new
                            {
                                FirstName = pp.Doctor.FirstName,
                                LastName = pp.Doctor.LastName,
                            },
                            TransactionEntries = pp.TransactionEntries
                                .Select(zz => new
                                {
                                    Quantity = zz.Quantity,
                                    Price = zz.Price,
                                    SalesTaxPercent = zz.SalesTaxPercent,
                                    Discount = zz.Discount,
                                    TransactionEntryItem = new
                                    {
                                        ItemName = zz.TransactionEntryItem.ItemName
                                    }
                                }),
                        })

                    })
                    .Take(listCount)
                    .ToList();
                var patients = list
                    .GroupBy(x => x.Patient)
                    .Select(x => new Patient()
                    {
                        FirstName = x.Key.FirstName,
                        LastName = x.Key.LastName,
                        Guardian = x.Key.Guardian,
                        Address = x.Key.Address,
                        Allergies = x.Key.Allergies,
                        PhoneNumber = x.Key.PhoneNumber,
                        Sex = x.Key.Sex,
                        Id = x.Key.Id,
                        Discount = x.Key.Discount,
                        Prescription = new ObservableCollection<Prescription>(
                            x.Select(p => new Prescription()
                            {
                                TransactionId = p.TransactionId,
                                Time = p.Time,
                                Patient = new Patient()
                                {
                                    FirstName = p.Patient.FirstName,
                                    LastName = p.Patient.LastName,
                                },
                                Doctor = new Doctor()
                                {
                                    FirstName = p.Doctor.FirstName,
                                    LastName = p.Doctor.LastName,
                                },
                                TransactionEntries = new ObservableCollection<TransactionEntryBase>(
                                    p.TransactionEntries
                                        .Select(z => new PrescriptionEntry()
                                        {
                                            Quantity = z.Quantity,
                                            Price = z.Price,
                                            SalesTaxPercent = z.SalesTaxPercent,
                                            Discount = z.Discount,
                                            TransactionEntryItem = new TransactionEntryItem()
                                            {
                                                ItemName = z.TransactionEntryItem.ItemName
                                            }
                                        }).ToList()),
                                Transactions = new ObservableCollection<TransactionBase>(p.Transactions.Select(pp =>
                                    new Prescription()
                                    {
                                        TransactionId = pp.TransactionId,
                                        Time = pp.Time,

                                        Patient = new Patient()
                                        {
                                            FirstName = pp.Patient.FirstName,
                                            LastName = pp.Patient.LastName,
                                            Guardian = pp.Patient.Guardian,
                                            Address = pp.Patient.Address,
                                            Allergies = pp.Patient.Allergies,
                                            PhoneNumber = pp.Patient.PhoneNumber,
                                            Sex = pp.Patient.Sex,
                                            Id = pp.Patient.Id

                                        },
                                        Doctor = new Doctor()
                                        {
                                            FirstName = pp.Doctor.FirstName,
                                            LastName = pp.Doctor.LastName,
                                        },
                                        TransactionEntries = new ObservableCollection<TransactionEntryBase>(
                                            pp.TransactionEntries
                                                .Select(q => new PrescriptionEntry()
                                                {
                                                    Quantity = q.Quantity,
                                                    Price = q.Price,
                                                    SalesTaxPercent = q.SalesTaxPercent,
                                                    Discount = q.Discount,
                                                    TransactionEntryItem = new TransactionEntryItem()
                                                    {
                                                        ItemName = q.TransactionEntryItem.ItemName
                                                    }
                                                })),

                                    }).ToList())

                            }))
                    }).ToList();

                return patients;
            }

        }

        public void SearchDoctors()
        {
            SearchDoctors(DoctorSearchText);
        }

        private void SearchDoctors(string doctorSearchText)
        {
            try
            {

                DoctorSearchResults = new ObservableCollection<Doctor>(GetDoctors(doctorSearchText));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Doctor> GetDoctors(string doctorSearchText)
        {
            using (var ctx = new RMSModel())
            {
                IQueryable<Prescription> Transactions;

                Transactions = ctx.TransactionBase.OfType<Prescription>().AsNoTracking()
                    .Where(x => (x.Doctor.FirstName + " " + x.Doctor.LastName).Contains(doctorSearchText) ||
                                x.Doctor.Code.Contains(doctorSearchText) ||
                                x.Doctor.PhoneNumber.Contains(doctorSearchText));




                var list = Transactions.OfType<Prescription>()
                    .OrderByDescending(x => x.TransactionId)
                    .Where(x => x.ParentTransactionId == null)
                    .OrderByDescending(x => x.Time)
                    .Select(x => new
                    {
                        TransactionId = x.TransactionId,
                        Time = x.Time,

                        Patient = new
                        {
                            x.Patient.Id,
                            FirstName = x.Patient.FirstName,
                            LastName = x.Patient.LastName,
                            Address = x.Patient.Address,
                            PhoneNumber = x.Patient.PhoneNumber,
                        },
                        Doctor = new
                        {
                            FirstName = x.Doctor.FirstName,
                            LastName = x.Doctor.LastName,
                            Address = x.Doctor.Address,
                            PhoneNumber = x.Doctor.PhoneNumber,
                            x.Doctor.Code,
                            x.Doctor.Id
                        },
                        TransactionEntries = x.TransactionEntries
                            .Select(z => new
                            {
                                Quantity = z.Quantity,
                                Price = z.Price,
                                SalesTaxPercent = z.SalesTaxPercent,
                                Discount = z.Discount,
                                TransactionEntryItem = new
                                {
                                    ItemName = z.TransactionEntryItem.ItemName
                                }
                            }).ToList(),
                        Transactions = x.Transactions.OfType<Prescription>().Select(pp => new
                        {
                            TransactionId = pp.TransactionId,
                            Time = pp.Time,
                            Patient = new
                            {
                                FirstName = pp.Patient.FirstName,
                                LastName = pp.Patient.LastName,
                                pp.Patient.Id,
                                Address = pp.Patient.Address,
                                PhoneNumber = pp.Patient.PhoneNumber,


                            },
                            Doctor = new
                            {
                                FirstName = pp.Doctor.FirstName,
                                LastName = pp.Doctor.LastName,
                                Address = pp.Doctor.Address,
                                PhoneNumber = pp.Doctor.PhoneNumber,
                                Code = pp.Doctor.Code,
                                Discount = pp.Doctor.Discount,
                                Id = pp.Doctor.Id
                            },
                            TransactionEntries = pp.TransactionEntries
                                .Select(zz => new
                                {
                                    Quantity = zz.Quantity,
                                    Price = zz.Price,
                                    SalesTaxPercent = zz.SalesTaxPercent,
                                    Discount = zz.Discount,
                                    TransactionEntryItem = new
                                    {
                                        ItemName = zz.TransactionEntryItem.ItemName
                                    }
                                }),
                        })

                    })
                    .Take(listCount)
                    .ToList();
                var doctors = list
                    .GroupBy(x => x.Doctor)
                    .Select(x => new Doctor()
                    {
                        FirstName = x.Key.FirstName,
                        LastName = x.Key.LastName,
                        Code = x.Key.Code,
                        Id = x.Key.Id,
                        Address = x.Key.Address,
                        PhoneNumber = x.Key.PhoneNumber,
                        Patients = x.GroupBy(t => t.Patient)
                            .Select(patient => new Patient()
                            {
                                Id = patient.Key.Id,
                                FirstName = patient.Key.FirstName,
                                LastName = patient.Key.LastName,
                                Address = patient.Key.Address,
                                PhoneNumber = patient.Key.PhoneNumber,

                                Prescription = new ObservableCollection<Prescription>(
                                    patient.Select(p => new Prescription()
                                    {
                                        TransactionId = p.TransactionId,
                                        Time = p.Time,
                                        Patient = new Patient()
                                        {
                                            FirstName = p.Patient.FirstName,
                                            LastName = p.Patient.FirstName,
                                        },

                                        Doctor = new Doctor()
                                        {
                                            FirstName = p.Doctor.FirstName,
                                            LastName = p.Doctor.LastName,
                                            Id = p.Doctor.Id
                                        },
                                        TransactionEntries = new ObservableCollection<TransactionEntryBase>(
                                            p.TransactionEntries
                                                .Select(z => new PrescriptionEntry()
                                                {
                                                    Quantity = z.Quantity,
                                                    Price = z.Price,
                                                    SalesTaxPercent = z.SalesTaxPercent,
                                                    Discount = z.Discount,
                                                    TransactionEntryItem = new TransactionEntryItem()
                                                    {
                                                        ItemName = z.TransactionEntryItem.ItemName
                                                    }
                                                }).ToList()),
                                        Transactions = new ObservableCollection<TransactionBase>(p.Transactions
                                            .Select(pp =>
                                                new Prescription()
                                                {
                                                    TransactionId = pp.TransactionId,
                                                    Time = pp.Time,

                                                    Patient = new Patient()
                                                    {

                                                        FirstName = pp.Patient.FirstName,
                                                        LastName = pp.Patient.LastName,
                                                        Id = pp.Patient.Id,

                                                    },
                                                    Doctor = new Doctor()
                                                    {
                                                        FirstName = pp.Doctor.FirstName,
                                                        LastName = pp.Doctor.LastName,
                                                        Id = pp.Doctor.Id,
                                                    },
                                                    TransactionEntries = new ObservableCollection<TransactionEntryBase>(
                                                        pp.TransactionEntries
                                                            .Select(q => new PrescriptionEntry()
                                                            {
                                                                Quantity = q.Quantity,
                                                                Price = q.Price,
                                                                SalesTaxPercent = q.SalesTaxPercent,
                                                                Discount = q.Discount,
                                                                TransactionEntryItem = new TransactionEntryItem()
                                                                {
                                                                    ItemName = q.TransactionEntryItem.ItemName
                                                                }
                                                            })),

                                                }).ToList())

                                    }))
                            }).ToList()

                    }).ToList();

                return doctors;
            }
        }

        public void SearchDrugs()
        {
            DrugSearchResults = new ObservableCollection<Medicine>(SearchDrugs(DrugSearchText));
        }

        public List<Medicine> SearchDrugs(string drugSearchText)
        {
            using (var ctx = new RMSModel())
            {
                IQueryable<Prescription> Transactions;
                if (string.IsNullOrEmpty(drugSearchText))
                {
                    Transactions = ctx.TransactionBase.OfType<Prescription>().AsNoTracking().Take(listCount);
                }
                else
                {
                    Transactions = ctx.TransactionBase.OfType<Prescription>().AsNoTracking()
                        .Where(x => x.TransactionEntries.Any(z =>
                            (z.TransactionEntryItem.ItemName + "|" + z.TransactionEntryItem.ItemNumber).Contains(
                                drugSearchText)));
                }






                var list = Transactions.OfType<Prescription>()
                    .OrderByDescending(x => x.TransactionId)
                    .Where(x => x.ParentTransactionId == null)
                    .OrderByDescending(x => x.Time)
                    .Select(x => new
                    {
                        TransactionId = x.TransactionId,
                        Time = x.Time,

                        Patient = new
                        {
                            FirstName = x.Patient.FirstName,
                            LastName = x.Patient.LastName,
                            Address = x.Patient.Address,
                            PhoneNumber = x.Patient.PhoneNumber
                        },
                        Doctor = new
                        {
                            FirstName = x.Doctor.FirstName,
                            LastName = x.Doctor.LastName,
                            Address = x.Doctor.Address,
                            PhoneNumber = x.Doctor.PhoneNumber,
                            Code = x.Doctor.Code,
                        },
                        TransactionEntries = x.TransactionEntries
                            .Select(z => new
                            {
                                Quantity = z.Quantity,
                                Price = z.Price,
                                SalesTaxPercent = z.SalesTaxPercent,
                                Discount = z.Discount,
                                ItemName = z.TransactionEntryItem.ItemName,
                                TransactionEntryItem = new
                                {
                                    ItemName = z.TransactionEntryItem.ItemName,
                                    z.TransactionEntryItem.ItemNumber,

                                },
                                InventoryItem = new
                                {
                                    ItemId = z.TransactionEntryItem.Item.ItemId,
                                    ItemName = z.TransactionEntryItem.Item.ItemName,
                                    Description = z.TransactionEntryItem.Item.Description,
                                    // SuggestedDosage = z.TransactionEntryItem.Item,
                                    Price = z.TransactionEntryItem.Item.Price,
                                    Quantity = z.TransactionEntryItem.Item.Quantity,
                                    QBActive = z.TransactionEntryItem.Item.QBActive,
                                    Inactive = z.TransactionEntryItem.Item.Inactive,
                                    SalesTax = z.TransactionEntryItem.Item.SalesTax,

                                }
                            }).ToList(),
                        Transactions = x.Transactions.OfType<Prescription>().Select(pp => new
                        {
                            TransactionId = pp.TransactionId,
                            Time = pp.Time,
                            Patient = new
                            {
                                FirstName = pp.Patient.FirstName,
                                LastName = pp.Patient.LastName,
                                Address = pp.Patient.Address,
                                PhoneNumber = pp.Patient.PhoneNumber

                            },
                            Doctor = new
                            {
                                FirstName = pp.Doctor.FirstName,
                                LastName = pp.Doctor.LastName,
                                Address = pp.Doctor.Address,
                                PhoneNumber = pp.Doctor.PhoneNumber,
                                Code = pp.Doctor.Code,
                            },
                            TransactionEntries = pp.TransactionEntries
                                .Select(zz => new
                                {
                                    Quantity = zz.Quantity,
                                    Price = zz.Price,
                                    SalesTaxPercent = zz.SalesTaxPercent,
                                    Discount = zz.Discount,

                                    TransactionEntryItem = new
                                    {
                                        zz.TransactionEntryItem.ItemName,
                                        zz.TransactionEntryItem.ItemNumber
                                    }
                                }),
                        })

                    })
                    .Take(listCount)
                    .ToList();
                var drugs = list
                    .GroupBy(x =>
                        x.TransactionEntries.Select(xx => xx.InventoryItem).FirstOrDefault());

                var res = drugs.Where(x => x.Key != null).Select(x => new Medicine()
                {
                    ItemName = x.Key.ItemName,
                    Description = x.Key.Description,
                    ItemId = x.Key.ItemId,
                    // SuggestedDosage = x.Key.SuggestedDosage,
                    Price = x.Key.Price,
                    Quantity = x.Key.Quantity,
                    SalesTax = x.Key.SalesTax,
                    QBActive = x.Key.QBActive,
                    Inactive = x.Key.Inactive,
                    Patients = x.GroupBy(t => t.Patient)
                        .Select(patient => new Patient()
                        {
                            FirstName = patient.Key.FirstName,
                            LastName = patient.Key.LastName,
                            Address = patient.Key.Address,
                            PhoneNumber = patient.Key.PhoneNumber,
                            Prescription = new ObservableCollection<Prescription>(
                                patient.Select(p => new Prescription()
                                {
                                    TransactionId = p.TransactionId,
                                    Time = p.Time,
                                    Patient = new Patient()
                                    {
                                        FirstName = p.Patient.FirstName,
                                        LastName = p.Patient.LastName,
                                        Address = p.Patient.Address,
                                        PhoneNumber = p.Patient.PhoneNumber,
                                    },
                                    Doctor = new Doctor()
                                    {
                                        FirstName = p.Doctor.FirstName,
                                        LastName = p.Doctor.LastName,
                                    },
                                    TransactionEntries = new ObservableCollection<TransactionEntryBase>(
                                        p.TransactionEntries
                                            .Select(z => new PrescriptionEntry()
                                            {
                                                Quantity = z.Quantity,
                                                Price = z.Price,
                                                SalesTaxPercent = z.SalesTaxPercent,
                                                Discount = z.Discount,
                                                TransactionEntryItem = new TransactionEntryItem()
                                                {
                                                    ItemName = z.TransactionEntryItem.ItemName
                                                }
                                            }).ToList()),
                                    Transactions = new ObservableCollection<TransactionBase>(p.Transactions.Select(pp =>
                                        new Prescription()
                                        {
                                            TransactionId = pp.TransactionId,
                                            Time = pp.Time,

                                            Patient = new Patient()
                                            {
                                                FirstName = pp.Patient.FirstName,
                                                LastName = pp.Patient.LastName,
                                                Address = pp.Patient.Address,
                                                PhoneNumber = pp.Patient.PhoneNumber,
                                            },
                                            Doctor = new Doctor()
                                            {
                                                FirstName = pp.Doctor.FirstName,
                                                LastName = pp.Doctor.LastName,
                                            },
                                            TransactionEntries = new ObservableCollection<TransactionEntryBase>(
                                                pp.TransactionEntries
                                                    .Select(q => new PrescriptionEntry()
                                                    {
                                                        Quantity = q.Quantity,
                                                        Price = q.Price,
                                                        SalesTaxPercent = q.SalesTaxPercent,
                                                        Discount = q.Discount,
                                                        TransactionEntryItem = new TransactionEntryItem()
                                                        {
                                                            ItemName = q.TransactionEntryItem.ItemName
                                                        }
                                                    })),

                                        }).ToList())

                                }))
                        }).ToList(),
                    Doctors = x.GroupBy(t => t.Doctor)
                        .Select(doctor => new Doctor()
                        {
                            FirstName = doctor.Key.FirstName,
                            LastName = doctor.Key.LastName,
                            Code = doctor.Key.Code,
                            Patients = x.GroupBy(t => t.Patient)
                                .Select(patient => new Patient()
                                {
                                    FirstName = patient.Key.FirstName,
                                    LastName = patient.Key.LastName,
                                    Address = patient.Key.Address,
                                    PhoneNumber = patient.Key.PhoneNumber,

                                    Prescription = new ObservableCollection<Prescription>(
                                        patient.Select(p => new Prescription()
                                        {
                                            TransactionId = p.TransactionId,
                                            Time = p.Time,
                                            Patient = new Patient()
                                            {
                                                FirstName = p.Patient.FirstName,
                                                LastName = p.Patient.FirstName,
                                            },

                                            Doctor = new Doctor()
                                            {
                                                FirstName = p.Doctor.FirstName,
                                                LastName = p.Doctor.LastName,

                                            },
                                            TransactionEntries = new ObservableCollection<TransactionEntryBase>(
                                                p.TransactionEntries
                                                    .Select(z => new PrescriptionEntry()
                                                    {
                                                        Quantity = z.Quantity,
                                                        Price = z.Price,
                                                        SalesTaxPercent = z.SalesTaxPercent,
                                                        Discount = z.Discount,
                                                        TransactionEntryItem = new TransactionEntryItem()
                                                        {
                                                            ItemName = z.TransactionEntryItem.ItemName
                                                        }
                                                    }).ToList()),
                                            Transactions = new ObservableCollection<TransactionBase>(p.Transactions
                                                .Select(pp =>
                                                    new Prescription()
                                                    {
                                                        TransactionId = pp.TransactionId,
                                                        Time = pp.Time,

                                                        Patient = new Patient()
                                                        {

                                                            FirstName = pp.Patient.FirstName,
                                                            LastName = pp.Patient.LastName,


                                                        },
                                                        Doctor = new Doctor()
                                                        {
                                                            FirstName = pp.Doctor.FirstName,
                                                            LastName = pp.Doctor.LastName,

                                                        },
                                                        TransactionEntries =
                                                            new ObservableCollection<TransactionEntryBase>(
                                                                pp.TransactionEntries
                                                                    .Select(q => new PrescriptionEntry()
                                                                    {
                                                                        Quantity = q.Quantity,
                                                                        Price = q.Price,
                                                                        SalesTaxPercent = q.SalesTaxPercent,
                                                                        Discount = q.Discount,
                                                                        TransactionEntryItem =
                                                                            new TransactionEntryItem()
                                                                            {
                                                                                ItemName = q.TransactionEntryItem
                                                                                    .ItemName
                                                                            }
                                                                    })),

                                                    }).ToList())

                                        }))
                                }).ToList(),


                        }).ToList()
                }).ToList();

                var dList = ctx.Item.OfType<Medicine>().Where(x => (x.Description.Contains(drugSearchText) || x.ItemName.Contains(drugSearchText)) && !x.TransactionEntryItems.Any()).Take(listCount).ToList();
                res.AddRange(dList);

                return res;
            }
        }
    }
}
