using System;
using System.Threading;
using System.Windows;
using QBPOSXMLRPLib;
using QuickBooks.Properties;

namespace QuickBooks
{
    public static class QBPosContext
    {
       static RequestProcessor rp = new RequestProcessorClass();
        private static bool isConnected = false;
        static string ticket = null;
        static string response = null;

        public static string ProcessXML(string input, string QBCompanyFile)
        {
            
            ///bool started = false;
            try
            {

                while (isConnected == true )
                {
                    Thread.Sleep(1000);
                }

               if(ticket == null) ticket = BeginSession(QBCompanyFile);
                isConnected = true;
                var res = rp.ProcessRequest(ticket, input).ToString();
                isConnected = false;
                return res;
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show("COM Error Description = " + ex.Message, "COM error");
                return string.Empty;
            }
            finally
            {
                if (ticket != null)
                {
                    rp.EndSession(ticket);
                }
                if (rp != null)
                {
                    rp.CloseConnection();
                    ticket = null;
                }
            }
        }

        private static string BeginSession(string QBCompanyFile)
        {
            ///DO NOT BREAK TO CHECK THIS IT WILL NOT SHOW THE WINDOW AND STICK
            ///  BREAK AFTER
            rp.OpenConnection("QBPOST", "QBPOST");
            string connString = QBCompanyFile;//"Computer Name=server;Company Data=hills and valley gd;Version=11";

            return rp.BeginSession(connString);
            
        }

        //public static void Dispose()
        //{
        //    CloseSession();
        //}

        private static void CloseSession()
        {
            rp = null;
        }
    }
}