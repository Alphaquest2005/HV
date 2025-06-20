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
    
    public partial class LocationType : BaseEntity<LocationType>
    {
        
        public LocationType()
        {
            this.PersonLocations = new ObservableCollection<PersonLocation>();
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
    
            
            ChangeTrackingCollection<LocationType> _changeTracker;
            [NotMapped]
            [IgnoreDataMember]
            public ChangeTrackingCollection<LocationType> ChangeTracker
            {
                get
                {
                    return _changeTracker;
                }
            }
            
            public new void StartTracking()
            {
                _changeTracker = new ChangeTrackingCollection<LocationType>(this);
            }
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
                    [Required(ErrorMessage="Name is required")]
                [StringLength(50)]
    	public string Name
    	{ 
    		get { return _Name; }
    		set
    		{
    			if (Equals(value, _Name)) return;
    			_Name = value;
                ValidateModelProperty(this, value);
    			NotifyPropertyChanged();
    		}
    	}
    	private string _Name;
        [DataMember]
    	public ObservableCollection<PersonLocation> PersonLocations
    	{
    		get { return _PersonLocations; }
    		set
    		{
    			if (Equals(value, _PersonLocations)) return;
    			_PersonLocations = value;
    			NotifyPropertyChanged();
    		}
    	}
    	private ObservableCollection<PersonLocation> _PersonLocations;
    }
}
