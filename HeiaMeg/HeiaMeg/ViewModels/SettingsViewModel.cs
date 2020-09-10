using HeiaMeg.Utils.Analytics;
using HeiaMeg.ViewModels.Base;
using HeiaMeg.ViewModels.Items.Settings;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;

namespace HeiaMeg.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        public SettingsThemeSelectionViewModel ThemeSelectionSetting { get; }
        public SettingsAboutViewModel AboutSetting { get; }
        public SettingsTermsViewModel TermsSetting { get; }
        public SettingsFeedbackViewModel FeedbackSetting { get; }
        public SettingsDebugViewModel DebugSetting { get; }

        public SettingsItemViewModel[] SettingsViewModels { get; }

        private SettingsItemViewModel _selected;
        public SettingsItemViewModel Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                DetailViewTitle = _selected?.Title;
                RaisePropertyChanged();
            }
        }

        private bool _showDetails;
        public bool ShowDetails
        {
            get => _showDetails;
            set => SetProperty(ref _showDetails, value);
        }

        public string Username
        {
            get => UserSettings.UserName;
            set
            {
                UserSettings.UserName = value;
                RaisePropertyChanged();
            }
        }

        public bool HasAcceptedAnalytics
        {
            get => UserSettings.HasAcceptedAnalytics;
            set
            {
                if (UserSettings.HasAcceptedAnalytics == value)
                    return;

                Analytics.TrackEvent(TrackingEvents.AnalyticsAccepted, new TrackingEvents.AnalyticsAcceptedArgs(value));

                UserSettings.HasAcceptedAnalytics = value;
                RaisePropertyChanged();
            }
        }


        private string _detailViewTitle;
        public string DetailViewTitle
        {
            get => _detailViewTitle;
            set => SetProperty(ref _detailViewTitle, value);
        }

        public Command<SettingsItemViewModel> SelectedSettingCommand { get; }

        public SettingsViewModel()
        {
            SelectedSettingCommand = new Command<SettingsItemViewModel>(item =>
            {
                Selected = item;
                ShowDetails = true;

                Analytics.TrackEvent(TrackingEvents.NavigatedTo,
                    new TrackingEvents.NavigatedToArgs(item));
            });

            ThemeSelectionSetting = new SettingsThemeSelectionViewModel(SelectedSettingCommand);
            AboutSetting = new SettingsAboutViewModel(SelectedSettingCommand);
            TermsSetting = new SettingsTermsViewModel(SelectedSettingCommand);
            FeedbackSetting = new SettingsFeedbackViewModel(SelectedSettingCommand);
#if DEBUG
            DebugSetting = new SettingsDebugViewModel(new Command<SettingsItemViewModel>(model =>
            {
                Selected = DebugSetting;
                ShowDetails = true;
            }));
#endif

            SettingsViewModels = new SettingsItemViewModel[]
            {
                ThemeSelectionSetting,
                AboutSetting,
                TermsSetting,
                FeedbackSetting,
#if DEBUG
                DebugSetting,
#endif
        };

            OnboardingViewModel.OnboardingCompleted += () =>
            {
                RaisePropertyChanged(nameof(Username));
                RaisePropertyChanged(nameof(HasAcceptedAnalytics));
                ThemeSelectionSetting.UpdateSelectedThemes();
            };
        }
    }
}
