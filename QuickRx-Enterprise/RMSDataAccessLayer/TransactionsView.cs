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
    
    public partial class TransactionsView : BaseEntity<TransactionsView>
    {
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
        	public Nullable<decimal> TotalSales
    	{ 
    		get { return _TotalSales; }
    		set
    		{
    			if (Equals(value, _TotalSales)) return;
    			_TotalSales = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private Nullable<decimal> _TotalSales;
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
    }
}
