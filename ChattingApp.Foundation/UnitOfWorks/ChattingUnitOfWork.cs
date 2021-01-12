using ChattingApp.Data;
using ChattingApp.Foundation.Contexts;
using ChattingApp.Foundation.Repositories;

namespace ChattingApp.Foundation.UnitOfWorks
{
    public class ChattingUnitOfWork : UnitOfWork, IChattingUnitOfWork
    {
        public ChattingUnitOfWork(ChattingContext context,
                IUserRepository userRepository,
                IMessageRepository messageRepository)
            : base(context)
        {
            UserRepository = userRepository;
            MessageRepository = messageRepository;
        }

        public IUserRepository UserRepository { get; set; }

        public IMessageRepository MessageRepository { get; set; }
    }
}
