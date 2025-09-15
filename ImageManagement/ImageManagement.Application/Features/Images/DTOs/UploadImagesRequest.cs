using System.ComponentModel.DataAnnotations;

namespace ImageManagement.Application.Features.Images.DTOs
{
    public class UploadImagesRequest
    {
        public int? CustomerId { get; set; }
        public int? LeadId { get; set; }
        
        [Required]
        [MinLength(1, ErrorMessage = "At least one image is required")]
        public IList<ImageUploadDto> Images { get; set; } = new List<ImageUploadDto>();
    }

    public class ImageUploadDto
    {
        [Required]
        public string Base64Data { get; set; } = string.Empty;
        
        public string? ContentType { get; set; }
        public string? FileName { get; set; }
        public string? Description { get; set; }
        public bool IsMainImage { get; set; } = false;
    }

    public class ImageResponseDto
    {
        public int Id { get; set; }
        public string Base64Data { get; set; } = string.Empty;
        public string? ContentType { get; set; }
        public string? FileName { get; set; }
        public string? Description { get; set; }
        public bool IsMainImage { get; set; }
        public long FileSize { get; set; }
        public DateTime DateCreated { get; set; }
        public int? CustomerId { get; set; }
        public int? LeadId { get; set; }
    }

    public class SetMainImageRequest
    {
        [Required]
        public int ImageId { get; set; }
        public int? CustomerId { get; set; }
        public int? LeadId { get; set; }
    }
}
