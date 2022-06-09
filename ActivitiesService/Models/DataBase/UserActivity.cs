using System.ComponentModel.DataAnnotations;
using Common;

namespace ActivitiesService.Models.DataBase;

public class UserActivity : NonDeletableEntity
{
    [Required] public string UserId { get; set; }
    public int ActivityId { get; set; }
    public Activity Activity { get; set; }
    public string Input { get; set; }
}