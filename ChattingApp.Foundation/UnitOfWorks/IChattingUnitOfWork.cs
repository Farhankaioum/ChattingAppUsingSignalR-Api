using ChattingApp.Data;
using ChattingApp.Foundation.Repositories;

namespace ChattingApp.Foundation.UnitOfWorks
{
    public interface IChattingUnitOfWork : IUnitOfWork
    {
        IUserRepository UserRepository { get; set; }
    }
}
