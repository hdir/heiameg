using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeiaMeg.Pages.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestbackWebView : ContentView
    {
        public QuestbackWebView()
        {
            InitializeComponent();
        }

        private void WebView_OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            switch (e.Result)
            {
                case WebNavigationResult.Success:
                    InfoView.IsVisible = false;
                    WebView.IsVisible = true;
                    break;
                case WebNavigationResult.Cancel:
                case WebNavigationResult.Timeout:
                case WebNavigationResult.Failure:
                    TextLabel.Text = "Nettverksproblem";
                    Spinner.IsRunning = false;
                    WebView.IsVisible = false;
                    break;
            }
        }
    }
}