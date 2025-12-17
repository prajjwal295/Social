using AutoMapper;
using Social.Api.Contracts.UserProfile.Request;
using Social.Api.Contracts.UserProfile.Response;
using Social.Application.UserProfiles.Commands;
using Social.Domain.Aggregates.UserProfileAggegate;

namespace Social.Api.Mappings
{
    public class ProfileMappings : Profile
    {
        public ProfileMappings() {
            CreateMap<UserProfile,UserProfileResponse>();
            CreateMap<UserProfileCreateUpdate, CreateUserCommand>();
            CreateMap<UserProfileCreateUpdate, UpdateUserProfileBasicInfoCommand>();
            CreateMap<BasicInfo , BasicInformation>();
        }
    }
}
