using System;
using System.Globalization;
using HeiaMeg.Resources;
using HeiaMeg.Services;
using Xamarin.Forms;

namespace HeiaMeg.Utils.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Status status)
            {
                switch (status)
                {
                    case Status.Waiting:
                        return Colors.ModalForeground;
                    case Status.Running:
                        return Colors.ModalForeground;
                    case Status.Failed:
                        return Colors.ModalError;
                    case Status.Completed:
                        return Colors.ModalSuccess;
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