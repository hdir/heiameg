using System.Threading.Tasks;

namespace HeiaMeg.Services.Native
{
    public interface INotificationService
    {
        Task ScheduleNotificationsAsync(int themeId);
        Task UnscheduleNotificationsAsync(int themeId);
    }

    public static class NotificationUtils
    {
        public static string NotificationTitle => $"Heia {UserSettings.UserName}!";
        public static string NotificationText(int count = 1) => "Du har " +
                                                                $"{(count > 1 ? count.ToString() : "en")} ny{(count > 1 ? "e" : "")} " +
                                                                $"melding{(count > 1 ? "er" : "")}.";
    }
}
