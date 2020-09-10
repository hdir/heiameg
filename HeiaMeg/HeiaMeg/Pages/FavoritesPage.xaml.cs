using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using HeiaMeg.Pages.Message;
using HeiaMeg.ViewModels;
using HeiaMeg.ViewModels.Items.Message;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeiaMeg.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritesPage : ContentPage
    {
        private bool _isEmpty = true;
        public bool IsEmpty
        {
            get => _isEmpty;
            set
            {
                if (_isEmpty != value)
                {
                    if (!_isEmpty && value)
                    {
                        EmptyView.FadeTo(1f);
                    }
                    else if (_isEmpty && !value)
                    {
                        EmptyView.FadeTo(0f);
                    }
                    _isEmpty = value;
                }
            }
        }


        public FavoritesPage()
        {
            InitializeComponent();

            MainViewModel.Messages.CollectionChanged += (sender, args) =>
            {
                if (args.OldItems != null)
                {
                    foreach (var item in args.OldItems)
                    {
                        if (item is INotifyPropertyChanged notify)
                            notify.PropertyChanged -= OnItemPropertyChanged;
                    }
                }
                if (args.NewItems != null)
                {
                    foreach (var item in args.NewItems)
                    {
                        if (item is INotifyPropertyChanged notify)
                            notify.PropertyChanged += OnItemPropertyChanged;
                    }
                }
                IsEmpty = CheckEmpty();
            };
        }

        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsEmpty = CheckEmpty();
        }

        private static bool CheckEmpty() => !MainViewModel.Messages.Where(FavoritesViewModel.FavoriteFilter).Any();

        private async void Share_OnTapped(object sender, EventArgs e)
        {
            if (sender is View view)
            {
                await view.ScaleTo(0.9f, 100);
#pragma warning disable 4014
                view.ScaleTo(1.1, 150, Easing.SpringOut);
#pragma warning restore 4014

                try
                {
                    var parent = view.Parent;
                    while (parent != null)
                    {
                        if (parent is MessageView messageView)
                        {
                            MessageViewModel.ShareScreenshot(messageView);
                            break;
                        }

                        parent = parent.Parent as View;
                    }
                }
                catch (Exception exception)
                {
                    Crashes.TrackError(exception);
                }
            }
        }
    }
}