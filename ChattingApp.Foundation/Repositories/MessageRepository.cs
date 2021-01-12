using ChattingApp.Data;
using ChattingApp.Foundation.Contexts;
using ChattingApp.Foundation.Entities;
using ChattingApp.Foundation.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChattingApp.Foundation.Repositories
{
    public class MessageRepository : Repository<Message, long, ChattingContext>, IMessageRepository
    {
        public MessageRepository(ChattingContext context)
            : base(context)
        {

        }

        public Message GetMessage(long id)
        {
            return _dbContext.Messages.FirstOrDefault(m => m.Id == id);
        }

        public IList<Message> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = _dbContext.Messages
               .Include(u => u.Sender)
               .Include(u => u.Recipient)
               .AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.RecipientDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId && u.SenderDeleted == false);
                    break;
                default:
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId
                        && u.RecipientDeleted == false && u.IsRead == false);
                    break;
            }

            messages = messages.OrderByDescending(d => d.MessageSent);

            return messages.ToList();

        }

        public IEnumerable<Message> GetMessageThread(Guid userId, Guid recipientId)
        {
            var messages = _dbContext.Messages
                .Include(u => u.Sender)
                .Include(u => u.Recipient)
                .Where(m => m.RecipientId == userId && m.RecipientDeleted == false
                    && m.SenderId == recipientId ||
                    m.RecipientId == recipientId &&
                    m.SenderId == userId && m.SenderDeleted == false)
                .OrderByDescending(m => m.MessageSent)
                .ToList();

            return messages;
        }
    }
}
