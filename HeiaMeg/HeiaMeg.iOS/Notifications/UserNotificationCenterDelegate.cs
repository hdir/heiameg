using System;
using UIKit;
using UserNotifications;

namespace HeiaMeg.iOS.Notifications
{
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // when in foreground
            completionHandler(UNNotificationPresentationOptions.Alert);
        }

        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            // when user clicks notification 

            if (response.IsDefaultAction)
            {
                if (int.TryParse(response.Notification.Request.Identifier, out var id))
                {
                    App.NotificationId = id;
                    UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
                }
                // Handle default action...
            }
            else if (response.IsDismissAction)
            {
                // Handle dismiss action
            }

            // Inform caller it has been handled
            completionHandler();
        }
    }
}