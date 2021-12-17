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
                return string.Format("d:{0} {1}",DisplayName, Code);
            }
            set
            {
                
            }
        }

        public new string DisplayName
        {
            get {
                if (FirstName != null && (FirstName.IndexOf("Dr ", StringComparison.Ordinal) == -1 && FirstName.IndexOf("Dr.", StringComparison.Ordinal) == -1))
                {
                    return "Dr." + " " + FirstName.Trim() + " " + LastName.Trim() + (string.IsNullOrEmpty(Code) ? "" : " - " + Code?.Trim());
                }
                else
                {
                    if (FirstName != null) return FirstName.Trim().Replace(".","").Replace(" ","").Replace("Dr", "Dr. ") + " " + LastName.Trim() + (string.IsNullOrEmpty(Code) ? "" : " - " + Code?.Trim());
                }
                return FirstName + " " + LastName + "-" + (string.IsNullOrEmpty(Code) ? "" : " - " + Code?.Trim());
            }// base.Salutation + " " +
        }

        public List<Patient> Patients { get; set; }
    }
}
