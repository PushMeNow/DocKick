using AutoMapper;
using DocKick.DataTransferModels.Users;
using DocKick.Entities.Users;

namespace DocKick.Mapper
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<User, UserProfileModel>()
                .ForMember(q => q.UserId, q => q.MapFrom(w => w.Id))
                .ForMember(q => q.Phone, q => q.MapFrom(w => w.PhoneNumber))
                .ReverseMap()
                .ForMember(q => q.Id, q => q.Ignore())
                .ForMember(q => q.Email, q => q.Ignore())
                .ForMember(q => q.PhoneNumber, q => q.MapFrom(w => w.Phone));
            
            CreateMap<UserProfileRequest, User>()
                .ForMember(q => q.Id, q => q.Ignore())
                .ForMember(q => q.Email, q => q.Ignore())
                .ForMember(q => q.PhoneNumber, q => q.MapFrom(w => w.Phone));
        }
    }
}