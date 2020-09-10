using System;
using System.Globalization;
using System.Text.RegularExpressions;
using HeiaMeg.Resources;
using Xamarin.Forms;

namespace HeiaMeg.Utils.Converters
{
    public class HyperlinkFormattedTextConverter : IValueConverter
    {
        public const string REGEX_URL = @"\b(?:https?://|www\.)\S+\b";

        private readonly Color _hyperlinkColor = Color.Blue;

        public HyperlinkFormattedTextConverter() { }

        public HyperlinkFormattedTextConverter(Color color)
        {
            _hyperlinkColor = color;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string text))
                return new ArgumentException("Value must be string");

            if (targetType != typeof(FormattedString))
                return new ArgumentException("Target must be FormattedString");

            return ParseHtmlAnchors(text, _hyperlinkColor);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static FormattedString ParseHtmlAnchors(string text, Color linkColor)
        {
            var formatted = new FormattedString();

            SplitUrlSpans(text, linkColor, ref formatted);

            return formatted;
        }

        private static void SplitUrlSpans(string text, Color linkColor, ref FormattedString fs)
        {
            while (true)
            {
                var match = Regex.Match(text, REGEX_URL);

                if (match.Success)
                {
                    var i = text.IndexOf(match.Value);

                    var url = match.Value;
                    var pre = text.Substring(0, i);
                    var post = text.Substring(i + url.Length, text.Length - (i + url.Length));

                    var spanPre = new Span() { Text = pre, FontSize = Sizes.TextSmall};

                    var tgr = new TapGestureRecognizer();
                    tgr.Tapped += (sender, args) => { Device.OpenUri(new Uri(url)); };

                    var spanLink = new Span()
                    {
                        Text = url,
                        TextDecorations = TextDecorations.Underline,
                        TextColor = linkColor,
                        GestureRecognizers = { tgr },
                        FontSize = Sizes.TextSmall
                    };

                    fs.Spans.Add(spanPre);
                    fs.Spans.Add(spanLink);

                    text = post;
                    continue;
                }

                fs.Spans.Add(new Span() { Text = text, FontSize = Sizes.TextSmall });
                break;
            }
        }
    }
}
