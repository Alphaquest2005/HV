
using System;
using System.Windows;
using log4netWrapper;
using System.Xml;
using QBPOSXMLRPLib;
using QuickBooks;

namespace QS2QBPost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           // var t = QBClass.Instance;
           
        }

       
        private  void Post2QB(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!QBClass.IsPosting)
                    QBClass.PostToQB();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

  
}

   
    

