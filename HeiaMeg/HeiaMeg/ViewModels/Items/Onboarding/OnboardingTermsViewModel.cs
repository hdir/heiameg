namespace HeiaMeg.ViewModels.Items.Onboarding
{
    public class OnboardingTermsViewModel : OnboardingItemViewModel
    {
        public bool IsTermsAccepted
        {
            get => IsValid;
            set
            {
                if (IsValid != value)
                {
                    IsValid = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool _isStatisticsAccepted;
        public bool IsStatisticsAccepted
        {
            get => _isStatisticsAccepted;
            set
            {
                _isStatisticsAccepted = value;
                RaisePropertyChanged();
            }
        }
    }
}