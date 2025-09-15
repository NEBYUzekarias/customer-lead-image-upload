using ImageManagement.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace ImageManagement.Domain
{
    public class Lead : BaseDomainEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Company { get; set; }

        [MaxLength(50)]
        public string? Source { get; set; } // Where the lead came from

        public DateTime? ContactDate { get; set; }

        public ICollection<ProfileImage> Images { get; set; } = new List<ProfileImage>();

        // Business rule: Maximum 10 images per lead
        public bool CanAddMoreImages => Images?.Count < 10;
        public int RemainingImageSlots => Math.Max(0, 10 - (Images?.Count ?? 0));
    }
}


