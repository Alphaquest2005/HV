using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Data;

namespace RMSDataAccessLayer
{
    public partial class PrescriptionEntry : ISearchItem
    {
        partial void CustomStartup2()
        {
            PropertyChanged += PrescriptionEntry_PropertyChanged;
        }

        private void PrescriptionEntry_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Quantity)) NotifyPropertyChanged(nameof(RepeatInfo));
            if (e.PropertyName == nameof(Repeat)) NotifyPropertyChanged(nameof(RepeatInfo));
            if (e.PropertyName == nameof(RepeatQuantity)) NotifyPropertyChanged(nameof(RepeatInfo));
        }

        public int Total => Convert.ToInt32(Repeat * RepeatQuantity);

        public int TotalTaken
        {
            get
            {
                var pres = ((Prescription) this.Transaction);

                var lst = new List<PrescriptionEntry>();
                if (pres.ParentPrescription != null)
                {
                    lst.AddRange(pres.ParentPrescription.Prescriptions.SelectMany(x => x.TransactionEntries).OfType<PrescriptionEntry>()
                        .Where(x => x.TransactionEntryId < TransactionEntryId));
                    lst.AddRange(pres.ParentPrescription.TransactionEntries.OfType<PrescriptionEntry>().Where(x => x.TransactionEntryId <= TransactionEntryId));
                }
                    

                lst.AddRange(pres.Prescriptions.SelectMany(x => x.TransactionEntries).OfType<PrescriptionEntry>().Where(x => x.TransactionEntryId <= TransactionEntryId));
                    
                return lst.Sum(x => Convert.ToInt32(x.Quantity));
            }
        }

        public int Remaining => Total - TotalTaken;

        public string RepeatInfo
        {
            get
            {
                if (RepeatQuantity == null) return "";
                var r = 0;
                var repeat = Math.DivRem(Remaining - Convert.ToInt32(Quantity), RepeatQuantity?? 1, out r);

                var rstr = "";
                if (r > 0) rstr = $"Bal:{r} |";

                var repeatstr = "";
                if (repeat > 0) repeatstr = $"Repeat: {repeat} of {RepeatQuantity}  ";

                var totalstr = (rstr + repeatstr);

                return string.IsNullOrEmpty(totalstr)?totalstr:totalstr.Substring(0,totalstr.Length - 2);
            }
        }

        public int Remainder => Convert.ToInt32(RepeatQuantity) - Convert.ToInt32(Quantity);

        #region ISearchItem Members
        [NotMapped]
        [IgnoreDataMember]
        public string SearchCriteria
        {
            get
            {
                return DisplayName + "|";
            }
            set
            {
               
            }
        }
        [NotMapped]
        [IgnoreDataMember]
        public string DisplayName
        {
            get { return ""; } //this.Item.DisplayName; 
            }
        [NotMapped]
        [IgnoreDataMember]
        public string Key
        {
            get { return TransactionEntryNumber; }
        }

        #endregion
    }
}
