using Xamarin.Forms;

namespace HeiaMeg.Resources
{
    public static class Sizes
    {
        private static float DeviceModifier => Device.RuntimePlatform == Device.Android ? 0.92f : 1f;

        #region Text

        public static double TextMicro { get; } = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) * DeviceModifier;
        public static double TextSmall { get; } = Device.GetNamedSize(NamedSize.Small, typeof(Label)) * DeviceModifier;
        public static double TextMedium { get; } = Device.GetNamedSize(NamedSize.Medium, typeof(Label)) * DeviceModifier + 2;
        public static double TextLarge { get; } = Device.GetNamedSize(NamedSize.Large, typeof(Label)) * DeviceModifier + 2;


        public static double Title { get; } = TextLarge + 4;
        public static double TitleLarge { get; } = Title + 4;

        #endregion

        public static double TopTabs { get; } = Title * 1.5;

        public static Thickness NewMessagePadding { get; } = new Thickness(20, 8);

        public static double NewMessageWidth => App.ScreenWidth - NewMessagePadding.HorizontalThickness;

        public static double OptionsHeight { get; } = 68 + TextMedium;

        public static readonly double PositionShiftValue = (NewMessagePadding.HorizontalThickness) * 0.95;
    }
}
