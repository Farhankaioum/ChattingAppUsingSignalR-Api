using ChattingApp.Foundation.Entities;
using ChattingApp.Foundation.Helpers;
using ChattingApp.Foundation.UnitOfWorks;
using System;
using System.Collections.Generic;

namespace ChattingApp.Foundation.Services
{
    public class UserService : IUserService
    {
        private readonly IChattingUnitOfWork _chattingUnitOfWork;
        private readonly IValidator _validator;

        public UserService(IChattingUnitOfWork chattingUnitOfWork, IValidator validator)
        {
            _chattingUnitOfWork = chattingUnitOfWork;
            _validator = validator;
        }

        public void AddUser(User user)
        {
            if (!_validator.IsValidString(user.FirstName))
                throw new EmptyException($"First name is not empty!");

            if(!_validator.IsValidString(user.LastName))
                throw new EmptyException($"Last name is not empty!");

            if (!_validator.IsValidEmail(user.Email))
                throw new InValidException("Email is not valid!");

            if (_validator.IsEmailDuplicateInDB(user.Email))
                throw new DublicateException("Email is already use!");

            user.Id = Guid.NewGuid();
            _chattingUnitOfWork.UserRepository.Add(user);
            _chattingUnitOfWork.Save();
        }

        public IList<User> GetAllUser()
        {
            return _chattingUnitOfWork.UserRepository.GetAll();
        }

        public User GetUserById(Guid id)
        {
            var existingUser = _chattingUnitOfWork.UserRepository.GetById(id);
            if (existingUser == null)
                throw new NullReferenceException($"User not found with id {id}");

            return existingUser;
        }

        public User GetUserByEmail(string email)
        {
           var existingUser =  _chattingUnitOfWork.UserRepository.GetUserByEmail(email);
            if (existingUser == null)
                throw new EmptyException($"User not found with this email {email}");

            return existingUser;
        }
    }
}
