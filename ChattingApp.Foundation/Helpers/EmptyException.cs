using System;

namespace ChattingApp.Foundation.Helpers
{
    [Serializable]
    public class EmptyException : Exception
    {
        public EmptyException()
        {
        }

        public EmptyException(string message) : base(message)
        {
        }
        
    }
}