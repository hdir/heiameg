using Xamarin.Forms;

namespace HeiaMeg.ViewModels.Items.Settings
{
    public class SettingsTermsViewModel : SettingsItemViewModel
    {
        public override string Title { get; } = "Vilkår";

        public SettingsTermsViewModel(Command<SettingsItemViewModel> clicked) : base(clicked)
        {
        }
    }
}