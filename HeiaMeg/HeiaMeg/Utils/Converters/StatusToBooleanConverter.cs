using System;
using System.Globalization;
using HeiaMeg.Services;
using Xamarin.Forms;

namespace HeiaMeg.Utils.Converters
{
    public class StatusToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Status status))
                return new ArgumentException($"Value must be of type {nameof(Status)}");

            if (parameter is Status trueStatus)
                return status == trueStatus;
            return status == Status.Completed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}