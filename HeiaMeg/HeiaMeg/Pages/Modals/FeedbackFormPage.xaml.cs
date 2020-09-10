using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
#if !DEBUG
using HeiaMeg.Services;
#endif

namespace HeiaMeg.Pages.Modals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedbackFormPage : ContentPage
    {
        private readonly int _id;

        private FeedbackFormPage(int id)
        {
            _id = id;
            InitializeComponent();
        }

        private void EntryContainer_OnFocusChangeRequested(object sender, FocusRequestArgs e)
        {
            if (e.Focus)
            {
                EntryContainer.BorderColor = Color.LawnGreen;
                EntryContainer.BackgroundColor = Color.White;
            }
            else
            {
                EntryContainer.BorderColor = Color.DarkGray;
                EntryContainer.BackgroundColor = Color.DarkGray;
            }
        }

        private async void Submit_OnTapped(object sender, EventArgs e)
        {
            var toggles = new List<string>();

            if (Alt1Switch.IsToggled)
                toggles.Add(Alt1.Text);
            if (Alt2Switch.IsToggled)
                toggles.Add(Alt2.Text);
            if (Alt3Switch.IsToggled)
                toggles.Add(Alt3.Text);
            if (Alt4Switch.IsToggled)
                toggles.Add(Alt4.Text);

            await CloseFeedback();
#if !DEBUG
            await FeedbackUploader.UploadFeedbackResponseAsync(_id, FreeText.Text, toggles);
#endif
        }

        private async void Back_OnTapped(object sender, EventArgs e)
        {
            await BackButton.ScaleTo(0.8, 100, Easing.SpringOut);
            await BackButton.ScaleTo(1, 100, Easing.SpringOut);

            await CloseFeedback();
        }

        public static async Task OpenFeedback(int id)
        {
            await Shell.Current.Navigation.PushModalAsync(new FeedbackFormPage(id), true);
        }

        public static async Task CloseFeedback()
        {
            if (Shell.Current.Navigation.ModalStack.LastOrDefault() is FeedbackFormPage)
                await Shell.Current.Navigation.PopModalAsync(true);
        }
    }
}