using System;
using System.Threading.Tasks;
using HeiaMeg.Resources;
using HeiaMeg.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeiaMeg.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnboardingPage : ContentPage
    {
        public OnboardingPage()
        {
            Resources = Styles.OnboardingStyle;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await Task.Delay(1000);
            await NextButton.TranslateTo(0, 0, easing: Easing.SpringOut);
        }

        private bool _isButtonBusy;
        public async void NextButton_OnClicked(object sender, EventArgs e)
        {
            if (_isButtonBusy || !NextButton.IsEnabled)
                return;

            _isButtonBusy = true;

            if (BindingContext is OnboardingViewModel vm)
            {
                if (!vm.NextCommand.CanExecute(null))
                    return;

                // fade out
#pragma warning disable 4014
                NextButton.TranslateTo(0, 100, easing: Easing.SpringIn);
#pragma warning restore 4014
                await OnboardingView.FadeTo(0);
                // execute
                vm.NextCommand.Execute(null);
                // fade in
                await OnboardingView.FadeTo(1);
                await NextButton.TranslateTo(0, 0, easing: Easing.SpringOut);
            }
            else
            {
#pragma warning disable 4014
                NextButton.TranslateTo(0, 100);
#pragma warning restore 4014
                await OnboardingView.FadeTo(0);
                await OnboardingView.FadeTo(1);
                await NextButton.TranslateTo(0, 0, easing: Easing.SpringOut);
            }
            _isButtonBusy = false;
        }

        protected override bool OnBackButtonPressed()
        {
            //return base.OnBackButtonPressed();
            return true;
        }
    }
}