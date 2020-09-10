namespace HeiaMeg.ViewModels.Items.Onboarding
{
    public class OnboardingWelcomeViewModel : OnboardingItemViewModel
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                IsValid = !string.IsNullOrEmpty(Name);
            }
        }
    }
}