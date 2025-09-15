using ImageManagement.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace ImageManagement.Domain
{
    public class ProfileImage : BaseDomainEntity
    {
        [Required]
        public string Base64Data { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ContentType { get; set; }

        [MaxLength(255)]
        public string? FileName { get; set; }

        public long FileSize { get; set; } // Size in bytes

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsMainImage { get; set; } = false;

        // Foreign key relationships - only one should be set
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int? LeadId { get; set; }
        public Lead? Lead { get; set; }

        // Business validation
        public bool IsValidImageType => !string.IsNullOrEmpty(ContentType) && 
            (ContentType.StartsWith("image/jpeg") || 
             ContentType.StartsWith("image/png") || 
             ContentType.StartsWith("image/gif") ||
             ContentType.StartsWith("image/webp"));
    }
}


