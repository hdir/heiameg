using System;
using HeiaMeg.ViewModels.Base;
using HeiaMeg.ViewModels.Items.Message;

namespace HeiaMeg.ViewModels
{
    public class FavoritesViewModel : ViewModel
    {
        #region Properties

        public static Func<MessageViewModel, bool> FavoriteFilter { get; } = model => model.IsFavorite;

        #endregion


        public FavoritesViewModel()
        {
        }
    }
}
