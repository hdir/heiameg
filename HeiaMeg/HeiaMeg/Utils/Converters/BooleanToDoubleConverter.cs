using System;
using System.Globalization;
using Xamarin.Forms;

namespace HeiaMeg.Utils.Converters
{
    public class BooleanToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                value = false;

            if (!(value is bool boolean))
                return new ArgumentException("Value must be boolean");

            var trueValue = 1d;
            var falseValue = 0d;
            var str = parameter?.ToString();
            if (string.IsNullOrEmpty(str))
                return boolean ? trueValue : falseValue;

            var parameters = str.Split('|');
            if (parameters.Length > 0)
                if (!double.TryParse(parameters[0], NumberStyles.Any, CultureInfo.InvariantCulture, out falseValue))
                    falseValue = 0d;
            if (parameters.Length > 1)
                if (!double.TryParse(parameters[1], NumberStyles.Any, CultureInfo.InvariantCulture, out trueValue))
                    trueValue = 1d;

            return boolean ? trueValue : falseValue;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanToFontAttributeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                value = false;

            if (!(value is bool boolean))
                return new ArgumentException("Value must be boolean");

            return boolean ? FontAttributes.Bold : FontAttributes.None;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}