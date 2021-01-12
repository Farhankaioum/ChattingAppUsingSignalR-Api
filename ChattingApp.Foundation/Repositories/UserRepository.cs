using ChattingApp.Data;
using ChattingApp.Foundation.Contexts;
using ChattingApp.Foundation.Entities;
using System;
using System.Linq;

namespace ChattingApp.Foundation.Repositories
{
    public class UserRepository : Repository<User, Guid, ChattingContext>, IUserRepository
    {
        public UserRepository(ChattingContext context)
            : base(context)
        {

        }

        public User GetUserByEmail(string email)
        {
           return _dbContext.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
