using System;
using System.Threading.Tasks;
using HeiaMeg.Services;
using HeiaMeg.Utils;
using HeiaMeg.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeiaMeg.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            this.SetBinding(IsDetailPageVisibleProperty, nameof(SettingsViewModel.ShowDetails), BindingMode.TwoWay);


            NameEntry.Unfocused += (sender, args) =>
            {
                // if accessibility is enabled the namechange should be possible
                NameEntry.InputTransparent = !Accessibility.IsEnabled; 
                PenIcon.FadeTo(1, 100u);
            };
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            DetailView.TranslationX = width;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MasterView.TranslationX = 0;
            DetailView.TranslationX = MasterView.Width;
            IsDetailPageVisible = false;
        }

        private async void EditName_OnTapped(object sender, EventArgs e)
        {
            if (NameEntry.InputTransparent)
            {
                NameEntry.InputTransparent = false;
                await PenIcon.FadeTo(0, 100u);
            }
            NameEntry.Focus();
            NameEntry.CursorPosition = NameEntry.Text.Length;
        }

        private async void Clear_OnClicked(object sender, EventArgs e)
        {
            UserSettings.UserName = "";
            UserSettings.StartDate = DateTime.Now;
            UserSettings.LastUpdateDate = DateTime.MinValue;

            foreach (var theme in ThemesManager.EnabledThemes)
                ThemesManager.DisableTheme(theme);

            await StorageService.Instance.ClearMessagesAsync();
        }

        public static readonly BindableProperty IsDetailPageVisibleProperty =
            BindableProperty.Create(
                nameof(IsDetailPageVisible),
                typeof(bool),
                typeof(SettingsPage),
                default(bool),
                propertyChanged:
                (bindableObject, oldValue, newValue) =>
                {
                    ((SettingsPage) bindableObject).IsDetailPageVisiblePropertyChanged((bool) oldValue, (bool) newValue);
                }
            );

        private async void IsDetailPageVisiblePropertyChanged(bool oldValue, bool newValue)
        {
            if (!oldValue && newValue)
            {
                while (ViewIsBusy)
                    await Task.Delay(20);
                // animate in
#pragma warning disable 4014
                DetailView.TranslateTo(0, 0, easing: Easing.CubicInOut);
                MasterView.TranslateTo(-MasterView.Width, 0, easing: Easing.CubicInOut);
#pragma warning restore 4014
            }
            else if (oldValue && !newValue)
            {
                while (ViewIsBusy)
                    await Task.Delay(20);
                // animate out
#pragma warning disable 4014
                DetailView.TranslateTo(MasterView.Width, 0, easing: Easing.CubicInOut);
                MasterView.TranslateTo(0, 0, easing: Easing.CubicInOut);
#pragma warning restore 4014
            }
        }

        public bool ViewIsBusy { get; set; }

        public bool IsDetailPageVisible
        {
            get => (bool) GetValue(IsDetailPageVisibleProperty);
            set => SetValue(IsDetailPageVisibleProperty, value);
        }

        private void AnimateIn(object sender, EventArgs e)
        {
            IsDetailPageVisible = true;
        }

        private void AnimateOut(object sender, EventArgs e)
        {
            IsDetailPageVisible = false;
        }

        private void SwipeGestureRecognizer_OnSwiped(object sender, SwipedEventArgs e)
        {
            IsDetailPageVisible = false;
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
#if DEBUG
            if (BindingContext is SettingsViewModel vm)
            {
                vm.DebugSetting.Clicked.Execute(null);
            }
#endif
        }
    }
}