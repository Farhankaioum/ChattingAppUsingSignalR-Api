using ChattingApp.Data;
using ChattingApp.Foundation.Contexts;
using ChattingApp.Foundation.Entities;
using ChattingApp.Foundation.Helpers;
using System;
using System.Collections.Generic;

namespace ChattingApp.Foundation.Repositories
{
    public interface IMessageRepository : IRepository<Message, long, ChattingContext>
    {
        Message GetMessage(long id);
        IList<Message> GetMessagesForUser(MessageParams messageParams);
        IEnumerable<Message> GetMessageThread(Guid userId, Guid recipientId);
    }
}
