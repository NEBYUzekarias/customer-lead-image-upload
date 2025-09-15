using MediatR;
using ImageManagement.Application.Features.Images.DTOs;
using ImageManagement.Application.Responses;

namespace ImageManagement.Application.Features.Images.CQRS.Commands
{
    public class UploadImagesCommand : IRequest<BaseCommandResponse>
    {
        public UploadImagesRequest Request { get; set; } = new UploadImagesRequest();
    }
}
