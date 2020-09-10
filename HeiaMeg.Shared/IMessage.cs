using System;

namespace HeiaMeg
{
    public interface IMessage
    {
        int Id { get; }
        int ThemeId { get; }
        string Text { get; }
        string Link { get; }
        string LinkText { get; }
        TimeSpan From { get; }
        TimeSpan To { get; }
        DateTime? Date { get; }
    }
}
