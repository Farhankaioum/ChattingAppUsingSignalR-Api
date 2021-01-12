using ChattingApp.Foundation.Entities;
using ChattingApp.Foundation.Helpers;
using ChattingApp.Foundation.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChattingApp.Foundation.Services
{
    public class MessageService : IMessageService
    {
        private readonly IChattingUnitOfWork _chattingUnitOfWork;
        private readonly IValidator _validator;

        public MessageService(IChattingUnitOfWork chattingUnitOfWork, IValidator validator)
        {
            _chattingUnitOfWork = chattingUnitOfWork;
            _validator = validator;
        }

        public void AddMessage(Message message)
        {
            var sender = _chattingUnitOfWork.UserRepository.GetById(message.SenderId);
            if (sender == null)
                throw new InValidException("Sender not found!");

            var receiver = _chattingUnitOfWork.UserRepository.GetById(message.RecipientId);
            if (receiver == null)
                throw new InValidException("Receipient not found!");

            if (!_validator.IsValidString(message.Content))
                throw new EmptyException("Content is not empty!");

            _chattingUnitOfWork.MessageRepository.Add(message);
            _chattingUnitOfWork.Save();
        }

        public void DeleteMessage(long msgId, Guid userId)
        {
            var user = _chattingUnitOfWork.UserRepository.GetById(userId);
            if (user == null)
                throw new InValidException("User not found!");

            var existingMsg = _chattingUnitOfWork.MessageRepository.GetById(msgId);
            if(existingMsg == null)
                throw new InValidException("Message not found!");

            _chattingUnitOfWork.MessageRepository.Remove(msgId);
            _chattingUnitOfWork.Save();
        }

        public Message GetMessage(long id)
        {
            var existingMsg = _chattingUnitOfWork.MessageRepository.GetById(id);
            if (existingMsg == null)
                throw new InValidException("Message not found!");

            return existingMsg;
        }

        public IList<Message> GetMessagesForUser(MessageParams messageParams)
        {
            return _chattingUnitOfWork.MessageRepository.GetMessagesForUser(messageParams);

        }

        public IList<Message> GetMessageThread(Guid userId, Guid recipientId)
        {
            return _chattingUnitOfWork.MessageRepository.GetMessageThread(userId, recipientId).ToList();
        }
    }
}
