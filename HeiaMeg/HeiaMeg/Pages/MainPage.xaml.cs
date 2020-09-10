using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using HeiaMeg.Pages.Modals;
using HeiaMeg.Resources;
using HeiaMeg.Services;
using HeiaMeg.Utils.Analytics;
using Microsoft.AppCenter.Analytics;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HeiaMeg.Pages
{
    [DesignTimeVisible(false)]
    public partial class MainPage : Shell
    {
        private static readonly UpdatingPage UpdatingPage = new UpdatingPage();

        public MainPage()
        {
            Resources = Styles.MainStyle;
            InitializeComponent();

            App.UpdateRoutine.StatusChanged += async status =>
            {
                switch (status)
                {
                    case Status.Waiting:
                        break;
                    case Status.Running:
                        //await OpenUpdatePage();
                        break;
                    case Status.Failed:
                        if (!PopupNavigation.Instance.PopupStack.Contains(UpdatingPage))
                        {
                            // Display if over maximum days since previously downloaded
                            if (UpdatePageShouldBePresentedToUser)
                            {
                                await OpenUpdatePage();
                            }
                        }
                        break;
                    case Status.Completed:
                        await CloseUpdatePage();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(status), status, null);
                }
            };
            // if it's already started
            //if (App.UpdateRoutine.Status == Status.Running)
            //    OpenUpdatePage().GetAwaiter().GetResult();
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);

            Analytics.TrackEvent(TrackingEvents.NavigatedTo,
                new TrackingEvents.NavigatedToArgs(args.Current.Location.OriginalString));


            if (Device.RuntimePlatform == Device.Android && DeviceInfo.Version.Major < 6)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public static async Task OpenUpdatePage()
        {
            if(PopupNavigation.Instance.PopupStack.Contains(UpdatingPage))
                return;
            
            // Should only be visible if needed to inform the user

            await PopupNavigation.Instance.PushAsync(UpdatingPage, true);

            // close if completed while opening
            if (App.UpdateRoutine.Status == Status.Completed)
                await CloseUpdatePage();
        }

        public static async Task CloseUpdatePage()
        {
            await Task.Delay(1000); // allow animation to complete

            if (PopupNavigation.Instance.PopupStack.Contains(UpdatingPage))
                await PopupNavigation.Instance.PopAllAsync(true);
        }

        protected override bool OnBackButtonPressed()
        {
            if (CurrentItem.CurrentItem is Tab tab)
            {
                if (tab.CurrentItem.Content is SettingsPage settings)
                {
                    if (settings.IsDetailPageVisible)
                    {
                        settings.IsDetailPageVisible = false;
                        return true;
                    }
                }
            }
            return base.OnBackButtonPressed();
        }

        public bool UpdatePageShouldBePresentedToUser => 
            (DateTime.Now - UserSettings.LastUpdateDate).TotalDays >= Config.UpdateRoutineMaximumIntervalDays;
    }
}

