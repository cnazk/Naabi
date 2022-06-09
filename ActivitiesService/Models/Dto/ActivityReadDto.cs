using Common;

namespace ActivitiesService.Models.Dto;

public class ActivityReadDto: EntityWithId
{
    public string Name { get; set; }
    public bool Disabled { get; set; } = false;
    public int ActivityCategoryId { get; set; }
}