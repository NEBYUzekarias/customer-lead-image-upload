using ImageManagement.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace ImageManagement.Domain
{
    public class Customer : BaseDomainEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        public ICollection<ProfileImage> Images { get; set; } = new List<ProfileImage>();

        // Business rule: Maximum 10 images per customer
        public bool CanAddMoreImages => Images?.Count < 10;
        public int RemainingImageSlots => Math.Max(0, 10 - (Images?.Count ?? 0));
    }
}


