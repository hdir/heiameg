using System;
using System.Collections;
using System.ComponentModel;
using HeiaMeg.ViewModels;
using Microsoft.AppCenter.Crashes;
using PanCardView.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeiaMeg.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArchivePage : ContentPage
    {
        public ArchivePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (App.NotificationId > 0)
            {
                App.NotificationId = 0;
                // Show new tab if notification id is clicked
                TabsView.SelectedItem = ArchiveViewModel.NewTab;
            }
        }

        private void TabsView_OnSelectionChanged(object sender, EventArgs args)
        {
            try
            {
                var view = TabsView.Children[TabsView.ItemsSource.FindIndex(TabsView.SelectedItem)];

                if (TabsScrollView.ScrollX > view.X)
                    TabsScrollView.ScrollToAsync(view.X, 0, true);
                else if (TabsScrollView.ScrollX + TabsScrollView.Width < view.X + view.Width)
                    TabsScrollView.ScrollToAsync(view.X + view.Width - TabsScrollView.Width, 0, true);

                if (MessagesView.FilteredItemsSource.Count() > 0)
                {
                    // Scroll to start on message list
                    var first = MessagesView.FilteredItemsSource.FindValue(0);
                    MessagesView.ScrollTo(first, ScrollToPosition.Start, false);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private void TabNext()
        {
            var next = TabsView.ItemsSource.FindIndex(TabsView.SelectedItem) + 1;
            if (TabsView.ItemsSource.Count() > next && TabsView.ItemsSource is IList list)
                TabsView.SelectedItem = list[next];
        }

        private void TabPrevious()
        {
            var prev = TabsView.ItemsSource.FindIndex(TabsView.SelectedItem) - 1;
            if (prev >= 0 && TabsView.ItemsSource is IList list)
                TabsView.SelectedItem = list[prev];
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            TabNext();
        }

        private void SwipeGestureRecognizer_OnSwiped(object sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Right:
                    TabPrevious();
                    break;
                case SwipeDirection.Left:
                    TabNext();
                    break;
                default:
                case SwipeDirection.Up:
                case SwipeDirection.Down:
                    break;
            }
        }

        private async void MessagesView_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "Filter")
                {
                    MessagesView.Opacity = 0.2;
                    await MessagesView.FadeTo(1, 650, Easing.Linear);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}