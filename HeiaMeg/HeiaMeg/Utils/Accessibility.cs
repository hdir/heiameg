using System;

namespace HeiaMeg.Utils
{
    public class Accessibility
    {
        public static event Action<bool> AccessibilityEnabledChanged;

        public static bool IsEnabled { get; private set; }

        public static void SetAccessibility(bool enabled, bool invoke = true)
        {
            IsEnabled = enabled;
            if (invoke)
                AccessibilityEnabledChanged?.Invoke(enabled);
        }
    }
}
