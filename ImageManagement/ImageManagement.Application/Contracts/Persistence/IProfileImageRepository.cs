using ImageManagement.Domain;

namespace ImageManagement.Application.Contracts.Persistence
{
    public interface IProfileImageRepository : IGenericRepository<ProfileImage>
    {
        Task<IList<ProfileImage>> GetByCustomerAsync(int customerId);
        Task<IList<ProfileImage>> GetByLeadAsync(int leadId);
        Task<int> CountByCustomerAsync(int customerId);
        Task<int> CountByLeadAsync(int leadId);
        Task<bool> CanAddMoreImagesAsync(int? customerId, int? leadId);
        Task<ProfileImage?> GetMainImageAsync(int? customerId, int? leadId);
        Task SetMainImageAsync(int imageId, int? customerId, int? leadId);
        Task<bool> DeleteImageAsync(int imageId, int? customerId, int? leadId);
    }
}


