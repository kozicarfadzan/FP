using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace FahrradladenPrinzenstrasse.Mobile.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] bytes = value as byte[];

            if (bytes == null || bytes.Length == 0)
                return ImageSource.FromFile("default_FP.png");

            ImageSource source = ImageSource.FromStream(() => new MemoryStream(bytes));
            return source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
