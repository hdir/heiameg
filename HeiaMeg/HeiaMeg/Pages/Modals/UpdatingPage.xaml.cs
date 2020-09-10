using System;
using System.Collections;
using System.Collections.Specialized;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace HeiaMeg.Pages.Modals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdatingPage : PopupPage
    {
        public UpdatingPage()
        {
            InitializeComponent();

            var viewCellHeight = (double) Resources["ViewCellHeight"];

            if (StepList.ItemsSource is INotifyCollectionChanged notify && StepList.ItemsSource is ICollection collection)
                notify.CollectionChanged += (o, eventArgs) =>
                {
                    StepList.HeightRequest = viewCellHeight * collection.Count;
                };
        }

        private void Cancel_OnClicked(object sender, EventArgs e)
        {
            MainPage.CloseUpdatePage().ConfigureAwait(false);
        }
    }
}