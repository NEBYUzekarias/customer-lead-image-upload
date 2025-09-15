using MediatR;
using ImageManagement.Application.Responses;

namespace ImageManagement.Application.Features.Images.CQRS.Commands
{
    public class DeleteImageCommand : IRequest<BaseCommandResponse>
    {
        public int ImageId { get; set; }
        public int? CustomerId { get; set; }
        public int? LeadId { get; set; }
    }
}
