using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Support.V7.App;

namespace HeiaMeg.Droid
{
    [Activity(
        Theme = "@style/SplashTheme",
        MainLauncher = true,
        NoHistory = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
    )]
    public class SplashScreen : AppCompatActivity
    {
        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            RunAppStart();
        }

        // Simulates background work that happens behind the splash screen
        private void RunAppStart()
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            OverridePendingTransition(0, 0);
        }

        public override void OnBackPressed() { }
    }
}