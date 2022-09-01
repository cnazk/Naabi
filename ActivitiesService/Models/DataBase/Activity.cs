using System.ComponentModel.DataAnnotations;
using Common;

namespace ActivitiesService.Models.DataBase;

public class Activity : NonDeletableEntity
{
    [Required] public string Name { get; set; }
    [Required] public string InputName { get; set; }

    public int ActivityCategoryId { get; set; }
    public ActivityCategory ActivityCategory { get; set; }

    public bool CanSelectTime { get; set; } = true;

    public bool Disabled { get; set; } = false;

    public ICollection<UserActivity> UserActivities { get; set; } = new List<UserActivity>();
}