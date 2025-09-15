using ImageManagement.Domain;

namespace ImageManagement.Application.Contracts.Persistence
{
    public interface ILeadRepository : IGenericRepository<Lead>
    {
        Task<Lead?> GetLeadWithImagesAsync(int leadId);
        Task<bool> LeadExistsAsync(int leadId);
        Task<int> GetImageCountForLeadAsync(int leadId);
    }
}
