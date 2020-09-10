using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HeiaMeg.Pages.Message;
using HeiaMeg.Pages.Modals;
using HeiaMeg.Resources;
using HeiaMeg.Services;
using HeiaMeg.Utils;
using HeiaMeg.Utils.Analytics;
using HeiaMeg.ViewControls;
using HeiaMeg.ViewControls.Custom;
using HeiaMeg.ViewModels.Base;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmHelpers.Commands;
using Xamarin.Essentials;
using Xamarin.Forms;
using FeedbackType = HeiaMeg.Models.FeedbackType;

namespace HeiaMeg.ViewModels.Items.Message
{
    public class MessageViewModel : ViewModel
    {
        public Models.Message Model { get; }

        public int Id { get; }
        public int ThemeId { get; }
        public string Text { get; }
        public string Title { get; }
        public string LinkText { get; }
        public DateTime Time { get; }
        public bool IsOpened { get; }
        public ImageSource ImageSource { get; }
        public List<OptionItemViewModel> Options { get; private set; }

        public MessageViewModel(Models.Message model)
        {
            Model = model;

            Id = Model.Id;
            ThemeId = Model.ThemeId;
            Text = StringUtils.ParseDynamicText(Model);
            Title = ThemesManager.Themes.FirstOrDefault(t => t.Id == model.ThemeId)?.Name.ToUpperInvariant();
            LinkText = (string.IsNullOrEmpty(model.LinkText) ? Model.Link : Model.LinkText);
            ImageSource = Images.ThemeIconFromId(model.ThemeId, true);
            Time = model.NotifyTime ?? default;
            IsFavorite = model.IsFavorite;
            IsOpened = model.Opened.HasValue && (DateTime.Today - model.Opened.Value.Date).Days > 0;

            Options = GetAvailableOptions();

            UrlClickedCommand = new AsyncCommand(
                OpenLink,
                canEx => true,
                exception =>
                {
                    Crashes.TrackError(exception, new Dictionary<string, string> { { "url: ", Model.Link } });
                    Shell.Current.DisplayAlert("Ops!", $"Noe gikk galt.\nKunne ikke åpne: {Model.Link}", "Ok").ConfigureAwait(false);
                }
            );

            FavoriteCommand = new AsyncCommand(
                ToggleFavorite,
                canEx => true, 
                exception => Crashes.TrackError(exception)
            );
        }

        private async Task OpenLink()
        {
            if (await Launcher.CanOpenAsync(Model.Link))
            {
                await Launcher.OpenAsync(Model.Link);
            }
            else
            {
                await Shell.Current.DisplayAlert("Ops!", $"Noe gikk galt.\nKunne ikke åpne: {Model.Link}", "Ok");
            }

            Analytics.TrackEvent(TrackingEvents.Message,
                new TrackingEvents.MessageToArgs(TrackingEvents.MessageToArgs.MessageAction.UrlClicked, Model));
            Analytics.TrackEvent(TrackingEvents.ItemTapped,
                new TrackingEvents.ItemTappedArgs(Model.Link));
        }

        private async Task ToggleFavorite()
        {
            IsFavorite = !IsFavorite;
            await StorageService.Instance.UpdateMessageAsync(Model);
            Analytics.TrackEvent(TrackingEvents.Message,
                new TrackingEvents.MessageToArgs(TrackingEvents.MessageToArgs.MessageAction.Favorited, Model));
        }

        public ICommand UrlClickedCommand { get; }

        public ICommand FavoriteCommand { get; }

        public bool IsFavorite
        {
            get => Model.IsFavorite;
            set
            {
                Model.IsFavorite = value;
                RaisePropertyChanged();
                Options = GetAvailableOptions();
            }
        }

        private List<OptionItemViewModel> GetAvailableOptions()
        {
            var all = new List<OptionItemViewModel>();

            if (Model.FeedbackType == FeedbackType.None)
            {
                all.Add(LikeOption);
                all.Add(DislikeOption);
            }

            if (!IsFavorite)
            {
                all.Add(TrashOption);
            }

            all.Add(ShareOption);

            return all;
        }

        private static readonly OptionItemViewModel LikeOption = new OptionItemViewModel("Bra", Images.Like,
            new Xamarin.Forms.Command<OptionImageButton>(async (view) =>
            {
                if (!(view.CommandParameter is MessageViewModel vm))
                    return;

                vm.Model.FeedbackType = FeedbackType.Like;
                await StorageService.Instance.UpdateMessageAsync(vm.Model);
                vm.Options = vm.GetAvailableOptions();

                Analytics.TrackEvent(TrackingEvents.Message,
                    new TrackingEvents.MessageToArgs(TrackingEvents.MessageToArgs.MessageAction.Liked,
                        vm.Model));
            }));

        private static readonly OptionItemViewModel DislikeOption = new OptionItemViewModel("Dårlig", Images.Dislike,
            new Xamarin.Forms.Command<OptionImageButton>(async (view) =>
            {
                if (!(view.CommandParameter is MessageViewModel vm))
                    return;
                vm.Model.FeedbackType = FeedbackType.Dislike;
                await StorageService.Instance.UpdateMessageAsync(vm.Model);
                vm.Options = vm.GetAvailableOptions();

                Analytics.TrackEvent(TrackingEvents.Message,
                    new TrackingEvents.MessageToArgs(TrackingEvents.MessageToArgs.MessageAction.Disliked,
                        vm.Model));

                if (await Shell.Current.DisplayAlert("Tilbakemelding", "Ønsker du gi oss en tilbakemelding?", "Ja",
                    "Nei"))
                {
                    await FeedbackFormPage.OpenFeedback(vm.Id);
                }
            }));

        private static readonly OptionItemViewModel TrashOption = new OptionItemViewModel("Fjern", Images.Trash,
            new Xamarin.Forms.Command<OptionImageButton>(async (view) =>
            {
                if (!(view.CommandParameter is MessageViewModel vm))
                    return;

                if (await Shell.Current.DisplayAlert("Fjern melding", "Er du sikker på at du vil fjerne meldingen?",
                    "Ja", "Nei"))
                {
                    vm.Model.Removed = DateTime.Now;
                    await StorageService.Instance.UpdateMessageAsync(vm.Model);
                    MainViewModel.Messages.Remove(vm);

                    Analytics.TrackEvent(TrackingEvents.Message,
                        new TrackingEvents.MessageToArgs(TrackingEvents.MessageToArgs.MessageAction.Removed,
                            vm.Model));

                    if (await Shell.Current.DisplayAlert("Tilbakemelding", "Ønsker du gi oss en tilbakemelding?", "Ja",
                        "Nei"))
                    {
                        await FeedbackFormPage.OpenFeedback(vm.Id);
                    }
                }
            }));

        private static readonly OptionItemViewModel ShareOption = new OptionItemViewModel("Del", Images.Share,
            new Xamarin.Forms.Command<OptionImageButton>(async (view) =>
            {
                if (!(view.CommandParameter is MessageViewModel vm))
                    return;

                try
                {
                    var parent = view.Parent;
                    while (parent != null)
                    {
                        if (parent is MessageView messageView)
                        {
                            await messageView.CloseOptions();
                            ShareScreenshot(messageView);
                            break;
                        }

                        parent = parent.Parent as View;
                    }
                }
                catch (Exception e)
                {
                    Crashes.TrackError(e);
                }
            }));

        public static void ShareScreenshot(MessageView view)
        {
            ShareManager.Instance.ShareScreenshot(view);

            if (view.BindingContext is MessageViewModel vm)
                Analytics.TrackEvent(TrackingEvents.Message,
                    new TrackingEvents.MessageToArgs(TrackingEvents.MessageToArgs.MessageAction.Shared, vm.Model));
            Analytics.TrackEvent(TrackingEvents.ItemTapped,
                new TrackingEvents.ItemTappedArgs(TrackingEvents.ItemsToTap.Share));
        }

        public override bool Equals(object obj)
        {
            if (obj is MessageViewModel other)
            {
                return    other.Id == Id
                       && other.IsFavorite == IsFavorite
                       && other.IsOpened == IsOpened;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return   Id.GetHashCode() 
                   ^ IsFavorite.GetHashCode() 
                   ^ IsOpened.GetHashCode();
        }
    }
}
