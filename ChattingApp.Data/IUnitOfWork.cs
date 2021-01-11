using System;

namespace ChattingApp.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
