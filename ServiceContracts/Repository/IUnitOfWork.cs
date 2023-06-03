using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IAsyncRepository<T> GetRepository<T>() where T : class;
        Task<bool> SaveChangesAsync();
        void CreateTransaction();
        void CommitTransaction();
    }
}
