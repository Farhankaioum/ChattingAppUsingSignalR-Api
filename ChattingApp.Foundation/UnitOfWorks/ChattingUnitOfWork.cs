using ChattingApp.Data;
using ChattingApp.Foundation.Contexts;

namespace ChattingApp.Foundation.UnitOfWorks
{
    public class ChattingUnitOfWork : UnitOfWork, IChattingUnitOfWork
    {
        public ChattingUnitOfWork(ChattingContext context)
            : base(context)
        {

        }
    }
}
