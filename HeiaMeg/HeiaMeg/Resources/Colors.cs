using Xamarin.Forms;

namespace HeiaMeg.Resources
{
    public static class Colors
    {
        #region Colors

        private static Color White { get; } = Color.FromHex("FFFFFF");
        private static Color White_Off { get; } = Color.FromHex("F9F6F4");
        private static Color White_iOS { get; } = Color.FromHex("FAFAFA");
        private static Color Black { get; } = Color.FromHex("191716");
        private static Color Red { get; } = Color.FromHex("C53317");
        private static Color Orange { get; } = Color.FromHex("F8533E");
        private static Color Beige_light { get; } = Color.FromHex("FFEDE1");
        private static Color Beige_dark { get; } = Color.FromHex("E5D0C6");
        private static Color Black_blue { get; } = Color.FromHex("0b353e");
        private static Color Blue { get; } = Color.FromHex("0000EE");
        private static Color Blue_light { get; } = Color.FromHex("C1F3F7");
        private static Color Blue_medium { get; } = Color.FromHex("ACDEE4");
        private static Color Blue_medium_plus { get; } = Color.FromHex("025169");
        private static Color Blue_dark { get; } = Color.FromHex("207384");
        private static Color Blue_darker { get; } = Color.FromHex("18535f");
        private static Color Blue_darker_plus { get; } = Color.FromHex("11424b");
        private static Color Blue_darkest { get; } = Color.FromHex("0b353e");
        private static Color Blue_accent { get; } = Color.FromHex("217484");
        //private static Color Blue_accent2 { get; } = Color.FromHex("1a5c6a");
        //private static Color Red_light { get; } = Color.FromHex("f5eeeb"); wrong? it's not red
        private static Color Gray { get; } = Color.FromHex("A4A4A4");

        #endregion

        public static readonly Color Text = Blue_darker;
        public static readonly Color Background = White;
        public static readonly Color BackgroundOverlay = Background.MultiplyAlpha(0.5);

        public static Color Hyperlink { get; } = Blue;

        public static Color Badge { get; } = Orange;

        #region Onboarding

        public static readonly Color OnboardingBackground = Beige_light;
        public static readonly Color OnboardingText = Blue_darker;
        public static readonly Color OnboardingSelected = Orange;

        #endregion

        #region Bottom Tabs

        public static Color TabBottomBackground { get; } = Blue_darker;
        public static Color TabBottomForeground { get; } = White;
        public static Color TabBottomForegroundActive { get; } = Blue_light;

        #endregion

        #region Top Tabs

        public static Color TabForeground { get; } = Blue_darker;
        public static Color TabBackground { get; } = Color.Transparent;
        public static Color TabBottomBorder { get; } = Black.MultiplyAlpha(0.1);

        #endregion

        #region Messages

        public static Color MessageBackground { get; } = Beige_light;
        public static Color MessageForeground { get; } = Black;
        public static Color MessageTitle { get; } = Blue_darker;
        public static Color MessageIconBackground { get; } = Blue_darker;
        public static Color MessageIconForeground { get; } = White;
        public static Color MessageTitleSplit { get; } = Black.MultiplyAlpha(0.1);

        public static Color MessageIconFavoriteBackground { get; } = Orange;

        public static Color NewMessageColor { get; } = Orange;
        public static Color NewMessageBackground { get; } = Beige_light;

        public static Color ScrollbarActive { get; } = Blue_darker;
        public static Color ScrollbarBackground { get; } = Black.MultiplyAlpha(0.1);

        #endregion

        #region Messages Options

        public static Color UtilityIcon { get; } = Blue_darker;
        public static Color OptionsIcon { get; } = Blue_darker;
        public static Color OptionsMessage { get; } = Blue_darker;

        #endregion

        #region Settings

        public static Color SettingsSelectThemeCircleBackground { get; } = Beige_light;
        public static Color SettingsText { get; } = Blue_darker;

        #endregion

        #region Modal

        public static Color ModalBackground { get; } = Beige_light;
        public static Color ModalForeground { get; } = Black;

        public static Color ModalActivity { get; } = Orange;

        public static Color ModalError { get; } = Red;
        public static Color ModalSuccess { get; } = Color.Green;

        #endregion
    }
}
