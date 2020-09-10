using System;
using System.Globalization;
using HeiaMeg.Services;
using Xamarin.Forms;

namespace HeiaMeg.Utils.Converters
{
    public class StatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Status status)
            {
                switch (status)
                {
                    case Status.Waiting:
                        return "-";
                    case Status.Running:
                        return "";
                    case Status.Failed:
                        return "\u2717";
                    case Status.Completed:
                        return "\u2713";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return new ArgumentException( $"Value must be of type {nameof(Status)}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}