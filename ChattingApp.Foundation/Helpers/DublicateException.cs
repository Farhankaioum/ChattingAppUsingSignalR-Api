using System;

namespace ChattingApp.Foundation.Helpers
{
    [Serializable]
    internal class DublicateException : Exception
    {
        public DublicateException()
        {
        }

        public DublicateException(string message) : base(message)
        {
        }
        
    }
}