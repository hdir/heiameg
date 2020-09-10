using HeiaMeg.ViewModels.Base;
using Xamarin.Forms;

namespace HeiaMeg.ViewModels.Items.Settings
{
    public abstract class SettingsItemViewModel : ViewModel
    {
        public abstract string Title { get; }

        public Command<SettingsItemViewModel> Clicked { get; }

        protected SettingsItemViewModel(Command<SettingsItemViewModel> clicked)
        {
            Clicked = clicked;
        }

    }
}