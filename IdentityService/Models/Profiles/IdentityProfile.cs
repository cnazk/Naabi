using AutoMapper;
using IdentityService.Models.DataBase;
using IdentityService.Models.Dtos;

namespace IdentityService.Models.Profiles;

public class IdentityProfile: Profile
{
    public IdentityProfile()
    {
        CreateMap<ApplicationUser, UserInfoDto>();
    }
}