using System;
using HeiaMeg.ViewModels.Base;

namespace HeiaMeg.ViewModels.Items.Onboarding
{
    public abstract class OnboardingItemViewModel : ViewModel
    {
        public event Action IsValidChanged;

        private bool _isValid;
        public bool IsValid
        {
            get => _isValid;
            protected set
            {
                if (_isValid == value)
                    return;

                _isValid = value;
                IsValidChanged?.Invoke();
            }
        }
    }
}