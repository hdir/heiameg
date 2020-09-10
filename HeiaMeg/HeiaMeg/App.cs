using System;
using System.Globalization;
using System.Threading.Tasks;
using HeiaMeg.Pages;
using HeiaMeg.Resources;
using HeiaMeg.Services;
using HeiaMeg.ViewModels;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using MainPage = HeiaMeg.Pages.MainPage;

namespace HeiaMeg
{
    public class App : Application
    {
        #region UpdateRoutine (singleton)

        private static readonly Lazy<UpdateRoutine> _updateRoutine =
            new Lazy<UpdateRoutine>(() => new UpdateRoutine());

        #endregion

        public static UpdateRoutine UpdateRoutine => _updateRoutine.Value;

        public App()
        {
            IsForeground = true;

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            MainPage = new MainPage();

            if (!UserSettings.IntroCompleted)
                MainPage.Navigation.PushModalAsync(new OnboardingPage(), false);
        }

        protected override async void OnStart()
        {
            base.OnStart();

            AppCenter.Start($"ios={Config.AnalyticsSecretIOS};" +
                            $"android={Config.AnalyticsSecretAndroid}",
                typeof(Analytics), typeof(Crashes));

            await AppCenter.SetEnabledAsync(UserSettings.HasAcceptedAnalytics);

            AttemptUpdateAsync();
        }

        protected override void OnSleep()
        {
            IsForeground = false;
            base.OnSleep();
        }

        protected override async void OnResume()
        {
            IsForeground = true;

            base.OnResume();

            if (NotificationId > 0)
            {
                await Shell.Current.GoToAsync($"//{Routes.Archive}");
                
                NotificationId = 0;
            }
            await MainViewModel.LoadMessagesAsync();

            await AttemptUpdateAsync();
        }

        private async Task AttemptUpdateAsync()
        {
            if ((DateTime.Now - UserSettings.LastUpdateDate).Days >= Config.UpdateRoutineIntervalDays)
            {
                if (await UpdateRoutine.RunAsyncTask() == Result.Completed)
                {
                    UserSettings.LastUpdateDate = DateTime.Now;
                    await MainViewModel.LoadMessagesAsync();
                }
            }
        }

        public static double ScreenWidth { get; set; }
        public static double ScreenHeight { get; set; }
        public static int NotificationId { get; set; }
        public static bool IsForeground { get; set; }
    }
}