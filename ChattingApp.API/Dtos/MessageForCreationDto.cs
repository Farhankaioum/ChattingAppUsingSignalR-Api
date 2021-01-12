using System;

namespace ChattingApp.API.Dtos
{
    public class MessageForCreationDto
    {
        public Guid SenderId { get; set; }

        public Guid RecipientId { get; set; }

        public DateTime MessageSent { get; set; }

        public string Content { get; set; }

        public MessageForCreationDto()
        {
            MessageSent = DateTime.Now;
        }
    }
}
