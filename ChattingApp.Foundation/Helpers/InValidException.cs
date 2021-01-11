using System;

namespace ChattingApp.Foundation.Helpers
{
    [Serializable]
    public class InValidException : Exception
    {
        public InValidException()
        {
        }

        public InValidException(string message) : base(message)
        {
        }
    }
}
