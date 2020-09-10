using System;
using System.Threading.Tasks;
using FFImageLoading.Forms.Platform;
using Foundation;
using HeiaMeg.iOS.Notifications;
using HeiaMeg.iOS.Services;
using HeiaMeg.Services;
using HeiaMeg.Utils;
using HeiaMeg.Utils.Analytics;
using HeiaMeg.ViewModels;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using PanCardView.iOS;
using StoreKit;
using UIKit;
using UserNotifications;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace HeiaMeg.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            App.ScreenHeight = UIScreen.MainScreen.Bounds.Height;
            App.ScreenWidth = UIScreen.MainScreen.Bounds.Width;

            Forms.SetFlags("CollectionView_Experimental");
            Forms.Init();
            CachedImageRenderer.Init();
            CardsViewRenderer.Preserve();

            Rg.Plugins.Popup.Popup.Init();

            Accessibility.SetAccessibility(UIAccessibility.IsVoiceOverRunning, false);
            NSNotificationCenter.DefaultCenter.AddObserver(UIView.VoiceOverStatusDidChangeNotification, notification =>
            {
                Accessibility.SetAccessibility(UIAccessibility.IsVoiceOverRunning);
            });

            LoadApplication(new App());

            UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

            // start background task
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(Config.DownloadInterval / 1000f);

            // reset our badge
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

            CheckPermissions();

            return base.FinishedLaunching(app, options);
        }

        public override void WillEnterForeground(UIApplication uiApplication)
        {
            base.WillEnterForeground(uiApplication);

            if (UserSettings.CanRequestReview())
                StartRequestReview();

            // reset our badge
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

        }

        private void CheckPermissions()
        {
            if (UserSettings.IntroCompleted)
                Permissions.RequestPermissions();
            else
                OnboardingViewModel.OnboardingCompleted += OnboardingCompleted;
        }

        private static void OnboardingCompleted()
        {
            Permissions.RequestPermissions();
            OnboardingViewModel.OnboardingCompleted -= OnboardingCompleted;
        }

        public override async void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            var didUpdate = false;
            try
            {
                if ((DateTime.Now - UserSettings.LastUpdateDate).Days >= Config.UpdateRoutineIntervalDays)
                {
                    if (await App.UpdateRoutine.RunAsyncTask() == Result.Completed)
                    {
                        UserSettings.LastUpdateDate = DateTime.Now;
                        didUpdate = true;
                    }

                    foreach (var theme in ThemesManager.EnabledThemes)
                    {
                        await NotificationService.Instance.ScheduleNotificationsAsync(theme.Id);
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
#if DEBUG
                Device.BeginInvokeOnMainThread(() => throw e);
#endif
            }

            completionHandler(didUpdate ? UIBackgroundFetchResult.NewData : UIBackgroundFetchResult.NoData);
        }
        private async void StartRequestReview()
        {
            UserSettings.HasRequestedReview = true;

            // wait a bit before requesting
            await Task.Delay(2000);

            var requestReview = false;

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 3))
            {
                // In-App request
                SKStoreReviewController.RequestReview();
            }
            else
            {
                //NOTE: untested behaviour is omitted

                //requestReview = await UserDialogs.Instance.ConfirmAsync(
                //    message: "Fornøyd med appen? Vi vil gjerne ha din vurdering så vi kan bli bedre",
                //    okText: "Gi vurdering",
                //    cancelText: "Hopp over");

                //if (requestReview)
                //{
                //    if (UIDevice.CurrentDevice.CheckSystemVersion(10, 3))
                //    {
                //        SKStoreReviewController.RequestReview();
                //    }
                //    CrossStoreReview.Current.OpenStoreReviewPage("1446753385");
                //}
            }

            Analytics.TrackEvent(TrackingEvents.RequestedReview,
                new TrackingEvents.RequestedReviewArgs(requestReview));
        }
    }
}
