using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GMap.NET;

namespace SalesRegion
{
    /// <summary>
    /// Interaction logic for scratchpad.xaml
    /// </summary>
    public partial class scratchpad : UserControl
    {
        public scratchpad()
        {
            InitializeComponent();
        }

        private void EditDoctorTB_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }


        private void Gmap_OnOnPositionChanged(PointLatLng point)
        {
            throw new NotImplementedException();
        }

        private void Gmap_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PrescriptionEntriesRptLst_LayoutUpdated(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
