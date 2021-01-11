using ChattingApp.Foundation.Contexts;
using System.Linq;

namespace ChattingApp.Foundation.Helpers
{
    public class Validator : IValidator
    {
        private readonly ChattingContext _chattingContext;

        public Validator(ChattingContext chattingContext)
        {
            _chattingContext = chattingContext;
        }

        public bool IsValidEmail(string email)
        {
            if (!IsValidString(email))
                throw new EmptyException("Email is not empty");

            if (email.Contains('@'))
                return true;

            return false;
        }

        public bool IsValidString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return true;
        }

        public bool IsEmailDuplicateInDB(string email)
        {
           var result = _chattingContext.Users.Any(u => u.Email == email);
           return result;
        }
    }
}
