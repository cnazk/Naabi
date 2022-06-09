using Common;

namespace ActivitiesService.Models.Dto;

public class CategoryReadDto : EntityWithId
{
    public string Name { get; set; }
    public bool Disabled { get; set; } = false;
}