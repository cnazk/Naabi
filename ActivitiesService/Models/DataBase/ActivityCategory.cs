using System.ComponentModel.DataAnnotations;
using Common;

namespace ActivitiesService.Models.DataBase;

public class ActivityCategory : NonDeletableEntity
{
    [Required] public string Name { get; set; }
    public bool Disabled { get; set; } = false;

    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}