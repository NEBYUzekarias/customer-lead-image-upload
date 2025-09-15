using AutoMapper;
using ImageManagement.Application.Features.Images.DTOs;
using ImageManagement.Domain;

namespace ImageManagement.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region ProfileImage Mappings

            CreateMap<ProfileImage, ImageResponseDto>().ReverseMap();
            CreateMap<ImageUploadDto, ProfileImage>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Lead, opt => opt.Ignore());

            #endregion ProfileImage
        }
    }
}