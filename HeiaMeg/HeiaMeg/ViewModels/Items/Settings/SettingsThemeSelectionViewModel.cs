using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HeiaMeg.Services;
using HeiaMeg.Utils.Analytics;
using HeiaMeg.ViewModels.Items.Theme;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;

namespace HeiaMeg.ViewModels.Items.Settings
{
    public class SettingsThemeSelectionViewModel : SettingsItemViewModel
    {
        public override string Title { get; } = "Endre tema";

        public ObservableCollection<ThemeViewModel> Themes { get; } =
            new ObservableCollection<ThemeViewModel>(
                ThemesManager.Themes
                    .Where(t => !t.IsDeactivated)
                    .Select(t => new ThemeViewModel(t))
                );

        public ICommand ItemTappedCommand { get; }

        public SettingsThemeSelectionViewModel(Command<SettingsItemViewModel> clicked) : base(clicked)
        {
            ItemTappedCommand = new Command<ThemeViewModel>(async (vm) =>
            {
                if (vm.IsSelected)
                {
                    if (await Shell.Current.DisplayAlert("", $"Ønsker du å skru av meldinger fra \n'{vm.Title}'", "ja",
                        "nei"))
                    {
                        vm.IsSelected = false;
                        ThemesManager.DisableTheme(vm.Model);
                        await Task.Run(async () => { await ScheduleManager.UnScheduleThemeAsync(vm.Model); });

                        // track enabled or disabled
                        Analytics.TrackEvent(TrackingEvents.Adjustment,
                            new TrackingEvents.AdjustmentArgs(vm.Model, false));
                    }
                }
                else if (SelectedCount < Config.MaximumThemes)
                {
                    vm.IsSelected = true;
                    ThemesManager.EnableTheme(vm.Model);
                    await Task.Run(async () => { await ScheduleManager.ScheduleThemeAsync(vm.Model); });
                    await MainViewModel.LoadMessagesAsync();

                    // track enabled or disabled
                    Analytics.TrackEvent(TrackingEvents.Adjustment,
                        new TrackingEvents.AdjustmentArgs(vm.Model, true));
                }
                else
                {
                    await Shell.Current.DisplayAlert("", "Sett deg maks to mål. Da gaper du ikke over for mye.", "ok");
                }
            });

            UpdateSelectedThemes();
        }

        public void UpdateSelectedThemes()
        {
            foreach (var theme in Themes) theme.IsSelected = ThemesManager.EnabledThemes.Contains(theme.Model);
        }

        public int SelectedCount => Themes.Count(m => m.IsSelected);
    }
}