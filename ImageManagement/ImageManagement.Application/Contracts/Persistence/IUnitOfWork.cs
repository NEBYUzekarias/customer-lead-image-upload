using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManagement.Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository CustomerRepository { get; }
        ILeadRepository LeadRepository { get; }
        IProfileImageRepository ProfileImageRepository { get; }
        Task<int> Save();
    }
}
