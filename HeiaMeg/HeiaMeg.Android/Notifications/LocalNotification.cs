using System;
using System.IO;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using TaskStackBuilder = Android.App.TaskStackBuilder;

namespace HeiaMeg.Droid.Notifications
{
    public static class LocalNotification
    {
        private static NotificationManager _notificationManager;
        private static AlarmManager _alarmManager;

        private static NotificationManager NotificationManager => _notificationManager ?? (_notificationManager = Application.Context.GetSystemService(Context.NotificationService) as NotificationManager);
        private static AlarmManager AlarmManager => _alarmManager ?? (_alarmManager = Application.Context.GetSystemService(Context.AlarmService) as AlarmManager);

        private static string PackageName => Application.Context.PackageName;
        private static string ChannelId => $"{PackageName}.default";

        private static readonly long[] VibrationPattern = { 250, 250, 250, 250 };

        public const string MESSAGE_KEY = "messageId";

        /// <summary>
        /// Show a local notification
        /// </summary>
        /// <param name="title">Title of the notification</param>
        /// <param name="body">Body or description of the notification</param>
        /// <param name="badgecount">Number to be displayed on badge</param>
        /// <param name="notificationId">Id of the notification</param>
        /// <param name="messageId">Unique Id of top most message</param>
        public static void Show(string title, string body, int badgecount = 1, int notificationId = 0, int messageId = 0)
        {
            var large = BitmapFactory.DecodeResource(Application.Context.Resources, Resource.Mipmap.ic_launcher);

#pragma warning disable 618
            var builder = new NotificationCompat.Builder(Application.Context)
                .SetContentTitle(title)
                .SetContentText(body)
                .SetAutoCancel(true)
                .SetLargeIcon(large)
                .SetNumber(badgecount)
                .SetSmallIcon(Resource.Drawable.small_icon);
#pragma warning restore 618

            // HeadsUp Notification for API 26+
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(ChannelId, "Default", NotificationImportance.High);
                channel.EnableLights(true);
                channel.LightColor = Color.Pink;
                channel.EnableVibration(true);
                channel.SetVibrationPattern(VibrationPattern);
                channel.SetShowBadge(true);
                channel.LockscreenVisibility = NotificationVisibility.Public;

                NotificationManager.CreateNotificationChannel(channel);

                builder.SetChannelId(ChannelId);
            }
            // HeadsUp Notification for API 21+
            else if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
#pragma warning disable 618
                builder.SetPriority((int)NotificationPriority.High);
                builder.SetVibrate(VibrationPattern);
#pragma warning restore 618
            }

            var valuesForActivity = new Bundle();
            valuesForActivity.PutInt(MESSAGE_KEY, messageId);

            // tap action
            var resultIntent = LauncherActivity;
            resultIntent.PutExtras(valuesForActivity);
            resultIntent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);

            var stackBuilder = TaskStackBuilder.Create(Application.Context);
            stackBuilder.AddNextIntent(resultIntent);
            var resultPendingIntent = stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

            builder.SetContentIntent(resultPendingIntent);

            NotificationManager.Notify(notificationId, builder.Build());
        }

        public static Intent LauncherActivity => Application.Context.PackageManager.GetLaunchIntentForPackage(Application.Context.PackageName);

        /// <summary>
        /// Show a local notification at a specified time
        /// </summary>
        /// <param name="title">Title of the notification</param>
        /// <param name="body">Body or description of the notification</param>
        /// <param name="id">Id of the notification</param>
        /// <param name="notifyTime">Time to show notification</param>
        public static void Show(string title, string body, int id, DateTime notifyTime)
        {
            var intent = CreateIntent(id);

            var localNotification = new AndroidNotification
            {
                Title = title,
                Body = body,
                Id = id,
                NotifyTime = notifyTime,
            };

            var serializedNotification = SerializeNotification(localNotification);
            intent.PutExtra(ScheduledAlarmHandler.LocalNotificationKey, serializedNotification);

            var pendingIntent = PendingIntent.GetBroadcast(Application.Context, id, intent, PendingIntentFlags.UpdateCurrent);
            var triggerTime = NotifyTimeInMilliseconds(localNotification.NotifyTime);

            AlarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
        }

        /// <summary>
        /// Cancel a local notification
        /// </summary>
        /// <param name="id">Id of the notification to cancel</param>
        public static void Cancel(int id)
        {
            var intent = CreateIntent(id);
            var pendingIntent = PendingIntent.GetBroadcast(Application.Context, id, intent, PendingIntentFlags.UpdateCurrent);
            pendingIntent.Cancel();

            AlarmManager.Cancel(pendingIntent);
            pendingIntent.Cancel();

            NotificationManager.Cancel(id);
        }

        private static Intent CreateIntent(int id)
        {
            return new Intent(Application.Context, typeof(ScheduledAlarmHandler))
                .SetAction("LocalNotifierIntent" + id);
        }

        private static string SerializeNotification(AndroidNotification notification)
        {
            var xmlSerializer = new XmlSerializer(notification.GetType());
            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, notification);
                return stringWriter.ToString();
            }
        }

        private static long NotifyTimeInMilliseconds(DateTime notifyTime)
        {
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
            var epochDifference = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;

            var utcAlarmTimeInMillis = utcTime.AddSeconds(-epochDifference).Ticks / 10000;
            return utcAlarmTimeInMillis;
        }
    }
}