using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
    public partial class Patient
    {
        //public Patient()
        //{
        //    Discount = 0;
        //}

        public string ContactInfo
        {
            get
            {
                string guardian = "";
               // string allergies = "";
                if(Guardian != null)
                {
                    guardian = @" - C\o " + Guardian;

                }
               //if (Allergies != null)
               // {
               //     allergies = " - A&I " + Allergies;
               // }
                
                       

                return String.Format("{0}{1} - {2}", DisplayName,guardian, Address);//allergies
            }
        }

        public string AllergiesEx
        {
            get
            {
                if (Allergies != null)
                {
                    return "Allergy: " + Allergies;
                }
                else
                {
                    return null;
                }
            }
        }

        public new string SearchCriteria => $"p:{DisplayName ?? ""}|{Allergies ?? ""}|{Address ?? ""}|{DOB?.Year.ToString() ?? ""}|{PhoneNumber?.ToString() ?? ""}|{Guardian ?? ""}";
    }
}
