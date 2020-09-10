using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeiaMeg.Pages.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsThemeSelectionView : ContentView
    {
        public SettingsThemeSelectionView()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (height <= 0)
                return;

            if (height < width * 1.2)
                ThemeGridView.HeightRequest = width * 1.5f;
        }
    }
}