using ChattingApp.Data;
using ChattingApp.Foundation.Contexts;
using ChattingApp.Foundation.Repositories;

namespace ChattingApp.Foundation.UnitOfWorks
{
    public class ChattingUnitOfWork : UnitOfWork, IChattingUnitOfWork
    {
        public ChattingUnitOfWork(ChattingContext context,
                IUserRepository userRepository)
            : base(context)
        {
            UserRepository = userRepository;
        }

        public IUserRepository UserRepository { get; set; }
    }
}
