using System;
using System.Linq;
using System.Threading.Tasks;
using HeiaMeg.Droid.Notifications;
using HeiaMeg.Services;
using HeiaMeg.Services.Native;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationService))]
namespace HeiaMeg.Droid.Notifications
{
    public class NotificationService : INotificationService
    {
        #region UpdateRoutine (singleton)

        private static readonly Lazy<NotificationService> _instance =
            new Lazy<NotificationService>(() => new NotificationService());

        #endregion

        public static NotificationService Instance => _instance.Value;

        private const int MAX_LOCAL_NOTIFICATIONS = 32; // Not really a maximum on Android, but most have a cap of 500 total

        public async Task ScheduleNotificationsAsync(int themeId)
        {
            var scheduled = (await StorageService.Instance.GetScheduledThemeMessagesAfterAsync(themeId, DateTime.Now)).Take(MAX_LOCAL_NOTIFICATIONS / Config.MaximumThemes).ToArray();

            foreach (var message in scheduled)
            {
                if (message.IsValid() && message.NotifyTime.HasValue)
                {
                    LocalNotification.Show(
                        title: NotificationUtils.NotificationTitle,
                        body: NotificationUtils.NotificationText(1),
                        id: message.Id,
                        notifyTime: message.NotifyTime.Value
                    );
                }
            }
        }

        public async Task UnscheduleNotificationsAsync(int themeId)
        {
            var scheduled = await StorageService.Instance.GetScheduledThemeMessagesAfterAsync(themeId, DateTime.Now);

            foreach (var message in scheduled)
            {
                LocalNotification.Cancel(message.Id);
            }
        }
    }
}