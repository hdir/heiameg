using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HeiaMeg.Services;
using HeiaMeg.Utils.Analytics;
using HeiaMeg.ViewModels.Base;
using HeiaMeg.ViewModels.Items.Onboarding;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace HeiaMeg.ViewModels
{
    public class OnboardingViewModel : ViewModel
    {
        private static readonly OnboardingWelcomeViewModel _welcomeView = new OnboardingWelcomeViewModel();
        private static readonly OnboardingThemeSelectionViewModel _selectionView = new OnboardingThemeSelectionViewModel();
        private static readonly OnboardingTermsViewModel _termsView = new OnboardingTermsViewModel();

        public static event Action OnboardingCompleted;

        public OnboardingItemViewModel[] Pages { get; } = 
        {
            _welcomeView,
            _selectionView,
            _termsView, 
        };

        public ICommand NextCommand { get; }

        private OnboardingItemViewModel _currentPage;
        public OnboardingItemViewModel CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        private bool _buttonIsValid;

        public bool ButtonIsValid
        {
            get => _buttonIsValid;
            set => SetProperty(ref _buttonIsValid, value);
        }

        public OnboardingViewModel()
        {
            NextCommand = new Command(() =>
            {
                var i = Pages.IndexOf(CurrentPage);
                SetPage(i + 1);
            }, () => ButtonIsValid);

            SetPage(0);

            Analytics.TrackEvent(TrackingEvents.Onboarding,
                new TrackingEvents.OnboardingArgs(TrackingEvents.OnboardingEvents.Started));
        }

        public void SetPage(int index)
        {
            if (index < 0)
                return;
            if (index >= Pages.Length)
            {
                FinishOnboarding();
            }
            else
            {
                Analytics.TrackEvent(TrackingEvents.Onboarding,
                    new TrackingEvents.OnboardingArgs(TrackingEvents.OnboardingEvents.Carousel, index));

                if (CurrentPage != null)
                    CurrentPage.IsValidChanged -= ButtonValidityChanged;
                CurrentPage = Pages[index];
                CurrentPage.IsValidChanged += ButtonValidityChanged;
                ButtonIsValid = CurrentPage.IsValid;
            }
        }

        private async void FinishOnboarding()
        {
            Analytics.TrackEvent(TrackingEvents.Onboarding,
                new TrackingEvents.OnboardingArgs(TrackingEvents.OnboardingEvents.Completed));

            UserSettings.UserName = _welcomeView.Name;
            UserSettings.StartDate = DateTime.Now;

            var selected = _selectionView.Themes.Where(m => m.IsSelected).Select(m => m.Model).ToList();
            foreach (var theme in selected)
            {
                ThemesManager.EnableTheme(theme);
                // Track selected
                Analytics.TrackEvent(TrackingEvents.Onboarding,
                    new TrackingEvents.OnboardingArgs(theme));
            }
            // Track combination
            if (selected.Count > 1)
                Analytics.TrackEvent(TrackingEvents.Onboarding, new TrackingEvents.OnboardingArgs(selected));

            // Set analytics
            UserSettings.HasAcceptedAnalytics = _termsView.IsStatisticsAccepted;
            await Analytics.SetEnabledAsync(_termsView.IsStatisticsAccepted);

            while (Application.Current.MainPage.Navigation.ModalStack.Count > 0)
                await Application.Current.MainPage.Navigation.PopModalAsync(false);

            await Task.Run(async () =>
            {
                // schedule async
                foreach (var theme in selected)
                {
                    await ScheduleManager.ScheduleThemeAsync(theme);
                }
                await MainViewModel.LoadMessagesAsync();
            });

            OnboardingCompleted?.Invoke();
        }

        private void ButtonValidityChanged()
        {
            ButtonIsValid = CurrentPage?.IsValid ?? true;
        }
    }
}
