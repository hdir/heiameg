using Foundation;
using UIKit;
using UserNotifications;

namespace HeiaMeg.iOS.Services
{
    public static class Permissions
    {
        public static void RequestPermissions()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Ask the user for permission to get notifications on iOS 10.0+
                UNUserNotificationCenter.Current.RequestAuthorization(
                    UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                    (approved, error) =>
                    {
                        if (!approved)
                        {
                            //NOTE: don't handle this - user was given the option and declined
                        }
                    });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                // Ask the user for permission to get notifications on iOS 8.0+
                var settings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                    new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);

                // Get current notification settings
                UNUserNotificationCenter.Current.GetNotificationSettings((s) => {
                    var alertsAllowed = (s.AlertSetting == UNNotificationSetting.Enabled);
                    if (!alertsAllowed)
                    {
                        //NOTE: don't handle this - user was given the option and declined
                    }
                });
            }
        }
    }
}