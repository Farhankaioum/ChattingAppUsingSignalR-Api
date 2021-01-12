using ChattingApp.Foundation.Entities;
using ChattingApp.Foundation.Helpers;
using System;
using System.Collections.Generic;

namespace ChattingApp.Foundation.Services
{
    public interface IMessageService
    {
        void AddMessage(Message message);
        void DeleteMessage(long msgId, Guid userId);
        Message GetMessage(long id);
        IList<Message> GetMessagesForUser(MessageParams messageParams);
        IList<Message> GetMessageThread(Guid userId, Guid recipientId);
    }
}
