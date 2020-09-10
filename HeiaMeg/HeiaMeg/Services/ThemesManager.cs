using System;
using System.Linq;
using HeiaMeg.Models;
using HeiaMeg.Resources;

namespace HeiaMeg.Services
{
    public static class ThemesManager
    {
        public static event Action ThemeEnabledChanged = () => { };

        public static readonly Theme[] Themes =
        {
            new Theme(ThemeType.Smoking)
            {
                Name = AppText.theme_smoking_name,
                Title = AppText.theme_smoking_title,
                Description = AppText.theme_smoking_description,
                IsDeactivated =  true,
            },
            new Theme(ThemeType.Sleep)
            {
                Name = AppText.theme_sleep_name,
                Title = AppText.theme_sleep_title,
                Description = AppText.theme_sleep_description,
            },
            new Theme(ThemeType.Alcohol)
            {
                Name = AppText.theme_alcohol_name,
                Title = AppText.theme_alcohol_title,
                Description = AppText.theme_alcohol_description,
            },
            new Theme(ThemeType.Exercise)
            {
                Name = AppText.theme_exercise_name,
                Title = AppText.theme_exercise_title,
                Description = AppText.theme_exercise_description,
            },
            new Theme(ThemeType.Diet)
            {
                Name = AppText.theme_diet_name,
                Title = AppText.theme_diet_title,
                Description = AppText.theme_diet_description,
            },
            new Theme(ThemeType.Mentality)
            {
                Name = AppText.theme_mentality_name,
                Title = AppText.theme_mentality_title,
                Description = AppText.theme_mentality_description,
            },
        };

        public static Theme[] EnabledThemes => Themes.Where(theme => theme.IsEnabled).ToArray();

        public static void EnableTheme(Theme theme)
        {
            UserSettings.Theme.SetEnabled(theme.Id, true);
            ThemeEnabledChanged.Invoke();
        }

        public static void DisableTheme(Theme theme)
        {
            UserSettings.Theme.SetEnabled(theme.Id, false);
            ThemeEnabledChanged.Invoke();
        }

        public static Theme GetTheme(int id)
        {
            return Themes.FirstOrDefault(t => t.Id == id);
        }
    }
}
