using System;
using System.Linq;
using System.Threading.Tasks;
using HeiaMeg.Models;
using HeiaMeg.Services.Native;
using HeiaMeg.Utils;
using HeiaMeg.ViewModels;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace HeiaMeg.Services
{
    public static class ScheduleManager
    {
        #region NotificationService

        private static readonly Lazy<INotificationService> _lazyNotificationService =
            new Lazy<INotificationService>(() => DependencyService.Get<INotificationService>());

        #endregion

        public static INotificationService NotificationService => _lazyNotificationService.Value;

        /// <summary>
        /// Schedules future message for theme depending on its frequency.
        /// NOTE: assumes all future messages are unscheduled.
        /// </summary>
        public static async Task<bool> ScheduleThemeAsync(Theme theme, DateTime? date = null)
        {
            var lastMessage = await StorageService.Instance.GetLastReleasedMessage(theme.Id);

            var welcomeMessage = await StorageService.Instance.GetWelcomeMessageAsync(theme.Id);
            if (lastMessage == null)
            {
                // important to do this first so consecutive doesn't include welcome message
                if (welcomeMessage != null)
                {
                    welcomeMessage.NotifyTime = date ?? DateTime.Now;
                    welcomeMessage.Opened = null;
                    welcomeMessage.FeedbackType = FeedbackType.None;

                    await StorageService.Instance.UpdateMessageAsync(welcomeMessage);
                }
                lastMessage = welcomeMessage;
            }

            var next = lastMessage?.NotifyTime?.Date.AddDays(1) ?? DateTime.Now;

            // fix: start from today if last message is old
            if (next < DateTime.Now)
                next = DateTime.Now;

            // get future consecutive that have not been notified
            var consecutive =
                (await StorageService.Instance.GetConsecutiveMessagesAfterAsync(theme.Id, next))
                .ToList();

            foreach (var message in consecutive)
            {
                var notifyTime = GetNotifiedTime(message, next);

                // fix: make sure consecutive day is in the future
                while (notifyTime <= DateTime.Now)
                    notifyTime = GetNotifiedTime(message, next.AddDays((int)theme.Frequency));

                message.NotifyTime = notifyTime;
                // set next day
                next = notifyTime.AddDays((int)theme.Frequency);
                //Debug.WriteLine($@"NEXT: {next:dd/MM HH:mm:ss ddd}");
            }

            // schedule date messages after consecutive is over
            var future =
                (await StorageService.Instance.GetThemeMessagesAfterAsync(theme.Id, next))
                .ToList();

            foreach (var msg in future)
            {
                msg.NotifyTime = GetNotifiedTime(msg);
            }

            // store messages
            try
            {
                await StorageService.Instance.UpdateMessagesAsync(consecutive);
                await StorageService.Instance.UpdateMessagesAsync(future);

                // Schedule notifications
                await NotificationService.ScheduleNotificationsAsync(theme.Id);
                return true;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
#if DEBUG
                Device.BeginInvokeOnMainThread(() => throw e);
#endif
                return false;
            }
        }

        /// <summary>
        /// Unschedules future message.
        /// </summary>
        public static async Task<bool> UnScheduleThemeAsync(Theme theme, DateTime? date = null)
        {
            // get scheduled messages in the future
            var messages = await StorageService.Instance.GetScheduledThemeMessagesAfterAsync(theme.Id, date ?? DateTime.Now);

            // Reset notifications
            await NotificationService.UnscheduleNotificationsAsync(theme.Id);

            foreach (var message in messages)
                message.NotifyTime = null;


            try
            {
                await StorageService.Instance.UpdateMessagesAsync(messages);
                return true;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
#if DEBUG
                Device.BeginInvokeOnMainThread(() => throw e);
#endif
                return false;
            }
        }

        private static DateTime GetNotifiedTime(IMessage message, DateTime fallback = default)
        {
            return TimeUtils.RandomTimeWithinTimeSpan(message.Date ?? fallback, message.From, message.To);
        }
    }
}
