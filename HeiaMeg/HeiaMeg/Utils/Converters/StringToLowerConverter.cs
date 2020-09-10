using System;
using System.Globalization;
using Xamarin.Forms;

namespace HeiaMeg.Utils.Converters
{
    public class StringToLowerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string text))
                return new ArgumentException("Value must be string");
            if (targetType != typeof(string))
                return new ArgumentException("Target must be string");

            return text.ToLowerInvariant();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}