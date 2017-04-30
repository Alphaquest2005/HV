using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
    public partial class Item : ISearchItem
    {
        //public Item()
        //{
        //    Department = "MISC";
        //}
        public string SearchCriteria
        {
            get {
                return String.Format("m:{0}|{1}|{2}", ItemName, Description, ItemNumber);
            }
            set
            {
            }
        }

        public virtual string DisplayName
        {
            get { return ItemName ?? Description; }
        }

        
        public string Key
        {
            get { return ItemId.ToString(); }
        }

        public double VatPrice
        {
            get { return Convert.ToDouble(Price)*((double) (1 + SalesTax)); }
        }

      
    }
}
