using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HeiaMeg.Services;
using HeiaMeg.ViewModels.Base;
using HeiaMeg.ViewModels.Items.Message;
using Xamarin.Forms;

namespace HeiaMeg.ViewModels
{
    public class ArchiveViewModel : ViewModel
    {
        #region Properties

        public static readonly TabItem NewTab = new TabItem("Nye") {Filter = model => !model.IsOpened};

        public ObservableCollection<TabItem> Tabs { get; } = new ObservableCollection<TabItem>
        {
            NewTab,
        };

        private TabItem _selectedTab;
        public TabItem SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (_selectedTab == value)
                    return;

                CurrentFilter = value.Filter;

                foreach (var tab in Tabs)
                    tab.IsCurrent = tab == value;

                var old = _selectedTab;
                SetProperty(ref _selectedTab, value);
                OnSelectedTabChanged(old, value);
            }
        }

        public bool ShowEmptyNewMessages => SelectedTab == NewTab && !MainViewModel.Messages.Where(NewTab.Filter).Any();

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private Func<MessageViewModel, bool> _currentFilter;
        public Func<MessageViewModel, bool> CurrentFilter
        {
            get => _currentFilter;
            set => SetProperty(ref _currentFilter, value);
        }

        public ICommand RefreshCommand { get;  }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }


        #endregion

        public ArchiveViewModel()
        {
            SelectedTab = Tabs.First();

            RefreshCommand = new Command(async () =>
            {
                IsRefreshing = true;
                await MainViewModel.LoadMessagesAsync();
                await Task.Delay(500); // add some more delay for dramatic effect
                IsRefreshing = false;

                NewTab.BadgeCount = 0; // set badges to 0 after refreshing
            });

            IsBusy = !MainViewModel.Messages.Any();
            if (IsBusy && UserSettings.IntroCompleted)
                StartTimeOutAsync(5000);

            MainViewModel.Messages.CollectionChanged += (sender, args) =>
            {
                // Tabs should display available themes

                // Prioritize enabled themes
                foreach (var enabledTheme in ThemesManager.EnabledThemes)
                {
                    if (Tabs.All(t => t.ThemeId != enabledTheme.Id))
                        Tabs.Add(new TabItem(enabledTheme.Name, enabledTheme.Id));
                }
                var themesId = MainViewModel.Messages.Select(m => m.ThemeId).Distinct().ToList();

                // Fill in with the rest
                foreach (var themeId in themesId)
                {
                    if (Tabs.All(t => t.ThemeId != themeId))
                        Tabs.Add(new TabItem(ThemesManager.GetTheme(themeId).Name, themeId));
                }

                NewTab.BadgeCount = MainViewModel.Messages.Where(NewTab.Filter).Count();

                RaisePropertyChanged(nameof(ShowEmptyNewMessages));

                if (IsBusy && MainViewModel.Messages.Any())
                    StartTimeOutAsync(200);
            };
        }

        private void OnSelectedTabChanged(TabItem old, TabItem @new)
        {
            if (old == NewTab)
            {
                NewTab.BadgeCount = 0;
            }

            RaisePropertyChanged(nameof(ShowEmptyNewMessages));
        }

        private async void StartTimeOutAsync(int timeoutMilliseconds)
        {
            await Task.Delay(timeoutMilliseconds);
            // if still busy, don't show busy-indicator (happens if user has deleted all his messages)
            if (IsBusy)
                IsBusy = false;
        }
    }

    public class TabItem : ViewModel
    {
        private bool _isCurrent;
        public bool IsCurrent
        {
            get => _isCurrent;
            set => SetProperty(ref _isCurrent, value);
        }

        public string Title { get; set; }

        public int ThemeId { get; set; }

        public TabItem(string title, int themeId = 0)
        {
            Title = title;
            ThemeId = themeId;

            if (ThemeId > 0)
                Filter = model => model.ThemeId == ThemeId;
        }

        public Func<MessageViewModel, bool> Filter { get; set; } = model => true;

        private int _badgeCount;

        public int BadgeCount
        {
            get => _badgeCount;
            set
            {
                SetProperty(ref _badgeCount, value);
                RaisePropertyChanged(nameof(HasBadges));
            }
        }

        public bool HasBadges => BadgeCount > 0;
    }
}
