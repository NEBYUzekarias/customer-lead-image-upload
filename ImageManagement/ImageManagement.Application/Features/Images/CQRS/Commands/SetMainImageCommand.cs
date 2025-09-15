using MediatR;
using ImageManagement.Application.Features.Images.DTOs;
using ImageManagement.Application.Responses;

namespace ImageManagement.Application.Features.Images.CQRS.Commands
{
    public class SetMainImageCommand : IRequest<BaseCommandResponse>
    {
        public SetMainImageRequest Request { get; set; } = new SetMainImageRequest();
    }
}
