using System;
using Microsoft.AppCenter.Analytics;
using Plugin.Settings.Abstractions;

namespace HeiaMeg
{
    public static class UserSettings
    {
        private static ISettings CrossSettings => Plugin.Settings.CrossSettings.Current;

        public static bool IntroCompleted => !string.IsNullOrEmpty(UserName);

        private static string _userName;
        public static string UserName
        {
            get => _userName ?? (_userName = CrossSettings.GetValueOrDefault(nameof(UserName), string.Empty));
            set
            {
                CrossSettings.AddOrUpdateValue(nameof(UserName), value);
                _userName = value;
            }
        }

        /// <summary>
        /// The date at midnight of the time when the user completed onboarding
        /// OR deleted all existing data
        /// NOTE: Returns DateTime.Now if this has not been set
        /// </summary>
        public static DateTime StartDate
        {
            get
            {
                var start = CrossSettings.GetValueOrDefault(nameof(StartDate), DateTime.Now);

                // Fix for those with an app installed before onboarding sets start date
                if ((DateTime.Now - start).TotalMilliseconds < 1)
                    StartDate = DateTime.Now;

                return start;
            }
            set => CrossSettings.AddOrUpdateValue(nameof(StartDate), value);
        }

        /// <summary>
        /// Last time updateroutine was run
        /// </summary>
        public static DateTime LastUpdateDate
        {
            get => CrossSettings.GetValueOrDefault(nameof(LastUpdateDate), DateTime.MinValue);
            set => CrossSettings.AddOrUpdateValue(nameof(LastUpdateDate), value);
        }

        /// <summary>
        /// Last modified messages returned by API
        /// </summary>
        public static DateTime LastMessagesModified
        {
            get => CrossSettings.GetValueOrDefault(nameof(LastMessagesModified), DateTime.MinValue);
            set => CrossSettings.AddOrUpdateValue(nameof(LastMessagesModified), value);
        }

        /// <summary>
        /// If app has requested review from user
        /// </summary>
        public static bool HasRequestedReview
        {
            get => CrossSettings.GetValueOrDefault(nameof(HasRequestedReview), false);
            set => CrossSettings.AddOrUpdateValue(nameof(HasRequestedReview), value);
        }

        /// <summary>
        /// If user has accepted analytics (is true for users who initially accepted terms)
        /// </summary>
        public static bool HasAcceptedAnalytics
        {
            get => CrossSettings.GetValueOrDefault(nameof(HasAcceptedAnalytics), IntroCompleted);
            set
            {
                Analytics.SetEnabledAsync(value);
                CrossSettings.AddOrUpdateValue(nameof(HasAcceptedAnalytics), value);
            }
        }

        /// <summary>
        /// If app can request review (hasn't requested before and sufficient amount of time has passed
        /// </summary>
        /// <returns></returns>
        public static bool CanRequestReview()
        {
            return !HasRequestedReview
                   && (DateTime.Now - StartDate).TotalDays >= Config.DaysBeforeRequestingReview;
        }

        public static class Theme
        {
            private static string ThemeEnabled(int id) => $"theme_enabled_{id}";

            public static bool GetEnabled(int id) => CrossSettings.GetValueOrDefault(ThemeEnabled(id), false);
            public static void SetEnabled(int id, bool enabled) { CrossSettings.AddOrUpdateValue(ThemeEnabled(id), enabled); }
        }
    }
}
