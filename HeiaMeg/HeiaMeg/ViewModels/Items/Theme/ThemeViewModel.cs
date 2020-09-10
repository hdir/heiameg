using HeiaMeg.Resources;
using HeiaMeg.ViewModels.Base;
using Xamarin.Forms;

namespace HeiaMeg.ViewModels.Items.Theme
{
    public class ThemeViewModel : ViewModel
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public string Title { get; }
        public string Description { get; }

        public ImageSource Icon { get; }

        public Models.Theme Model { get; }

        public ThemeViewModel(Models.Theme theme)
        {
            Model = theme;
            Title = theme.Title;
            Icon = Images.ThemeIconFromId(theme.Id);
            Description = theme.Description;
        }
    }
}