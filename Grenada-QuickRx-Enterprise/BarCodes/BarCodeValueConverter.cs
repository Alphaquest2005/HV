using System;
using System.Windows.Data;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Windows;
using GenCode128;

namespace BarCodes
{
    public class BarCodeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.ToString() == "0") return null;
            long val = 0;
            if ( value != null && long.TryParse(value.ToString(), out val))
            {
                //string sval = val.ToString().PadLeft(12, '0');
                //BarCodes.UPCA.cUPCA upc = new UPCA.cUPCA();
                //BitmapSource bs = upc.CreateBarCodeBitmapSource(sval, 1);

                Image myimg = Code128Rendering.MakeBarcodeImage(value.ToString(), int.Parse("2"), true);
                var b = new Bitmap(myimg);
                IntPtr hBitmap = b.GetHbitmap();
                System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                return bitmapSource;

            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
