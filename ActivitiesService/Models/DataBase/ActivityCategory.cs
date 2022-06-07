using System.ComponentModel.DataAnnotations;

namespace ActivitiesService.Models.DataBase;

public class ActivityCategory : EntityWithId
{
    [Required] public string Name { get; set; }
    public bool Disabled { get; set; } = false;

    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}