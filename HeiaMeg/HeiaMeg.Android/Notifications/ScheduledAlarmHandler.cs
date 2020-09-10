using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Android.Content;
using HeiaMeg.Services;
using HeiaMeg.Services.Native;
using Microsoft.AppCenter.Crashes;

namespace HeiaMeg.Droid.Notifications
{
    /// <summary>
    /// Broadcast receiver
    /// </summary>
    [BroadcastReceiver(Enabled = true, Label = "Local Notifications BroadcastReceiver")]
    public class ScheduledAlarmHandler : BroadcastReceiver
    {
        public const string LocalNotificationKey = "LocalNotification";

        public override async void OnReceive(Context context, Intent intent)
        {
            // gets a scheduled notification and displays an instant "combined" notification.
            var extra = intent.GetStringExtra(LocalNotificationKey);

            if (!string.IsNullOrEmpty(extra))
            {
                var notification = DeserializeNotification(extra);

                if (notification != null)
                {
                    if (await ShowNotificationWithUnreadMessagesAsync())
                    {
                        var id = notification.Id;
                        if (id > 0)
                            App.NotificationId = id;
                    }
                }
            }
        }

        public static async Task<bool> ShowNotificationWithUnreadMessagesAsync()
        {
            // counts how many unread messages
            try
            {
                var messages = await StorageService.Instance.GetUnreadMessagesBeforeAsync(DateTime.Now);

                if (messages.Count > 0)
                {
                    // send new instant notification with message ID of last message so last message is opened
                    LocalNotification.Show(
                        title: NotificationUtils.NotificationTitle,
                        body: NotificationUtils.NotificationText(messages.Count),
                        badgecount: messages.Count,
                        notificationId: 0
                    );

                    return true;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }

            return false;
        }

        private static AndroidNotification DeserializeNotification(string notificationString)
        {
            var xmlSerializer = new XmlSerializer(typeof(AndroidNotification));
            using (var stringReader = new StringReader(notificationString))
            {
                var notification = (AndroidNotification)xmlSerializer.Deserialize(stringReader);
                return notification;
            }
        }
    }
}