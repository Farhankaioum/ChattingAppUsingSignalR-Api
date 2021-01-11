using System;

namespace ChattingApp.Foundation.Helpers
{
    [Serializable]
    internal class EmptyException : Exception
    {
        public EmptyException()
        {
        }

        public EmptyException(string message) : base(message)
        {
        }
        
    }
}