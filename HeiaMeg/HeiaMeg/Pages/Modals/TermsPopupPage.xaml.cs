using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace HeiaMeg.Pages.Modals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsPopupPage : PopupPage
    {
        public TermsPopupPage()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAllAsync(true);
        }
    }
}