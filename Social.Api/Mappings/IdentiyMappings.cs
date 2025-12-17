using AutoMapper;
using Social.Api.Contracts.Identity.Request;
using Social.Application.Identity.Commands;

namespace Social.Api.Mappings
{
    public class IdentityMappings : Profile
    {
        public IdentityMappings()
        {
            CreateMap<RegisterUserRequest, RegisterIdentity>();
            CreateMap<LoginRequest , LoginIdentity>();
        }
    }
}
