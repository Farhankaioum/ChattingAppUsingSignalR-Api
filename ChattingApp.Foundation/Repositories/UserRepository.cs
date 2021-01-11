using ChattingApp.Data;
using ChattingApp.Foundation.Contexts;
using ChattingApp.Foundation.Entities;
using System;

namespace ChattingApp.Foundation.Repositories
{
    public class UserRepository : Repository<User, Guid, ChattingContext>, IUserRepository
    {
        public UserRepository(ChattingContext context)
            : base(context)
        {

        }
    }
}
