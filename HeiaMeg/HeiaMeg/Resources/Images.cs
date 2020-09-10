using FFImageLoading.Svg.Forms;
using HeiaMeg.Utils;
using Xamarin.Forms;

namespace HeiaMeg.Resources
{
    public static class Images
    {
        private const string BASE_PATH = "HeiaMeg.Resources.Images";

        #region Image Paths

        private static readonly SvgImageSource ic_app_icon = SvgImageSource.FromResource($"{BASE_PATH}.app_icon.svg");
        private static readonly SvgImageSource ic_app_icon_small_blue = SvgImageSource.FromResource($"{BASE_PATH}.app_icon_small_blue.svg");
        private static readonly SvgImageSource ic_heart_heia = SvgImageSource.FromResource($"{BASE_PATH}.heart_heia.svg");
        private static readonly SvgImageSource ic_back = SvgImageSource.FromResource($"{BASE_PATH}.arrow_back.svg");
        private static readonly SvgImageSource ic_hdir = SvgImageSource.FromResource($"{BASE_PATH}.helsedirektoratet.svg");

        private static readonly SvgImageSource ic_smoking = SvgImageSource.FromResource($"{BASE_PATH}.Themes.no-smoking.svg").ReplaceColor(Colors.OnboardingText);
        private static readonly SvgImageSource ic_smoking_white = SvgImageSource.FromResource($"{BASE_PATH}.Themes.no-smoking.svg").ReplaceColor(Color.White);
        private static readonly SvgImageSource ic_beer = SvgImageSource.FromResource($"{BASE_PATH}.Themes.beer.svg").ReplaceColor(Colors.OnboardingText);
        private static readonly SvgImageSource ic_beer_white = SvgImageSource.FromResource($"{BASE_PATH}.Themes.beer.svg").ReplaceColor(Color.White);
        private static readonly SvgImageSource ic_jogging = SvgImageSource.FromResource($"{BASE_PATH}.Themes.jogging.svg").ReplaceColor(Colors.OnboardingText);
        private static readonly SvgImageSource ic_jogging_white = SvgImageSource.FromResource($"{BASE_PATH}.Themes.jogging.svg").ReplaceColor(Color.White);
        private static readonly SvgImageSource ic_brain = SvgImageSource.FromResource($"{BASE_PATH}.Themes.brain.svg").ReplaceColor(Colors.OnboardingText);
        private static readonly SvgImageSource ic_brain_white = SvgImageSource.FromResource($"{BASE_PATH}.Themes.brain.svg").ReplaceColor(Color.White);
        private static readonly SvgImageSource ic_cutlery = SvgImageSource.FromResource($"{BASE_PATH}.Themes.cutlery.svg").ReplaceColor(Colors.OnboardingText);
        private static readonly SvgImageSource ic_cutlery_white = SvgImageSource.FromResource($"{BASE_PATH}.Themes.cutlery.svg").ReplaceColor(Color.White);
        private static readonly SvgImageSource ic_sleep = SvgImageSource.FromResource($"{BASE_PATH}.Themes.sleep.svg").ReplaceColor(Colors.OnboardingText);
        private static readonly SvgImageSource ic_sleep_white = SvgImageSource.FromResource($"{BASE_PATH}.Themes.sleep.svg").ReplaceColor(Color.White);

        private static readonly SvgImageSource ic_like = SvgImageSource.FromResource($"{BASE_PATH}.Options.thumbsup.svg");
        private static readonly SvgImageSource ic_dislike = SvgImageSource.FromResource($"{BASE_PATH}.Options.thumbsdown.svg");
        private static readonly SvgImageSource ic_trash = SvgImageSource.FromResource($"{BASE_PATH}.Options.trash.svg");
        private static readonly SvgImageSource ic_share = SvgImageSource.FromResource($"{BASE_PATH}.Options.share.svg");

        private static readonly SvgImageSource ic_heart = SvgImageSource.FromResource($"{BASE_PATH}.heart.svg");
        private static readonly SvgImageSource ic_heart_fill = SvgImageSource.FromResource($"{BASE_PATH}.heart_fill.svg");
        private static readonly SvgImageSource ic_heart_fill_2 = SvgImageSource.FromResource($"{BASE_PATH}.heart_fill.svg");
        private static readonly SvgImageSource ic_heart_huge = SvgImageSource.FromResource($"{BASE_PATH}.heart_fill.svg");

        private static readonly SvgImageSource ic_checkmark_soft = SvgImageSource.FromResource($"{BASE_PATH}.checkmark_soft.svg");
        private static readonly SvgImageSource ic_checkmark_soft_2 = SvgImageSource.FromResource($"{BASE_PATH}.checkmark_soft.svg");
        private static readonly SvgImageSource ic_checkmark_soft_3 = SvgImageSource.FromResource($"{BASE_PATH}.checkmark_soft.svg");

        private static readonly SvgImageSource ic_pen = SvgImageSource.FromResource($"{BASE_PATH}.pen_edit_name.svg");
        private static readonly SvgImageSource ic_plus = SvgImageSource.FromResource($"{BASE_PATH}.plus.svg");
        #endregion

        public static SvgImageSource Back { get; } = ic_back.ReplaceColor(Colors.MessageTitle);
        public static SvgImageSource AppIcon { get; } = ic_app_icon;

        public static SvgImageSource TabTopIconDefault { get; } = ic_app_icon_small_blue.ReplaceColor(Colors.MessageTitle);
        public static SvgImageSource TabTopIconFavorite { get; } = ic_heart_heia;

        public static SvgImageSource Like { get; } = ic_like.ReplaceColor(Colors.OptionsIcon);
        public static SvgImageSource Dislike { get; } = ic_dislike.ReplaceColor(Colors.OptionsIcon);
        public static SvgImageSource Trash { get; } = ic_trash.ReplaceColor(Colors.OptionsIcon);
        public static SvgImageSource Share { get; } = ic_share.ReplaceColor(Colors.OptionsIcon);

        public static SvgImageSource Favorite { get; } = ic_heart.ReplaceColor(Colors.UtilityIcon);
        public static SvgImageSource FavoriteFill { get; } = ic_heart_fill.ReplaceColor(Colors.NewMessageColor);
        public static SvgImageSource FavoriteHuge { get; } = ic_heart_huge.ReplaceColor(Colors.NewMessageColor);

        public static SvgImageSource Checkmark { get; } = ic_checkmark_soft.ReplaceColor(Color.White);
        public static SvgImageSource Checkmark_Orange { get; } = ic_checkmark_soft_2.ReplaceColor(Colors.OnboardingSelected);
        public static SvgImageSource Checkmark_Orange2 { get; } = ic_checkmark_soft_3.ReplaceColor(Colors.OnboardingSelected);

        public static SvgImageSource Pen { get; } = ic_pen;
        public static SvgImageSource Plus { get; } = ic_plus.ReplaceColor(Colors.UtilityIcon);

        public static SvgImageSource Helsedirektoratet { get; } = ic_hdir;

        public static SvgImageSource ThemeIconFromId(int id, bool light = false) 
        {
            switch (id)
            {
                case 1:
                    return light ? ic_smoking_white : ic_smoking;
                case 2:
                    return light ? ic_beer_white : ic_beer;
                case 3:
                    return light ? ic_jogging_white : ic_jogging;
                case 4:
                    return light ? ic_cutlery_white : ic_cutlery;
                case 5:
                    return light ? ic_brain_white : ic_brain;
                case 6:
                    return light ? ic_sleep_white : ic_sleep;
                default:
                    return ic_heart_heia;
            }
        }
    }
}
