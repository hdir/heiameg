using System;
using System.Globalization;
using Xamarin.Forms;

namespace HeiaMeg.Utils.Converters
{
    public class StringToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case string text:
                    return !string.IsNullOrEmpty(text);
                case null:
                    return false;
                default:
                    return new ArgumentException("Value must be string");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}