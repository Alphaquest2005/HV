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
    
    public partial class Store : BaseEntity<Store>
    {
        [DataMember]
                    [Required(ErrorMessage="StoreId is required")]
    	public int StoreId
    	{ 
    		get { return _StoreId; }
    		set
    		{
    			if (Equals(value, _StoreId)) return;
    			_StoreId = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private int _StoreId;
        [DataMember]
                    [Required(ErrorMessage="StoreCode is required")]
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
                    [Required(ErrorMessage="StoreAddress is required")]
    	public string StoreAddress
    	{ 
    		get { return _StoreAddress; }
    		set
    		{
    			if (Equals(value, _StoreAddress)) return;
    			_StoreAddress = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private string _StoreAddress;
        [DataMember]
                    [Required(ErrorMessage="CompanyId is required")]
    	public int CompanyId
    	{ 
    		get { return _CompanyId; }
    		set
    		{
    			if (Equals(value, _CompanyId)) return;
    			_CompanyId = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private int _CompanyId;
        [DataMember]
                    [Required(ErrorMessage="TransactionSeed is required")]
    	public int TransactionSeed
    	{ 
    		get { return _TransactionSeed; }
    		set
    		{
    			if (Equals(value, _TransactionSeed)) return;
    			_TransactionSeed = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private int _TransactionSeed;
        [DataMember]
                    [Required(ErrorMessage="SeedTransaction is required")]
    	public int SeedTransaction
    	{ 
    		get { return _SeedTransaction; }
    		set
    		{
    			if (Equals(value, _SeedTransaction)) return;
    			_SeedTransaction = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private int _SeedTransaction;
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
    }
}
