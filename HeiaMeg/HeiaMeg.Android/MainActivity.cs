using System;
using System.Globalization;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Views;
using Android.Views.Accessibility;
using FFImageLoading.Forms.Platform;
using HeiaMeg.Droid.Notifications;
using HeiaMeg.Droid.Services;
using HeiaMeg.Utils;
using HeiaMeg.Utils.Analytics;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using PanCardView.Droid;
using Plugin.StoreReview;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace HeiaMeg.Droid
{

    [Activity(
        Theme = "@style/MainTheme",
        LaunchMode = LaunchMode.SingleInstance,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait,
        WindowSoftInputMode = SoftInput.AdjustNothing
    )]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity, AccessibilityManager.ITouchExplorationStateChangeListener
    {
        public static Context Context;

        public MainActivity()
        {
            Context = this;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            App.ScreenHeight = (Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            App.ScreenWidth = (Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);

            var accessibilityManager = (AccessibilityManager)GetSystemService(AccessibilityService);
            //NOTE: ref: https://stackoverflow.com/a/12362545 we only use touch exploration events for checking if talkback is enabled
            accessibilityManager.AddTouchExplorationStateChangeListener(this);
            Accessibility.SetAccessibility(accessibilityManager.IsTouchExplorationEnabled, false);

            base.OnCreate(savedInstanceState);

            Forms.SetFlags("CollectionView_Experimental");

            Forms.Init(this, savedInstanceState);
            Platform.Init(this, savedInstanceState);
            CachedImageRenderer.Init(true);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            CardsViewRenderer.Preserve();

            var locale = new Java.Util.Locale(CultureInfo.InvariantCulture.Name);
            Java.Util.Locale.Default = locale;

            var config = new Android.Content.Res.Configuration { Locale = locale };
            Android.App.Application.Context.Resources.Configuration.SetTo(config);

            JobStarter.SetupBackgroundUpdateJob(this);

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        protected override void OnResume()
        {
            base.OnResume();

            // Cancel active notification with badges
            LocalNotification.Cancel(0);

            if (UserSettings.CanRequestReview())
                StartRequestReview();
        }

        private async void StartRequestReview()
        {
            UserSettings.HasRequestedReview = true;

            // wait a bit before requesting
            await Task.Delay(3000);
            try
            {
                var requestReview = await Shell.Current.DisplayAlert("",
                    "Fornøyd med appen? Vi vil gjerne ha din vurdering så vi kan bli bedre",
                    "Gi vurdering",
                    "Hopp over");

                if (requestReview)
                {
                    CrossStoreReview.Current.OpenStoreReviewPage(Application.PackageName);
                }

                Analytics.TrackEvent(TrackingEvents.RequestedReview,
                    new TrackingEvents.RequestedReviewArgs(requestReview));
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }

        public void OnTouchExplorationStateChanged(bool enabled)
        {
            Accessibility.SetAccessibility(enabled);
        }
    }
}