using System;
using System.Globalization;
using Xamarin.Forms;

namespace HeiaMeg.Utils.Converters
{
    public class BooleanToBoldConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(FontAttributes))
                return new ArgumentException("Target must be FontAttributes");

            if (!(value is bool boolean))
                return new ArgumentException("value must be bool");

            if (bool.TryParse(parameter?.ToString(), out var inverted) && inverted)
                return boolean ? FontAttributes.None : FontAttributes.Bold;

            return boolean ? FontAttributes.Bold : FontAttributes.None;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}