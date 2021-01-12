using System;

namespace ChattingApp.API.Dtos
{
    public class MessageToReturnDto
    {
        public long Id { get; set; }

        public Guid SenderId { get; set; }

        public Guid RecipientId { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; }

        public DateTime? DateRead { get; set; }

        public DateTime MessageSent { get; set; }
    }
}
