using System.Collections.ObjectModel;
using HeiaMeg.Models;
using HeiaMeg.Services;
using Xamarin.Forms;

namespace HeiaMeg.ViewModels.Items.Settings
{
    public class SettingsDebugViewModel : SettingsItemViewModel
    {
        public override string Title { get; } = "Debug";

        public ObservableCollection<DebugInfo> DebugCollection { get; } = new ObservableCollection<DebugInfo>();

        public SettingsDebugViewModel(Command<SettingsItemViewModel> clicked) : base(clicked)
        {
            CollectDebugInfoAsync();
        }

        private async void CollectDebugInfoAsync()
        {
#if DEBUG
            foreach (var info in await StorageService.Instance.GetDebugInfoAsync())
            {
                DebugCollection.Add(info);
            }
#endif
        }
    }
}