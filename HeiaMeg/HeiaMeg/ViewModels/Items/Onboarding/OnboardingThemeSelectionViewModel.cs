using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using HeiaMeg.Services;
using HeiaMeg.ViewModels.Items.Theme;
using Xamarin.Forms;

namespace HeiaMeg.ViewModels.Items.Onboarding
{
    public class OnboardingThemeSelectionViewModel : OnboardingItemViewModel, IThemeSelection
    {
        public ObservableCollection<ThemeViewModel> Themes { get; } =
            new ObservableCollection<ThemeViewModel>(ThemesManager.Themes
                .Where(t => !t.IsDeactivated)
                .Select(t => new ThemeViewModel(t))
            );

        public ICommand ItemTappedCommand { get; }

        private readonly List<ThemeViewModel> _selectionQueue = new List<ThemeViewModel>(Config.MaximumThemes);

        public OnboardingThemeSelectionViewModel()
        {
            ItemTappedCommand = new Command<ThemeViewModel>((vm) =>
            {
                if (vm.IsSelected)
                {
                    _selectionQueue.Remove(vm);
                    vm.IsSelected = false;
                }
                else if (SelectedCount < Config.MaximumThemes)
                {
                    vm.IsSelected = true;
                    _selectionQueue.Add(vm);
                }
                else
                {
                    // Untoggle first selection
                    if (_selectionQueue.Any())
                    {
                        var old = _selectionQueue.First();
                        old.IsSelected = false;
                        _selectionQueue.Remove(old);
                    }

                    // Select current
                    vm.IsSelected = true;
                    _selectionQueue.Add(vm);
                }

                IsValid = SelectedCount >= Config.MinimumThemes;
            });
        }

        public int SelectedCount => Themes.Count(m => m.IsSelected);
    }
}