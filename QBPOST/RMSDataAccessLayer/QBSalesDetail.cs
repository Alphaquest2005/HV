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
    
    public partial class QBSalesDetail : BaseEntity<QBSalesDetail>
    {
        [DataMember]
                    [Required(ErrorMessage="Id is required")]
    	public int Id
    	{ 
    		get { return _Id; }
    		set
    		{
    			if (Equals(value, _Id)) return;
    			_Id = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private int _Id;
        [DataMember]
                    [Required(ErrorMessage="TxnDate is required")]
    	public System.DateTime TxnDate
    	{ 
    		get { return _TxnDate; }
    		set
    		{
    			if (Equals(value, _TxnDate)) return;
    			_TxnDate = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private System.DateTime _TxnDate;
        [DataMember]
                    [Required(ErrorMessage="Total is required")]
    	public double Total
    	{ 
    		get { return _Total; }
    		set
    		{
    			if (Equals(value, _Total)) return;
    			_Total = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private double _Total;
        [DataMember]
    	public QBSale QBSale
    	{
    		get { return _QBSale; }
    		set
    		{
    			if (Equals(value, _QBSale)) return;
    			_QBSale = value;
    			QBSaleChangeTracker = _QBSale == null ? null
    				: new ChangeTrackingCollection<QBSale> { _QBSale };
    			NotifyPropertyChanged();
    		}
    	}
    	private QBSale _QBSale;
    	private ChangeTrackingCollection<QBSale> QBSaleChangeTracker { get; set; }
    }
}