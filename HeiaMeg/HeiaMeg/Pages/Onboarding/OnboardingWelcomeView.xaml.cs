using System;
using System.Threading.Tasks;
using FFImageLoading.Svg.Forms;
using HeiaMeg.Resources;
using HeiaMeg.Utils;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeiaMeg.Pages.Onboarding
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnboardingWelcomeView : ContentView
    {
        public OnboardingWelcomeView()
        {
            InitializeComponent();

            //BUG FIX https://github.com/xamarin/xamarin-android/issues/3096
            //BUG FIX https://github.com/luberda-molinet/FFImageLoading/issues/1325
            //TODO: remove with release of fix
            if (DeviceInfo.Version.Major >= 6)
            {
                IconView.Content = new SvgCachedImage()
                {
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                    Aspect = Aspect.AspectFit,
                    Source = Images.TabTopIconDefault,
                };
            }
        }

        private async void VisualElement_OnFocusChangeRequested(object sender, FocusRequestArgs e)
        {
            if (e.Focus)
                await FocusNameInput();
            else
                await UnfocusNameInput();
        }

        private async Task FocusNameInput()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                await Task.WhenAll(ContentStackLayout.TranslateTo(0, -50, easing: Easing.CubicInOut));
            }
            else
            {
                var keyboardHeight = App.ScreenHeight * 0.45f;
                var target = Height - keyboardHeight;

                var yOffset = target - Math.Min(EntryView.GetPosition().Y + EntryContainer.Height, Height);

                await Task.WhenAll(TextContainer.FadeTo(0),
                    EntryContainer.TranslateTo(0, yOffset, easing: Easing.CubicInOut));
            }
        }

        private async Task UnfocusNameInput()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                await Task.WhenAll(ContentStackLayout.TranslateTo(0, 0, easing: Easing.CubicInOut));
            }
            else
            {
                await EntryContainer.TranslateTo(0, 0, easing: Easing.CubicInOut);
                await TextContainer.FadeTo(1);
            }
        }

        private async void EntryView_OnFocused(object sender, FocusEventArgs e)
        {
            await FocusNameInput();
        }

        private async void EntryView_OnUnfocused(object sender, FocusEventArgs e)
        {
            await UnfocusNameInput();
        }

        private async void EntryView_OnCompleted(object sender, EventArgs e)
        {
            await UnfocusNameInput();
            var parent = Parent;
            while (parent != null)
            {
                if (parent is OnboardingPage page)
                {
                    page.NextButton_OnClicked(this, null);
                    break;
                }

                parent = parent.Parent;
            }
        }

        private void EntryLabel_OnTapped(object sender, EventArgs e)
        {
            EntryView.Focus();
        }
    }
}