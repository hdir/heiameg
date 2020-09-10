using System;
using HeiaMeg.ViewModels.Items.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeiaMeg.Pages.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsListItem : Grid
    {
        public SettingsListItem()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            if (BindingContext is SettingsItemViewModel vm)
                vm.Clicked?.Execute(vm);
        }
    }
}