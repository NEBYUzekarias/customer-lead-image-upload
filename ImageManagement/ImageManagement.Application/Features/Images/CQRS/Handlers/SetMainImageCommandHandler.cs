using ImageManagement.Application.Contracts.Persistence;
using ImageManagement.Application.Features.Images.CQRS.Commands;
using ImageManagement.Application.Responses;
using MediatR;

namespace ImageManagement.Application.Features.Images.CQRS.Handlers
{
    public class SetMainImageCommandHandler : IRequestHandler<SetMainImageCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SetMainImageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(SetMainImageCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                // Validate that either CustomerId or LeadId is provided, but not both
                if ((request.Request.CustomerId == null && request.Request.LeadId == null) ||
                    (request.Request.CustomerId != null && request.Request.LeadId != null))
                {
                    response.Success = false;
                    response.Message = "Provide either CustomerId or LeadId, but not both.";
                    return response;
                }

                // Verify the image exists and belongs to the specified customer/lead
                var image = await _unitOfWork.ProfileImageRepository.Get(request.Request.ImageId);
                if (image == null)
                {
                    response.Success = false;
                    response.Message = "Image not found.";
                    return response;
                }

                // Verify ownership
                if (request.Request.CustomerId.HasValue && image.CustomerId != request.Request.CustomerId)
                {
                    response.Success = false;
                    response.Message = "Image does not belong to the specified customer.";
                    return response;
                }

                if (request.Request.LeadId.HasValue && image.LeadId != request.Request.LeadId)
                {
                    response.Success = false;
                    response.Message = "Image does not belong to the specified lead.";
                    return response;
                }

                // Set as main image
                await _unitOfWork.ProfileImageRepository.SetMainImageAsync(
                    request.Request.ImageId,
                    request.Request.CustomerId,
                    request.Request.LeadId);

                await _unitOfWork.Save();

                response.Success = true;
                response.Message = "Main image set successfully.";
                response.Id = request.Request.ImageId;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while setting the main image.";
                response.Errors.Add(ex.Message);
                return response;
            }
        }
    }
}
