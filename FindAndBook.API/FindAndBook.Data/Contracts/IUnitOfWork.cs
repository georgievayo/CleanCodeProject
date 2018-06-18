using System.Threading.Tasks;

namespace FindAndBook.Data.Contracts
{
    public interface IUnitOfWork
    {
        void Commit();

        Task CommitAsync();
    }
}
