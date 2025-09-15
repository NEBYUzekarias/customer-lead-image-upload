using ImageManagement.Application.Contracts.Persistence;

namespace ImageManagement.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlogAppDbContext _context;
        private ICustomerRepository? _customerRepository;
        private ILeadRepository? _leadRepository;
        private IProfileImageRepository? _profileImageRepository;

        public UnitOfWork(BlogAppDbContext context)
        {
            _context = context;
        }

        public ICustomerRepository CustomerRepository
        {
            get
            {
                return _customerRepository ??= new CustomerRepository(_context);
            }
        }

        public ILeadRepository LeadRepository
        {
            get
            {
                return _leadRepository ??= new LeadRepository(_context);
            }
        }

        public IProfileImageRepository ProfileImageRepository
        {
            get
            {
                return _profileImageRepository ??= new ProfileImageRepository(_context);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
