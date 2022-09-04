using Common;

namespace ActivitiesService.Models.Dto;

public class ActivityReadDto : EntityWithId
{
    public string Name { get; set; }
    public string InputName { get; set; }
    public bool Disabled { get; set; }
    public int ActivityCategoryId { get; set; }
    public bool CanSelectTime { get; set; }
    public CategoryReadDto ActivityCategory { get; set; }
    public string Unit { get; set; }
}