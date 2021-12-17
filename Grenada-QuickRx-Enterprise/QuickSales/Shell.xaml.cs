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
using System.Windows.Shapes;
using log4netWrapper;

namespace QuickSales
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public Shell()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                var lastexception = false;
                while (lastexception == false)
                {

                    if (ex.InnerException == null)
                    {
                        lastexception = true;
                        var errorMessage = $"An unhandled Exception occurred!: {ex.Message} ---- {ex.StackTrace}";
                        Logger.Log(LoggingLevel.Error, errorMessage);
                        MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }

                    ex = ex.InnerException;

                }
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            App app = Application.Current as App;
            app.LoginRoutine();

        }

      
    }
}
