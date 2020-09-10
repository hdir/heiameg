namespace HeiaMeg
{
    public static class Config
    {
        /// <summary>
        /// Url for REST API
        /// </summary>
        public const string ApiUrl = "";
        /// <summary>
        /// API Key for REST API
        /// </summary>
        public const string ApiKey = "";

        /// <summary>
        /// Local database name
        /// </summary>
        public const string DatabaseName = "baredu.db";

        /// <summary>
        /// Analytics Secret for Android
        /// </summary>
        public const string AnalyticsSecretAndroid = "";
        /// <summary>
        /// Analytics Secret for iOS
        /// </summary>
        public const string AnalyticsSecretIOS = "";

        /// <summary>
        /// URL to questback response form
        /// </summary>
        public const string FeedbackUrl = "";

        /// <summary>
        /// Minumum amount of themes to select in onboarding
        /// </summary>
        public const int MinimumThemes = 1;

        /// <summary>
        /// Maximum amount of themes to select in onboarding
        /// </summary>
        public const int MaximumThemes = 2;

        /// <summary>
        /// Maximum days before routine will run when app starts in foreground
        /// </summary>
        public const double UpdateRoutineIntervalDays = 1;

        /// <summary>
        /// Days before user will be presented update-routine failures
        /// </summary>
        public const double UpdateRoutineMaximumIntervalDays = 7;

        /// <summary>
        /// Interval for backgroundjob updating content
        /// </summary>
        public const uint DownloadInterval = 1 * 60 * 60 * 1000; // 1 hour

        /// <summary>
        /// Days before app can request review
        /// </summary>
        public const uint DaysBeforeRequestingReview = 10;

        public struct Feedback
        {
            public const string FormsId = "";
            public const uint MessageFieldId = 1493674206;
            public const uint ResponseFieldId = 507354746;
            public const uint ToggleFieldId = 1854224302;
        }

        public struct Debug
        {
            public const string FormsId = "";
            public const uint DeviceIdId = 308120499;
            public const uint TaskId = 686759602;
            public const uint DatabasePathId = 1493630628;
        }

        public struct DebugLog
        {
            public const string FormsId = "";
            public const uint EventId = 165789180;
            public const uint ParamsId = 801208981;
            public const uint DeviceId = 1912714671;
        }
    }
}
