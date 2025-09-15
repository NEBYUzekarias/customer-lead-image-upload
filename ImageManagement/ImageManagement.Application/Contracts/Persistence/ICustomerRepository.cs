using ImageManagement.Domain;

namespace ImageManagement.Application.Contracts.Persistence
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer?> GetCustomerWithImagesAsync(int customerId);
        Task<bool> CustomerExistsAsync(int customerId);
        Task<int> GetImageCountForCustomerAsync(int customerId);
    }
}
