using MediatR;
using ImageManagement.Application.Features.Images.DTOs;

namespace ImageManagement.Application.Features.Images.CQRS.Queries
{
    public class GetImagesQuery : IRequest<IList<ImageResponseDto>>
    {
        public int? CustomerId { get; set; }
        public int? LeadId { get; set; }
    }
}
