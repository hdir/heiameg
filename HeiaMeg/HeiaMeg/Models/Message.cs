using System;
using Newtonsoft.Json;
using SQLite;

namespace HeiaMeg.Models
{
    public class Message : IMessage
    {
        [JsonProperty, PrimaryKey]
        public int Id { get; set; }
        [JsonProperty]
        public int ThemeId { get; set; }

        [JsonProperty]
        public string Text { get; set; }
        [JsonProperty]
        public string Link { get; set; }
        [JsonProperty]
        public string LinkText { get; set; }

        [JsonProperty]
        public DateTime? Date { get; set; }
        [JsonProperty]
        public TimeSpan From { get; set; }
        [JsonProperty]
        public TimeSpan To { get; set; }

        public DateTime? Opened { get; set; }
        public DateTime? NotifyTime { get; set; }
        public DateTime? Removed { get; set; }

        public bool IsFavorite { get; set; }
        public FeedbackType FeedbackType { get; set; }

        protected bool Equals(Message other)
        {
            return Id == other.Id
                   && ThemeId == other.ThemeId
                   && NotifyTime == other.NotifyTime
                   && string.Equals(Text, other.Text)
                   && string.Equals(Link, other.Link)
                   && string.Equals(LinkText, other.LinkText)
                   && FeedbackType == other.FeedbackType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Message)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ ThemeId;
                hashCode = (hashCode * 397) ^ NotifyTime.GetHashCode();
                hashCode = (hashCode * 397) ^ (Text != null ? Text.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Link != null ? Link.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LinkText != null ? LinkText.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)FeedbackType;
                return hashCode;
            }
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Text) || !string.IsNullOrEmpty(Link);
        }
    }
}
