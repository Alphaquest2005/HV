using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
    public partial class Doctor
    {

        
        public new string SearchCriteria
        {
            get
            {
                return $"d:{DisplayName} {Code}";
            }
            set
            {
                
            }
        }

        public string DisplayName
        {
            get {
                if (FirstName != null && (FirstName.IndexOf("Dr ", StringComparison.Ordinal) == -1 && FirstName.IndexOf("Dr.", StringComparison.Ordinal) == -1))
                {
                    return "Dr." + " " + FirstName.Trim() + " " + LastName.Trim();
                }
                else
                {
                    if (FirstName != null) return FirstName.Trim().Replace(".","").Replace(" ","").Replace("Dr", "Dr. ") + " " + LastName.Trim();
                }
                return FirstName + " " + LastName;
            }// base.Salutation + " " +
        }


    }
}
