using Newtonsoft.Json;

namespace HeiaMeg.Models
{
    public class Theme : ITheme
    {
        [JsonProperty] public int Id { get; private set; }
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public string Title { get; set; }
        [JsonProperty] public string Description { get; set; }

        public Theme()
        {
        }

        public Theme(int id)
        {
            Id = id;
        }

        public Theme(ThemeType theme) : this((int)theme)
        {
        }

        [JsonIgnore]
        public bool IsEnabled => UserSettings.Theme.GetEnabled(Id);

        [JsonIgnore]
        public bool IsDeactivated { get; set; }

        [JsonIgnore]
        public Frequency Frequency
        {
            get
            {
                switch (Id)
                {
                    case (int)ThemeType.Alcohol:
                        return Frequency.Rare;
                    default:
                        return Frequency.Often;
                }
            }
        }
    }

    public enum ThemeType
    {
        Smoking = 1,
        Alcohol = 2,
        Exercise = 3,
        Diet = 4,
        Mentality = 5,
        Sleep = 6,
    }

    public enum Frequency
    {
        Often = 1,
        Moderate = 2,
        Rare = 3,
    }
}