using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace HeiaMeg.Utils.Converters
{
    public class DateTimeToFriendlyDayTimeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException("The target must be a string");
            if (!(value is DateTime))
                throw new InvalidOperationException("The value must be a DateTime");

            var date = (DateTime)value;
            var diff = DateTime.Today - date.Date;

            var time = date.ToString("HH:mm", CultureInfo.InvariantCulture);

            if (diff.Days == 0)
                return $"I dag {time}";
            if (diff.Days == 1)
                return $"I går {time}";
            if (diff.Days < 7)
                return $"{date.DayOfWeek.ToStringNorwegian().ToUpperInvariant()} {time}";

            return date.ToString("dd/MM/yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class BooleanToReplaceStringMapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Dictionary<string, string>))
                throw new InvalidOperationException("The target must be a Dictionary<string, string>");
            if (!(value is bool boolean))
                throw new InvalidOperationException("The value must be a DateTime");

            try
            {
                var colors = parameter.ToString().Split('|');

                var trueColor = Color.FromHex(colors[0]);
                var falseColor = Color.FromHex(colors[1]);

                var color = boolean ? trueColor : falseColor;
                return ImageUtils.ReplacementDictionary(color);
            }
            catch (Exception e)
            {
#if DEBUG
                throw e;
#else
                return new Dictionary<string, string>();
#endif
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}