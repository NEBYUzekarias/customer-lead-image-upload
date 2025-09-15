using ImageManagement.Application.Contracts.Persistence;
using ImageManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace ImageManagement.Persistence.Repositories
{
    public class LeadRepository : GenericRepository<Lead>, ILeadRepository
    {
        private readonly BlogAppDbContext _context;

        public LeadRepository(BlogAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Lead?> GetLeadWithImagesAsync(int leadId)
        {
            return await _context.Leads
                .Include(l => l.Images.OrderByDescending(i => i.IsMainImage).ThenBy(i => i.DateCreated))
                .FirstOrDefaultAsync(l => l.Id == leadId);
        }

        public async Task<bool> LeadExistsAsync(int leadId)
        {
            return await _context.Leads.AnyAsync(l => l.Id == leadId);
        }

        public async Task<int> GetImageCountForLeadAsync(int leadId)
        {
            return await _context.ProfileImages.CountAsync(p => p.LeadId == leadId);
        }
    }
}
