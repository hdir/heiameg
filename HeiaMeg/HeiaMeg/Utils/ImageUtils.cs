using System.Collections.Generic;
using FFImageLoading.Svg.Forms;
using Xamarin.Forms;

namespace HeiaMeg.Utils
{
    public static class ImageUtils
    {
        public static SvgCachedImage ReplaceColor(this SvgCachedImage svg, Color color, string replaceTitle = "fill", string replaceText = "\"#000000\"")
        {
            svg.ReplaceStringMap = ReplacementDictionary(color, replaceTitle, replaceText);
            return svg;
        }

        public static SvgImageSource ReplaceColor(this SvgImageSource svg, Color color, string replaceTitle = "fill", string replaceText = "\"#000000\"")
        {
            svg.ReplaceStringMap = ReplacementDictionary(color, replaceTitle, replaceText);
            return svg;
        }

        public static Dictionary<string, string> ReplacementDictionary(Color color, string replaceTitle = "fill",
            string replaceText = "\"#000000\"")
        {
            return new Dictionary<string, string>
            {
                {
                    $"{replaceTitle}={replaceText}",
                    $"{replaceTitle}=\"{color.ToHex()}\""
                },
                {
                    $"stroke={replaceText}",
                    $"stroke=\"{color.ToHex()}\""
                },
            };
        }
    }
}
