using System.ComponentModel.DataAnnotations;

namespace ActivitiesService.Models.DataBase;

public class UserActivity: EntityWithId
{
    [Required]
    public string UserId { get; set; }
    public int ActivityId { get; set; }
    public Activity Activity { get; set; }
}