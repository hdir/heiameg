using Xamarin.Forms;

namespace HeiaMeg.ViewModels.Items.Settings
{
    public class SettingsFeedbackViewModel : SettingsItemViewModel
    {
        public override string Title { get; } = "Gi tilbakemelding";

        public SettingsFeedbackViewModel(Command<SettingsItemViewModel> clicked) : base(clicked)
        {
        }
    }
}