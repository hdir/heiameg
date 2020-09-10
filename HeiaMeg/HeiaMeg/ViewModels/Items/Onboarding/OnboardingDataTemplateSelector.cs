using Xamarin.Forms;

namespace HeiaMeg.ViewModels.Items.Onboarding
{
    public class OnboardingDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OnboardingTemplate { get; set; }
        public DataTemplate ThemeSelectionTemplate { get; set; }
        public DataTemplate TermsTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch (item)
            {
                case OnboardingWelcomeViewModel _:
                    return OnboardingTemplate;
                case OnboardingThemeSelectionViewModel _:
                    return ThemeSelectionTemplate;
                case OnboardingTermsViewModel _:
                    return TermsTemplate;
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