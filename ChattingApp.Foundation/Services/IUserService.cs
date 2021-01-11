using ChattingApp.Foundation.Entities;
using System;
using System.Collections.Generic;

namespace ChattingApp.Foundation.Services
{
    public interface IUserService
    {
        void AddUser(User user);
        User GetUserById(Guid id);
        IList<User> GetAllUser();
    }
}
