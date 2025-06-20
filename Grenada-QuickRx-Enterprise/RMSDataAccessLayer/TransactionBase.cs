//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RMSDataAccessLayer
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using TrackableEntities;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using System.Collections.ObjectModel;

    using System;
    using System.Collections.Generic;
    using TrackableEntities.Client;
    
    public partial class TransactionBase : BaseEntity<TransactionBase>
    {
        
        public TransactionBase()
        {
            this.TransactionEntries = new ObservableCollection<TransactionEntryBase>();
            this.Transactions = new ObservableCollection<TransactionBase>();
            CustomStartup();
            CustomStartup2();
            this.PropertyChanged += UpdatePropertyChanged;
            
        }
        partial void CustomStartup();
        partial void CustomStartup2();
    
            private void UpdatePropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (!string.IsNullOrEmpty(e.PropertyName) && (!Environment.StackTrace.Contains("Internal.Materialization")) && TrackingState == TrackingState.Unchanged)
                {
                    TrackingState = TrackingState.Modified;
                }
            }
    
            
            ChangeTrackingCollection<TransactionBase> _changeTracker;
            [NotMapped]
            [IgnoreDataMember]
            public ChangeTrackingCollection<TransactionBase> ChangeTracker
            {
                get
                {
                    return _changeTracker;
                }
            }
            
            public new void StartTracking()
            {
                _changeTracker = new ChangeTrackingCollection<TransactionBase>(this);
            }
        [DataMember]
                    [Required(ErrorMessage="StationId is required")]
    	public int StationId
    	{ 
    		get { return _StationId; }
    		set
    		{
    			if (Equals(value, _StationId)) return;
    			_StationId = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private int _StationId;
        [DataMember]
                    [Required(ErrorMessage="BatchId is required")]
    	public int BatchId
    	{ 
    		get { return _BatchId; }
    		set
    		{
    			if (Equals(value, _BatchId)) return;
    			_BatchId = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private int _BatchId;
        [DataMember]
        	public Nullable<int> CloseBatchId
    	{ 
    		get { return _CloseBatchId; }
    		set
    		{
    			if (Equals(value, _CloseBatchId)) return;
    			_CloseBatchId = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private Nullable<int> _CloseBatchId;
        [DataMember]
                    [Required(ErrorMessage="Time is required")]
    	public System.DateTime Time
    	{ 
    		get { return _Time; }
    		set
    		{
    			if (Equals(value, _Time)) return;
    			_Time = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private System.DateTime _Time;
        [DataMember]
        	public Nullable<int> CustomerId
    	{ 
    		get { return _CustomerId; }
    		set
    		{
    			if (Equals(value, _CustomerId)) return;
    			_CustomerId = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private Nullable<int> _CustomerId;
        [DataMember]
                    [Required(ErrorMessage="CashierId is required")]
    	public int CashierId
    	{ 
    		get { return _CashierId; }
    		set
    		{
    			if (Equals(value, _CashierId)) return;
    			_CashierId = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private int _CashierId;
        [DataMember]
                    [StringLength(255)]
    	public string Comment
    	{ 
    		get { return _Comment; }
    		set
    		{
    			if (Equals(value, _Comment)) return;
    			_Comment = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private string _Comment;
        [DataMember]
                    [StringLength(50)]
    	public string ReferenceNumber
    	{ 
    		get { return _ReferenceNumber; }
    		set
    		{
    			if (Equals(value, _ReferenceNumber)) return;
    			_ReferenceNumber = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private string _ReferenceNumber;
        [DataMember]
                    [StringLength(30)]
    	public string StoreCode
    	{ 
    		get { return _StoreCode; }
    		set
    		{
    			if (Equals(value, _StoreCode)) return;
    			_StoreCode = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private string _StoreCode;
        [DataMember]
                    [Required(ErrorMessage="TransactionId is required")]
    	public int TransactionId
    	{ 
    		get { return _TransactionId; }
    		set
    		{
    			if (Equals(value, _TransactionId)) return;
    			_TransactionId = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private int _TransactionId;
        [DataMember]
                    [Required(ErrorMessage="OpenClose is required")]
    	public bool OpenClose
    	{ 
    		get { return _OpenClose; }
    		set
    		{
    			if (Equals(value, _OpenClose)) return;
    			_OpenClose = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private bool _OpenClose;
        [DataMember]
        	public Nullable<int> PharmacistId
    	{ 
    		get { return _PharmacistId; }
    		set
    		{
    			if (Equals(value, _PharmacistId)) return;
    			_PharmacistId = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private Nullable<int> _PharmacistId;
        [DataMember]
                    [StringLength(50)]
    	public string Status
    	{ 
    		get { return _Status; }
    		set
    		{
    			if (Equals(value, _Status)) return;
    			_Status = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private string _Status;
        [DataMember]
        	public byte[] EntryTimeStamp
    	{ 
    		get { return _EntryTimeStamp; }
    		set
    		{
    			if (Equals(value, _EntryTimeStamp)) return;
    			_EntryTimeStamp = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private byte[] _EntryTimeStamp;
        [DataMember]
        	public Nullable<int> ParentTransactionId
    	{ 
    		get { return _ParentTransactionId; }
    		set
    		{
    			if (Equals(value, _ParentTransactionId)) return;
    			_ParentTransactionId = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private Nullable<int> _ParentTransactionId;
        [DataMember]
                    [StringLength(50)]
    	public string DeliveryType
    	{ 
    		get { return _DeliveryType; }
    		set
    		{
    			if (Equals(value, _DeliveryType)) return;
    			_DeliveryType = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private string _DeliveryType;
        [DataMember]
    	public ObservableCollection<TransactionEntryBase> TransactionEntries
    	{
    		get { return _TransactionEntries; }
    		set
    		{
    			if (Equals(value, _TransactionEntries)) return;
    			_TransactionEntries = value;
    			NotifyPropertyChanged();
    		}
    	}
    	private ObservableCollection<TransactionEntryBase> _TransactionEntries;
        [DataMember]
    	public Person Customer
    	{
    		get { return _Customer; }
    		set
    		{
    			if (Equals(value, _Customer)) return;
    			_Customer = value;
    			CustomerChangeTracker = _Customer == null ? null
    				: new ChangeTrackingCollection<Person> { _Customer };
    			NotifyPropertyChanged();
    		}
    	}
    	private Person _Customer;
    	private ChangeTrackingCollection<Person> CustomerChangeTracker { get; set; }
        [DataMember]
    	public Cashier Cashier
    	{
    		get { return _Cashier; }
    		set
    		{
    			if (Equals(value, _Cashier)) return;
    			_Cashier = value;
    			CashierChangeTracker = _Cashier == null ? null
    				: new ChangeTrackingCollection<Cashier> { _Cashier };
    			NotifyPropertyChanged();
    		}
    	}
    	private Cashier _Cashier;
    	private ChangeTrackingCollection<Cashier> CashierChangeTracker { get; set; }
        [DataMember]
    	public Batch Batch
    	{
    		get { return _Batch; }
    		set
    		{
    			if (Equals(value, _Batch)) return;
    			_Batch = value;
    			BatchChangeTracker = _Batch == null ? null
    				: new ChangeTrackingCollection<Batch> { _Batch };
    			NotifyPropertyChanged();
    		}
    	}
    	private Batch _Batch;
    	private ChangeTrackingCollection<Batch> BatchChangeTracker { get; set; }
        [DataMember]
    	public Station Station
    	{
    		get { return _Station; }
    		set
    		{
    			if (Equals(value, _Station)) return;
    			_Station = value;
    			StationChangeTracker = _Station == null ? null
    				: new ChangeTrackingCollection<Station> { _Station };
    			NotifyPropertyChanged();
    		}
    	}
    	private Station _Station;
    	private ChangeTrackingCollection<Station> StationChangeTracker { get; set; }
        [DataMember]
    	public Batch CloseBatch
    	{
    		get { return _CloseBatch; }
    		set
    		{
    			if (Equals(value, _CloseBatch)) return;
    			_CloseBatch = value;
    			CloseBatchChangeTracker = _CloseBatch == null ? null
    				: new ChangeTrackingCollection<Batch> { _CloseBatch };
    			NotifyPropertyChanged();
    		}
    	}
    	private Batch _CloseBatch;
    	private ChangeTrackingCollection<Batch> CloseBatchChangeTracker { get; set; }
        [DataMember]
    	public Cashier Pharmacist
    	{
    		get { return _Pharmacist; }
    		set
    		{
    			if (Equals(value, _Pharmacist)) return;
    			_Pharmacist = value;
    			PharmacistChangeTracker = _Pharmacist == null ? null
    				: new ChangeTrackingCollection<Cashier> { _Pharmacist };
    			NotifyPropertyChanged();
    		}
    	}
    	private Cashier _Pharmacist;
    	private ChangeTrackingCollection<Cashier> PharmacistChangeTracker { get; set; }
        [DataMember]
    	public ObservableCollection<TransactionBase> Transactions
    	{
    		get { return _Transactions; }
    		set
    		{
    			if (Equals(value, _Transactions)) return;
    			_Transactions = value;
    			NotifyPropertyChanged();
    		}
    	}
    	private ObservableCollection<TransactionBase> _Transactions;
        [DataMember]
    	public TransactionBase ParentTransaction
    	{
    		get { return _ParentTransaction; }
    		set
    		{
    			if (Equals(value, _ParentTransaction)) return;
    			_ParentTransaction = value;
    			ParentTransactionChangeTracker = _ParentTransaction == null ? null
    				: new ChangeTrackingCollection<TransactionBase> { _ParentTransaction };
    			NotifyPropertyChanged();
    		}
    	}
    	private TransactionBase _ParentTransaction;
    	private ChangeTrackingCollection<TransactionBase> ParentTransactionChangeTracker { get; set; }
        [DataMember]
    	public TransactionLocation TransactionLocation
    	{
    		get { return _TransactionLocation; }
    		set
    		{
    			if (Equals(value, _TransactionLocation)) return;
    			_TransactionLocation = value;
    			TransactionLocationChangeTracker = _TransactionLocation == null ? null
    				: new ChangeTrackingCollection<TransactionLocation> { _TransactionLocation };
    			NotifyPropertyChanged();
    		}
    	}
    	private TransactionLocation _TransactionLocation;
    	private ChangeTrackingCollection<TransactionLocation> TransactionLocationChangeTracker { get; set; }
    }
}
