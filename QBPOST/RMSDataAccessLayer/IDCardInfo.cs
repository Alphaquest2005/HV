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
    
    public partial class IDCardInfo : BaseEntity<IDCardInfo>
    {
        [DataMember]
                    [Required(ErrorMessage="FirstName is required")]
                [StringLength(50)]
    	public string FirstName
    	{ 
    		get { return _FirstName; }
    		set
    		{
    			if (Equals(value, _FirstName)) return;
    			_FirstName = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private string _FirstName;
        [DataMember]
                    [Required(ErrorMessage="LastName is required")]
                [StringLength(50)]
    	public string LastName
    	{ 
    		get { return _LastName; }
    		set
    		{
    			if (Equals(value, _LastName)) return;
    			_LastName = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private string _LastName;
        [DataMember]
                    [StringLength(255)]
    	public string Address
    	{ 
    		get { return _Address; }
    		set
    		{
    			if (Equals(value, _Address)) return;
    			_Address = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private string _Address;
        [DataMember]
                    [StringLength(50)]
    	public string PhoneNumber
    	{ 
    		get { return _PhoneNumber; }
    		set
    		{
    			if (Equals(value, _PhoneNumber)) return;
    			_PhoneNumber = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private string _PhoneNumber;
        [DataMember]
                    [StringLength(50)]
    	public string CardId
    	{ 
    		get { return _CardId; }
    		set
    		{
    			if (Equals(value, _CardId)) return;
    			_CardId = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private string _CardId;
        [DataMember]
        	public Nullable<double> TotalSales
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
    	private Nullable<double> _TotalSales;
        [DataMember]
                    [Required(ErrorMessage="Points is required")]
    	public int Points
    	{ 
    		get { return _Points; }
    		set
    		{
    			if (Equals(value, _Points)) return;
    			_Points = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private int _Points;
        [DataMember]
                    [Required(ErrorMessage="EntrySalesAmount is required")]
    	public double EntrySalesAmount
    	{ 
    		get { return _EntrySalesAmount; }
    		set
    		{
    			if (Equals(value, _EntrySalesAmount)) return;
    			_EntrySalesAmount = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private double _EntrySalesAmount;
        [DataMember]
                    [Required(ErrorMessage="MaxSalesAmount is required")]
    	public double MaxSalesAmount
    	{ 
    		get { return _MaxSalesAmount; }
    		set
    		{
    			if (Equals(value, _MaxSalesAmount)) return;
    			_MaxSalesAmount = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private double _MaxSalesAmount;
        [DataMember]
                    [Required(ErrorMessage="PointRatePerDollar is required")]
    	public double PointRatePerDollar
    	{ 
    		get { return _PointRatePerDollar; }
    		set
    		{
    			if (Equals(value, _PointRatePerDollar)) return;
    			_PointRatePerDollar = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private double _PointRatePerDollar;
        [DataMember]
                    [Required(ErrorMessage="Discount is required")]
    	public double Discount
    	{ 
    		get { return _Discount; }
    		set
    		{
    			if (Equals(value, _Discount)) return;
    			_Discount = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private double _Discount;
        [DataMember]
                    [Required(ErrorMessage="QuickBooksPriceLevel is required")]
                [StringLength(50)]
    	public string QuickBooksPriceLevel
    	{ 
    		get { return _QuickBooksPriceLevel; }
    		set
    		{
    			if (Equals(value, _QuickBooksPriceLevel)) return;
    			_QuickBooksPriceLevel = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private string _QuickBooksPriceLevel;
        [DataMember]
                    [Required(ErrorMessage="Store is required")]
    	public string Store
    	{ 
    		get { return _Store; }
    		set
    		{
    			if (Equals(value, _Store)) return;
    			_Store = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private string _Store;
        [DataMember]
                    [Required(ErrorMessage="PatientId is required")]
    	public int PatientId
    	{ 
    		get { return _PatientId; }
    		set
    		{
    			if (Equals(value, _PatientId)) return;
    			_PatientId = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private int _PatientId;
        [DataMember]
                    [Required(ErrorMessage="Membership is required")]
                [StringLength(50)]
    	public string Membership
    	{ 
    		get { return _Membership; }
    		set
    		{
    			if (Equals(value, _Membership)) return;
    			_Membership = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private string _Membership;
        [DataMember]
                    [Required(ErrorMessage="MembershipId is required")]
    	public int MembershipId
    	{ 
    		get { return _MembershipId; }
    		set
    		{
    			if (Equals(value, _MembershipId)) return;
    			_MembershipId = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private int _MembershipId;
        [DataMember]
    	public Patient Patient
    	{
    		get { return _Patient; }
    		set
    		{
    			if (Equals(value, _Patient)) return;
    			_Patient = value;
    			PatientChangeTracker = _Patient == null ? null
    				: new ChangeTrackingCollection<Patient> { _Patient };
    			NotifyPropertyChanged();
    		}
    	}
    	private Patient _Patient;
    	private ChangeTrackingCollection<Patient> PatientChangeTracker { get; set; }
    }
}
