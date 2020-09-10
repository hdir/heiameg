using System;
using Android.App;
using Android.Content;
using HeiaMeg.Droid.Notifications;
using HeiaMeg.Services;
#if !DEBUG
using Microsoft.AppCenter.Crashes;
#endif

namespace HeiaMeg.Droid.Receivers
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[]
    {
        Intent.ActionBootCompleted,
        Intent.ActionLockedBootCompleted,
        "android.intent.action.QUICKBOOT_POWERON",
        "com.htc.intent.action.QUICKBOOT_POWERON" //HTC
    })]
    [MetaData("bootType", Value = "restart")]
    [MetaData("sendToBack", Value = "true")]
    public class BootReceiver : BroadcastReceiver
    {
        // BootReceiver makes sure receives an event after the phone has finished booting up

        public override async void OnReceive(Context context, Intent intent)
        {
            try
            {
                foreach (var theme in ThemesManager.EnabledThemes)
                {
                    await NotificationService.Instance.ScheduleNotificationsAsync(theme.Id);
                }
                await ScheduledAlarmHandler.ShowNotificationWithUnreadMessagesAsync();
            }
            catch (Exception e)
            {
#if DEBUG
                throw e;
#else
                Crashes.TrackError(e);
#endif
            }

        }
    }
}