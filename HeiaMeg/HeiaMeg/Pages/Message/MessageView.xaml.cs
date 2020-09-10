using System;
using System.Threading.Tasks;
using HeiaMeg.Utils;
using HeiaMeg.ViewControls;
using HeiaMeg.ViewModels.Items.Message;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeiaMeg.Pages.Message
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageView : ContentView
    {
        private static MessageView _currentOpenOptionsMessageView;

        private bool _isAnimatingOptions;

        public View TitleView
        {
            set => TitleViewContainer.Content = value;
        }

        public View MessageTextView
        {
            set => MessageViewContainer.Content = value;
        }

        public View UtilityView
        {
            set => UtilityModalViewContainer.Content = value;
        }

        public View BackgroundView
        {
            set => BackgroundViewContainer.Content = value;
        }

        public View OptionsButton
        {
            set => OptionsButtonView.Content = value;
        }

        public MessageView()
        {
            InitializeComponent();

            // Closes open options if container is touched
            this.AddTouch(async (sender, args) =>
            {
                await CloseOptions();
            });
        }

        private void Url_OnTapped(object sender, EventArgs e)
        {
            if (sender is View view)
            {
                view.Opacity = 0.6f;
            }
        }

        private async void OptionsButton_OnClicked(object sender, EventArgs e)
        {
            if (_isAnimatingOptions)
                return;

            _isAnimatingOptions = true;

            if (UtilityModalViewContainer.Content == null)
                await OpenOptions();
            else
                await CloseOptions();

            _isAnimatingOptions = false;
        }

        public async Task OpenOptions()
        {
#pragma warning disable 4014
            // close previous
            _currentOpenOptionsMessageView?.CloseOptions();

            _currentOpenOptionsMessageView = this;

            PlusButton.FadeTo(0, 100u);
            Cross.FadeTo(1f, 100);
            Cross.RotateTo(135, 350u, easing: Easing.SpringOut);
            Cross.ScaleTo(1.2, 250u, easing: Easing.SpringOut);
#pragma warning restore 4014
            var optionsView = new OptionsView
            {
                BindingContext = BindingContext,
                Tapped = async (sender, args) => await CloseOptions(true),
                ItemsSource = (BindingContext as MessageViewModel)?.Options,
            };

            UtilityModalViewContainer.Content = optionsView;
            await UtilityModalAnimation.AnimateInAsync();
        }

        public async Task CloseOptions(bool letAnimate = true)
        {
#pragma warning disable 4014
            PlusButton.FadeTo(1, 100u);
            Cross.FadeTo(0f, 100);
            Cross.RotateTo(0, 350u, easing: Easing.SpringOut);
            Cross.ScaleTo(1, easing: Easing.CubicOut);
#pragma warning restore 4014
            if (UtilityModalViewContainer?.Content != null)
            {
                if (letAnimate)
                {
                    await UtilityModalAnimation.AnimateOutAsync();
                }
                else
                {
#pragma warning disable 4014
                    UtilityModalAnimation.AnimateOutAsync();
#pragma warning restore 4014
                }
                UtilityModalViewContainer.Content = null;
            }

            if (_currentOpenOptionsMessageView == this)
                _currentOpenOptionsMessageView = null;
        }
    }

    public class MessageViewViewCell : ViewCell
    {
        public MessageView MessageView { get; set; }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            if (child is MessageView view)
                MessageView = view;
        }


        protected override void OnAppearing()
        {
            MessageView?.CloseOptions(false);
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            MessageView?.CloseOptions(false);
            base.OnDisappearing();
        }
    }
}