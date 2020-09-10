using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HeiaMeg.ViewModels.Items.Theme
{
    public interface IThemeSelection
    {
        ObservableCollection<ThemeViewModel> Themes { get; }

        ICommand ItemTappedCommand { get; }
    }
}
