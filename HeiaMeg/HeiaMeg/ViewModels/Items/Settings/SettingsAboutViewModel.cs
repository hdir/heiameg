using Xamarin.Forms;

namespace HeiaMeg.ViewModels.Items.Settings
{
    public class SettingsAboutViewModel : SettingsItemViewModel
    {
        public override string Title { get; } = "Om Heia Meg";

        public SettingsAboutViewModel(Command<SettingsItemViewModel> clicked) : base(clicked)
        {
        }
    }
}