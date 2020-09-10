using System;

namespace HeiaMeg.Droid.Notifications
{
    public class AndroidNotification
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int Id { get; set; }
        public DateTime NotifyTime { get; set; }
    }
}