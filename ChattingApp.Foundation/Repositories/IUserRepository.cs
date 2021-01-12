using ChattingApp.Data;
using ChattingApp.Foundation.Contexts;
using ChattingApp.Foundation.Entities;
using System;

namespace ChattingApp.Foundation.Repositories
{
    public interface IUserRepository : IRepository<User, Guid, ChattingContext>
    {
        User GetUserByEmail(string email);
    }
}
