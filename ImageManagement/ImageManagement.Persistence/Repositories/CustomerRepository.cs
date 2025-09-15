using ImageManagement.Application.Contracts.Persistence;
using ImageManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace ImageManagement.Persistence.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly BlogAppDbContext _context;

        public CustomerRepository(BlogAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Customer?> GetCustomerWithImagesAsync(int customerId)
        {
            return await _context.Customers
                .Include(c => c.Images.OrderByDescending(i => i.IsMainImage).ThenBy(i => i.DateCreated))
                .FirstOrDefaultAsync(c => c.Id == customerId);
        }

        public async Task<bool> CustomerExistsAsync(int customerId)
        {
            return await _context.Customers.AnyAsync(c => c.Id == customerId);
        }

        public async Task<int> GetImageCountForCustomerAsync(int customerId)
        {
            return await _context.ProfileImages.CountAsync(p => p.CustomerId == customerId);
        }
    }
}
