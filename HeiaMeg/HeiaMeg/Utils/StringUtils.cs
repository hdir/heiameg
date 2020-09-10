using HeiaMeg.Models;

namespace HeiaMeg.Utils
{
    public static class StringUtils
    {
        public const string Name = "{name}";
        public const string Day = "{day}";

        public static string ParseDynamicText(Message message)
        {
            var parsed = message.Text;
            parsed = parsed.Replace(Name, UserSettings.UserName);
            parsed = parsed.Replace(Day, message.NotifyTime?.DayOfWeek.ToStringNorwegian() ?? "");


            return parsed;
        }
    }
}
