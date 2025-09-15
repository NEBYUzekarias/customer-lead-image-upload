using ImageManagement.Application.Contracts.Persistence;
using ImageManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace ImageManagement.Persistence.Repositories
{
    public class ProfileImageRepository : GenericRepository<ProfileImage>, IProfileImageRepository
    {
        private readonly BlogAppDbContext _context;

        public ProfileImageRepository(BlogAppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IList<ProfileImage>> GetByCustomerAsync(int customerId)
        {
            return await _context.ProfileImages
                .Where(p => p.CustomerId == customerId)
                .OrderByDescending(p => p.IsMainImage)
                .ThenBy(p => p.DateCreated)
                .ToListAsync();
        }

        public async Task<IList<ProfileImage>> GetByLeadAsync(int leadId)
        {
            return await _context.ProfileImages
                .Where(p => p.LeadId == leadId)
                .OrderByDescending(p => p.IsMainImage)
                .ThenBy(p => p.DateCreated)
                .ToListAsync();
        }

        public async Task<int> CountByCustomerAsync(int customerId)
        {
            return await _context.ProfileImages.CountAsync(p => p.CustomerId == customerId);
        }

        public async Task<int> CountByLeadAsync(int leadId)
        {
            return await _context.ProfileImages.CountAsync(p => p.LeadId == leadId);
        }

        public async Task<bool> CanAddMoreImagesAsync(int? customerId, int? leadId)
        {
            if (customerId.HasValue)
            {
                var count = await CountByCustomerAsync(customerId.Value);
                return count < 10;
            }
            
            if (leadId.HasValue)
            {
                var count = await CountByLeadAsync(leadId.Value);
                return count < 10;
            }
            
            return false;
        }

        public async Task<ProfileImage?> GetMainImageAsync(int? customerId, int? leadId)
        {
            if (customerId.HasValue)
            {
                return await _context.ProfileImages
                    .FirstOrDefaultAsync(p => p.CustomerId == customerId && p.IsMainImage);
            }
            
            if (leadId.HasValue)
            {
                return await _context.ProfileImages
                    .FirstOrDefaultAsync(p => p.LeadId == leadId && p.IsMainImage);
            }
            
            return null;
        }

        public async Task SetMainImageAsync(int imageId, int? customerId, int? leadId)
        {
            // First, unset all main images for this customer/lead
            if (customerId.HasValue)
            {
                var existingMainImages = await _context.ProfileImages
                    .Where(p => p.CustomerId == customerId && p.IsMainImage)
                    .ToListAsync();
                
                foreach (var img in existingMainImages)
                {
                    img.IsMainImage = false;
                }
            }
            
            if (leadId.HasValue)
            {
                var existingMainImages = await _context.ProfileImages
                    .Where(p => p.LeadId == leadId && p.IsMainImage)
                    .ToListAsync();
                
                foreach (var img in existingMainImages)
                {
                    img.IsMainImage = false;
                }
            }

            // Set the new main image
            var newMainImage = await _context.ProfileImages.FindAsync(imageId);
            if (newMainImage != null)
            {
                newMainImage.IsMainImage = true;
            }
        }

        public async Task<bool> DeleteImageAsync(int imageId, int? customerId, int? leadId)
        {
            var image = await _context.ProfileImages.FindAsync(imageId);
            
            if (image == null) return false;
            
            // Verify the image belongs to the specified customer or lead
            if (customerId.HasValue && image.CustomerId != customerId) return false;
            if (leadId.HasValue && image.LeadId != leadId) return false;
            
            _context.ProfileImages.Remove(image);
            return true;
        }
    }
}


