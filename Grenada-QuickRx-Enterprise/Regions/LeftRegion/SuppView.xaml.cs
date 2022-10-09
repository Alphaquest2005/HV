using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RMSDataAccessLayer;
using System.Threading.Tasks;
using SalesRegion;
using TrackableEntities;


namespace LeftRegion
{
    /// <summary>
    /// Interaction logic for TransactionView.xaml
    /// </summary>
    public partial class SuppView : UserControl
    {
        public SuppView()
        {
            InitializeComponent();

            // Setup the view model context

            tvm = SuppVM.Instance;


        }
        private SuppVM tvm;
        private void SearchPatient(object sender, RoutedEventArgs e)
        {

        }

        private void SearchMedication(object sender, RoutedEventArgs e)
        {

        }

        private void SearchDoctor(object sender, RoutedEventArgs e)
        {

        }

        private void SearchTransactions(object sender, RoutedEventArgs e)
        {

        }

        private async void PrescriptionSearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e == null) return;
            if (e.Key == Key.Enter)
            {
                if (tvm != null)
                {
                    tvm.SearchResults = new System.Collections.ObjectModel.ObservableCollection<Prescription>() { };


                    Status.Text = "Searching";


                    if (Application.Current != null)
                        Application.Current.Dispatcher.Invoke(
                            () => { if (SearchBox != null) tvm.SearchText = SearchBox.Text; });

                    await Task.Run(() => tvm.SearchTransactions()).ConfigureAwait(false);

                    if (Application.Current != null)
                        Application.Current.Dispatcher.Invoke(
                            () => { Status.Text = ""; });


                }
            }
        }

        private async void PatientSearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e == null) return;
            if (e.Key == Key.Enter)
            {
                if (tvm != null)
                {
                    tvm.PatientSearchResults = new System.Collections.ObjectModel.ObservableCollection<Patient>() { };


                    PatientStatus.Text = "Searching";


                    if (Application.Current != null)
                        Application.Current.Dispatcher.Invoke(
                            () => { if (PatientSearchBox != null) tvm.PatientSearchText = PatientSearchBox.Text; });

                    await Task.Run(() => tvm.SearchPatients()).ConfigureAwait(false);

                    if (Application.Current != null)
                        Application.Current.Dispatcher.Invoke(
                            () => { PatientStatus.Text = ""; });


                }
            }
        }
        private async void DoctorSearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e == null) return;
            if (e.Key == Key.Enter)
            {
                if (tvm != null)
                {
                    tvm.DoctorSearchResults = new System.Collections.ObjectModel.ObservableCollection<Doctor>() { };


                    DoctorStatus.Text = "Searching";


                    if (Application.Current != null)
                        Application.Current.Dispatcher.Invoke(
                            () => { if (DoctorSearchBox != null) tvm.DoctorSearchText = DoctorSearchBox.Text; });


                    await Task.Run(() => tvm.SearchDoctors()).ConfigureAwait(false);

                    if (Application.Current != null)
                        Application.Current.Dispatcher.Invoke(
                            () => { DoctorStatus.Text = ""; });


                }
            }
        }

        private async void DrugSearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e == null) return;
            if (e.Key == Key.Enter)
            {
                if (tvm != null)
                {
                    tvm.DrugSearchResults = new System.Collections.ObjectModel.ObservableCollection<Medicine>() { };


                    DrugStatus.Text = "Searching";


                    if (Application.Current != null)
                        Application.Current.Dispatcher.Invoke(
                            () => { if (DrugSearchBox != null) tvm.DrugSearchText = DrugSearchBox.Text; });


                    await Task.Run(() => tvm.SearchDrugs()).ConfigureAwait(false);

                    if (Application.Current != null)
                        Application.Current.Dispatcher.Invoke(
                            () => { DrugStatus.Text = ""; });




                }
            }
        }

        private void AutoRepeatBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tvm == null) return;
            var i = sender as FrameworkElement;
            if (i != null)
            {
                var p = (Prescription)i.DataContext; //GetTransactionData()
                var rp = p.TransactionId;
                var ntrn = SalesVM.Instance.GoToTransaction(rp);
                //tvm.TransactionData = rp;
                if(ntrn != null)
                    tvm.AutoRepeat(null);
                else
                {
                    SalesVM.Instance.CreateNewTransaction(p);
                }
            }

        }


        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (tvm != null)
            {
                var frameworkElement = sender as FrameworkElement;
                if (frameworkElement != null)
                    tvm.TransactionData = (Prescription)frameworkElement.DataContext;//GetTransactionData()
            }
            e.Handled = true;
        }

        private void NewPrescription(object sender, RoutedEventArgs e)
        {
            if (tvm != null)
            {
                var frameworkElement = sender as FrameworkElement;
                if (frameworkElement != null)
                    tvm.TransactionData = (Prescription)frameworkElement.DataContext;//GetTransactionData()
                tvm.CopyPrescription();
            }
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SalesRegion.SalesVM.Instance.ShowInactiveItems = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SalesRegion.SalesVM.Instance.ShowInactiveItems = false;
        }


        private void SavePatientBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!SalesVM.Instance.SavePatient(SuppVM.Instance.CurrentPatient))
            {
                MessageBox.Show("Problem saving Patient. Please ensure you fill out First and Last Name");
            }
            else
            {
                MessageBox.Show($"{SuppVM.Instance.CurrentPatient.Name} Saved!");
            }
        }

        private void SaveMedicineBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SuppVM.Instance.CurrentDrug.Description == "New Drug")
            {
                MessageBox.Show("Please give the Drug a name");
                return;
            }
            if (!SalesVM.Instance.SaveMedicine(SuppVM.Instance.CurrentDrug))
            {
                MessageBox.Show("Problem saving Drug. Please ensure you fill out Item Name, Dosage, Quantity and Price");
            }
            else
            {
                MessageBox.Show($"{SuppVM.Instance.CurrentDrug.Description} Saved!");
            }
        }

        private void PatientExpander_OnExpanded(object sender, RoutedEventArgs e)
        {
            if (tvm != null)
            {
                var frameworkElement = sender as FrameworkElement;
                if (frameworkElement != null)
                    tvm.CurrentPatient = (Patient)frameworkElement.DataContext;//GetTransactionData()
            }
        }

        private void DoctorExpander_OnExpanded(object sender, RoutedEventArgs e)
        {
            if (tvm != null)
            {
                var frameworkElement = sender as FrameworkElement;
                if (frameworkElement != null)
                    tvm.CurrentDoctor = (Doctor)frameworkElement.DataContext;//GetTransactionData()
            }
        }

        private void DrugExpander_OnExpanded(object sender, RoutedEventArgs e)
        {
            if (tvm != null)
            {
                var frameworkElement = sender as FrameworkElement;
                if (frameworkElement != null)
                    tvm.CurrentDrug = (Medicine)frameworkElement.DataContext;//GetTransactionData()
            }
        }

        private void SaveDoctorBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!SalesVM.Instance.SaveDoctor(SuppVM.Instance.CurrentDoctor))
            {
                MessageBox.Show("Problem saving Doctor. Please ensure you fill out First, Last Name and Code - use zero if you don't know");
            }
            else
            {
                MessageBox.Show($"{SuppVM.Instance.CurrentDoctor.Name} Saved!");
            }
        }

        private void AddDrugBtn_Click(object sender, RoutedEventArgs e)
        {
            Medicine i = new Medicine() { TrackingState = TrackingState.Added };
            i.StartTracking();
            i.Description = "New Drug";
            i.QBActive = true;
            tvm.CurrentDrug = i;

        }

        private void AddPatientBtn_Click(object sender, RoutedEventArgs e)
        {
            var i = new Patient() { TrackingState = TrackingState.Added };
            i.StartTracking();
            tvm.CurrentPatient = i;
        }

        private void AddDoctorBtn_Click(object sender, RoutedEventArgs e)
        {
            var i = new Doctor() { TrackingState = TrackingState.Added };
            i.StartTracking();
            tvm.CurrentDoctor = i;
        }
    }
}
