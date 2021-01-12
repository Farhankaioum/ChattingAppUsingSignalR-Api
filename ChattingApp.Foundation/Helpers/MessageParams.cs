using System;

namespace ChattingApp.Foundation.Helpers
{
    public class MessageParams
    {
        public Guid UserId { get; set; }

        public string MessageContainer { get; set; } = "Unread";
    }
}
