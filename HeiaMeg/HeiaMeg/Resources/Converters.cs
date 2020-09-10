using HeiaMeg.Utils.Converters;

namespace HeiaMeg.Resources
{
    public static class Converters
    {
        public static StringToUpperConverter StringToUpperConverter { get; } = new StringToUpperConverter();
        public static StringToBooleanConverter StringToBooleanConverter { get; } = new StringToBooleanConverter();

        public static DateTimeToFriendlyDayTimeStringConverter DateTimeToFriendlyDayTimeStringConverter { get; } = new DateTimeToFriendlyDayTimeStringConverter();

        public static InvertedBooleanConverter InvertedBooleanConverter { get; } = new InvertedBooleanConverter();

        public static BooleanToDoubleConverter BooleanToDoubleConverter { get; } = new BooleanToDoubleConverter();
        public static BooleanToFontAttributeConverter BooleanToFontAttributeConverter { get; } = new BooleanToFontAttributeConverter();

        public static HyperlinkFormattedTextConverter HyperlinkFormattedTextConverter { get; } = new HyperlinkFormattedTextConverter(Colors.Hyperlink);

    }
}
