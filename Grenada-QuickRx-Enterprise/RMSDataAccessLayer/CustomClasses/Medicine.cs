using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;

namespace RMSDataAccessLayer
{
   public partial class Medicine
   {
       public List<Patient> Patients { get; set; }
       public List<Doctor> Doctors { get; set; }
   }
}
