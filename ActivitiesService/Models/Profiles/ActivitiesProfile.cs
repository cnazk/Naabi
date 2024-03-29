using ActivitiesService.Models.DataBase;
using ActivitiesService.Models.Dto;
using AutoMapper;

namespace ActivitiesService.Models.Profiles;

public class ActivitiesProfile : Profile
{
    public ActivitiesProfile()
    {
        CreateMap<ActivityCategory, CategoryReadDto>();
        CreateMap<CategoryWriteDto, ActivityCategory>();
        CreateMap<Activity, ActivityReadDto>();
        CreateMap<ActivityWriteDto, Activity>();
        CreateMap<UserActivityWriteDto, UserActivity>();
        CreateMap<UserActivity, UserActivityReadDto>();
    }
}