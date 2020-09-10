using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace HeiaMeg.Pages.Modals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThemeDetailsPopupPage : PopupPage
    {
        public ThemeDetailsPopupPage()
        {
            InitializeComponent();
        }

        private void Close_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAllAsync(true);
        }
    }
}