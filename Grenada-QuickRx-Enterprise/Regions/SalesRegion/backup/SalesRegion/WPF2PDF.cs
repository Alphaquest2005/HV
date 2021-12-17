using System.Linq;

using RMSDataAccessLayer;

using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using System.Collections.Immutable;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Printing;
using System.Threading.Tasks;
using System.Transactions;
using SUT.PrintEngine.Utils;
using System.Windows.Media;
using Common.Core.Logging;
using log4netWrapper;
using QuickBooks;
using SalesRegion.Messages;
using SimpleMvvmToolkit;
using TrackableEntities;
using TrackableEntities.Common;
using TrackableEntities.EF6;
using System.Windows.Xps;
using System.IO;
using System.Windows.Xps.Packaging;
using System.IO.Packaging;
using SUT.PrintEngine;
using System.Windows.Documents;
using System.Windows.Media.Imaging;


using System;
using System.Linq;
using System.Printing;
using System.Runtime.InteropServices;

namespace SalesRegion
{
    public partial class SalesVM
    {
        public class WPF2PDF
        {
            private static int PixelsPerInch = 96;
            private static double PaperWidth = 8.5;
            private static int PaperHeight = 28;

            public static string CreatePDF(ref Grid rpt, string reportName)
            {



                XpsDocumentWriter writer;
                MemoryStream lMemoryStream = new MemoryStream();
                Package package = Package.Open(lMemoryStream, FileMode.Create);
                XpsDocument doc = new XpsDocument(package);
                DrawingVisual v = PrintVisual.GetVisual(ref rpt);
                // create XPS file based on a WPF Visual, and store it in a memorystream

                if (rpt.ActualWidth > PaperWidth)
                {


                    PageContent pageCnt = new PageContent();
                    FixedPage page;
                    // var oldParent = RemoveChild(rpt);
                    page = new FixedPage()
                    {
                        Height = rpt.ActualHeight,
                        Width = rpt.ActualWidth,
                    }; // {Height = (PaperWidth*PixelsPerInch), Width = (PaperHeight*PixelsPerInch), };
                    RenderTargetBitmap bmp = new RenderTargetBitmap((int) rpt.ActualWidth, (int) rpt.ActualHeight, 0, 0,
                        PixelFormats.Pbgra32);
                    bmp.Render(v);

                    Image image = new Image();
                    image.Source = bmp;
                    page.Children.Add(image);
                    // ((System.Windows.Markup.IAddChild) pageCnt).AddChild(page);


                    writer = XpsDocument.CreateXpsDocumentWriter(doc);
                    writer.Write(page);

                    //page.Children.Remove(rpt);
                    //AddChild(rpt, oldParent);
                }
                else
                {

                    writer = XpsDocument.CreateXpsDocumentWriter(doc);
                    writer.Write(v);
                }


                doc.Close();
                package.Close();

                var pdfXpsDoc = PdfSharp.Xps.XpsModel.XpsDocument.Open(lMemoryStream);
                string file = Path.Combine(
                    Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                    reportName + ".pdf");

                PdfSharp.Xps.XpsConverter.Convert(pdfXpsDoc, file, 0);

                return file;
            }
        }



        public static class PdfFilePrinter
        {
            private const string PdfPrinterDriveName = "Microsoft Print To PDF";

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            private class DOCINFOA
            {
                [MarshalAs(UnmanagedType.LPStr)] public string pDocName;
                [MarshalAs(UnmanagedType.LPStr)] public string pOutputFile;
                [MarshalAs(UnmanagedType.LPStr)] public string pDataType;
            }

            [DllImport("winspool.drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi,
                ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            private static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter,
                out IntPtr hPrinter, IntPtr pd);

            [DllImport("winspool.drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true,
                CallingConvention = CallingConvention.StdCall)]
            private static extern bool ClosePrinter(IntPtr hPrinter);

            [DllImport("winspool.drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi,
                ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
            private static extern int StartDocPrinter(IntPtr hPrinter, int level,
                [In, MarshalAs(UnmanagedType.LPStruct)]
                DOCINFOA di);

            [DllImport("winspool.drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true,
                CallingConvention = CallingConvention.StdCall)]
            private static extern bool EndDocPrinter(IntPtr hPrinter);

            [DllImport("winspool.drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true,
                CallingConvention = CallingConvention.StdCall)]
            private static extern bool StartPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true,
                CallingConvention = CallingConvention.StdCall)]
            private static extern bool EndPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true,
                CallingConvention = CallingConvention.StdCall)]
            private static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

            public static void PrintXpsToPdf(byte[] bytes, string outputFilePath, string documentTitle)
            {
                // Get Microsoft Print to PDF print queue
                var pdfPrintQueue = GetMicrosoftPdfPrintQueue();

                // Copy byte array to unmanaged pointer
                var ptrUnmanagedBytes = Marshal.AllocCoTaskMem(bytes.Length);
                Marshal.Copy(bytes, 0, ptrUnmanagedBytes, bytes.Length);

                // Prepare document info
                var di = new DOCINFOA
                {
                    pDocName = documentTitle,
                    pOutputFile = outputFilePath,
                    pDataType = "RAW"
                };

                // Print to PDF
                var errorCode = SendBytesToPrinter(pdfPrintQueue.Name, ptrUnmanagedBytes, bytes.Length, di,
                    out var jobId);

                // Free unmanaged memory
                Marshal.FreeCoTaskMem(ptrUnmanagedBytes);

                // Check if job in error state (for example not enough disk space)
                var jobFailed = false;
                try
                {
                    var pdfPrintJob = pdfPrintQueue.GetJob(jobId);
                    if (pdfPrintJob.IsInError)
                    {
                        jobFailed = true;
                        pdfPrintJob.Cancel();
                    }
                }
                catch
                {
                    // If job succeeds, GetJob will throw an exception. Ignore it. 
                }
                finally
                {
                    pdfPrintQueue.Dispose();
                }

                if (errorCode > 0 || jobFailed)
                {
                    try
                    {
                        if (File.Exists(outputFilePath))
                        {
                            File.Delete(outputFilePath);
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }

                if (errorCode > 0)
                {
                    throw new Exception($"Printing to PDF failed. Error code: {errorCode}.");
                }

                if (jobFailed)
                {
                    throw new Exception("PDF Print job failed.");
                }
            }

            private static int SendBytesToPrinter(string szPrinterName, IntPtr pBytes, int dwCount,
                DOCINFOA documentInfo, out int jobId)
            {
                jobId = 0;
                var dwWritten = 0;
                var success = false;

                if (OpenPrinter(szPrinterName.Normalize(), out var hPrinter, IntPtr.Zero))
                {
                    jobId = StartDocPrinter(hPrinter, 1, documentInfo);
                    if (jobId > 0)
                    {
                        if (StartPagePrinter(hPrinter))
                        {
                            success = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                            EndPagePrinter(hPrinter);
                        }

                        EndDocPrinter(hPrinter);
                    }

                    ClosePrinter(hPrinter);
                }

                // TODO: The other methods such as OpenPrinter also have return values. Check those?

                if (success == false)
                {
                    return Marshal.GetLastWin32Error();
                }

                return 0;
            }

            private static PrintQueue GetMicrosoftPdfPrintQueue()
            {
                PrintQueue pdfPrintQueue = null;

                try
                {
                    using (var printServer = new PrintServer())
                    {
                        var flags = new[] {EnumeratedPrintQueueTypes.Local};
                        // FirstOrDefault because it's possible for there to be multiple PDF printers with the same driver name (though unusual)
                        // To get a specific printer, search by FullName property instead (note that in Windows, queue name can be changed)
                        pdfPrintQueue = printServer.GetPrintQueues(flags)
                            .FirstOrDefault(lq => lq.QueueDriver.Name == PdfPrinterDriveName);
                    }

                    if (pdfPrintQueue == null)
                    {
                        throw new Exception($"Could not find printer with driver name: {PdfPrinterDriveName}");
                    }

                    if (!pdfPrintQueue.IsXpsDevice)
                    {
                        throw new Exception(
                            $"PrintQueue '{pdfPrintQueue.Name}' does not understand XPS page description language.");
                    }

                    return pdfPrintQueue;
                }
                catch
                {
                    pdfPrintQueue?.Dispose();
                    throw;
                }
            }
        }

    }
}
