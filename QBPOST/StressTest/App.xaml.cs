using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using log4netWrapper;

namespace QBPost
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Dispatcher.UnhandledException += DispatcherOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            Logger.Initialize();
            Logger.Log(LoggingLevel.Info, string.Format("Application Started - {0}", DateTime.Now));

       
        }

        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs eo)
        {
            LogError(eo.Exception);
        }
    
        private void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs eo)
        {
            LogError(eo.Exception);
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs eo)
        {
            LogError(eo.ExceptionObject as Exception);
        }
        private static void LogError(Exception e)
        {
            Logger.Log(LoggingLevel.Error, e.Message + e.StackTrace);
            MessageBox.Show(e.Message + e.StackTrace, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }


    }
}
