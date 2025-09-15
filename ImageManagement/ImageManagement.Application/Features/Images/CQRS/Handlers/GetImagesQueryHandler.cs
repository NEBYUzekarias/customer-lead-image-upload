using ImageManagement.Application.Contracts.Persistence;
using ImageManagement.Application.Features.Images.CQRS.Queries;
using ImageManagement.Application.Features.Images.DTOs;
using MediatR;
using AutoMapper;

namespace ImageManagement.Application.Features.Images.CQRS.Handlers
{
    public class GetImagesQueryHandler : IRequestHandler<GetImagesQuery, IList<ImageResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetImagesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IList<ImageResponseDto>> Handle(GetImagesQuery request, CancellationToken cancellationToken)
        {
            if ((request.CustomerId == null && request.LeadId == null) ||
                (request.CustomerId != null && request.LeadId != null))
            {
                throw new ArgumentException("Provide either CustomerId or LeadId, but not both.");
            }

            IList<Domain.ProfileImage> images;

            if (request.CustomerId.HasValue)
            {
                images = await _unitOfWork.ProfileImageRepository.GetByCustomerAsync(request.CustomerId.Value);
            }
            else
            {
                images = await _unitOfWork.ProfileImageRepository.GetByLeadAsync(request.LeadId!.Value);
            }

            return _mapper.Map<IList<ImageResponseDto>>(images);
        }
    }
}
