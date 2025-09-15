using ImageManagement.Application.Contracts.Persistence;
using ImageManagement.Application.Features.Images.CQRS.Commands;
using ImageManagement.Application.Responses;
using MediatR;

namespace ImageManagement.Application.Features.Images.CQRS.Handlers
{
    public class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteImageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                // Use the repository's DeleteImageAsync method which includes validation
                var deleted = await _unitOfWork.ProfileImageRepository.DeleteImageAsync(
                    request.ImageId, 
                    request.CustomerId, 
                    request.LeadId);

                if (deleted)
                {
                    await _unitOfWork.Save();
                    response.Success = true;
                    response.Message = "Image deleted successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Image not found or you don't have permission to delete it.";
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while deleting the image.";
                response.Errors.Add(ex.Message);
                return response;
            }
        }
    }
}
