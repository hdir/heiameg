using Xamarin.Forms;

namespace HeiaMeg.ViewModels.Items.Settings
{
    public class SettingDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ChangeThemesTemplate { get; set; }
        public DataTemplate AboutTemplate { get; set; }
        public DataTemplate TermsTemplate { get; set; }
        public DataTemplate FeedbackTemplate { get; set; }
        public DataTemplate DebugTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case SettingsThemeSelectionViewModel _:
                    return ChangeThemesTemplate;
                case SettingsAboutViewModel _:
                    return AboutTemplate;
                case SettingsTermsViewModel _:
                    return TermsTemplate;
                case SettingsFeedbackViewModel _:
                    return FeedbackTemplate;
                case SettingsDebugViewModel _:
                    return DebugTemplate;
            }
            return new DataTemplate(MissingView);
        }

        private static View MissingView()
        {
            return new Grid()
            {
                BackgroundColor = Color.Fuchsia,
                Children =
                {
                    new Label()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        Text = $"Missing View",
                    }
                }
            };
        }
    }
}