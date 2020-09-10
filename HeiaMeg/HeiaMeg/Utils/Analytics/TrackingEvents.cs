using System;
using System.Collections.Generic;
using System.Linq;
using HeiaMeg.Models;
using HeiaMeg.ViewModels.Items.Settings;
using Xamarin.Forms;

namespace HeiaMeg.Utils.Analytics
{
    public static class TrackingEvents
    {
        public const string Message = "Messages (New)";
        public const string ItemTapped = "Item Tapped";
        public const string AnalyticsAccepted = "AnalyticsAccepted";
        public const string Onboarding = "Onboarding";
        public const string Adjustment = "Adjustment";
        public const string NavigatedTo = "Navigated To";
        public const string RequestedReview = "RequestedReview";

        public static class OnboardingEvents
        {
            public const string Started = "Started";
            public const string Carousel = "Page";
            public const string Completed = "Completed";
        }

        public static class ItemsToTap
        {
            public const string Terms = "Terms";
            public const string Share = "Share";
        }

        public class NavigatedToArgs : Dictionary<string, string>
        {
            public NavigatedToArgs(string route)
            {
                Add("Tab", route);
            }
            public NavigatedToArgs(SettingsItemViewModel setting)
            {
                Add("Setting", setting.Title);
            }
        }

        public class OnboardingArgs : Dictionary<string, string>
        {
            public OnboardingArgs(string onBoardingEvent)
            {
                Add("Event", onBoardingEvent);
            }

            public OnboardingArgs(ITheme model) 
            {
                Add("Theme", model.Name);
            }

            public OnboardingArgs(string name, int pageIndex)
            {
                Add(name, pageIndex.ToString());
            }

            public OnboardingArgs(IEnumerable<Theme> selected)
            {
                var toString = SelectedToGroupName(selected);
                Add("Combination", toString);
            }

            private static string SelectedToGroupName(IEnumerable<Theme> selected) =>
                string.Join(" & ", selected.Select(t => t.Name));
        }

        public class MessageToArgs : Dictionary<string, string>
        {
            public enum MessageAction
            {
                UrlClicked, Shared, Removed, Liked, Disliked, Favorited
            }

            public MessageToArgs(MessageAction action, IMessage message)
            {
                Add(action.ToString(), message.Id.ToString());
            }
        }

        public class ItemTappedArgs : Dictionary<string, string>
        {
            public ItemTappedArgs(string whichItem)
            {
                Add("Item", whichItem);
            }
            public ItemTappedArgs(Uri uri)
            {
                Add("Url", uri.ToString());
            }
        }

        public class AnalyticsAcceptedArgs : Dictionary<string, string>
        {
            public AnalyticsAcceptedArgs(bool value)
            {
                Add("value", value.ToString());
            }
        }

        public class AdjustmentArgs : Dictionary<string, string>
        {
            public AdjustmentArgs(ITheme theme, bool enabled)
            {
                if (theme != null)
                    Add($"{theme.Title} Enabled", enabled.ToString());
            }

            public AdjustmentArgs(ITheme theme, Frequency frequency)
            {
                if (theme != null)
                    Add($"{theme.Title} Frequency", frequency.ToString());
            }
        }

        public class BackgroundArgs : Dictionary<string, string>
        {
            public BackgroundArgs(string task)
            {
                Add(Device.RuntimePlatform, task);
            }

            public BackgroundArgs(string task, bool result)
            {
                Add(task, result.ToString());
            }
        }

        public class FeedbackArgs : Dictionary<string, string>
        {
            public FeedbackArgs(IMessage message, FeedbackType feedback)
            {
                var tag = $"{feedback}";
                if (message?.Date == null)
                    tag += " (Consecutive)";
                Add(tag, message?.Id.ToString());
            }
        }

        public class RequestedReviewArgs : Dictionary<string, string>
        {
            public RequestedReviewArgs(bool accepted)
            {
                Add("OpenedStore", accepted.ToString());
            }
        }
    }

    public enum FeedbackType
    {
        None,
        Like,
        Dislike,
    }
}
