using System;
using HeiaMeg.Pages.Modals;
using HeiaMeg.Utils.Analytics;
using Microsoft.AppCenter.Analytics;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeiaMeg.Pages.Onboarding
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnboardingTermsView : ContentView
    {
        public OnboardingTermsView()
        {
            InitializeComponent();
        }

        private void ShowTermsPopup()
        {
            PopupNavigation.Instance.PushAsync(new TermsPopupPage());

            Analytics.TrackEvent(TrackingEvents.ItemTapped,
                new TrackingEvents.ItemTappedArgs(TrackingEvents.ItemsToTap.Terms));
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            ShowTermsPopup();
        }
    }
}