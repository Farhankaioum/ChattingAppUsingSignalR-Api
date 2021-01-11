namespace ChattingApp.Foundation.Helpers
{
    public interface IValidator
    {
        bool IsValidString(string value);
        bool IsValidEmail(string email);
        bool IsEmailDuplicateInDB(string email);
    }
}
