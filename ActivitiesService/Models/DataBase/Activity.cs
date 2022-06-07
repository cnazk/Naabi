namespace ActivitiesService.Models.DataBase;

public class Activity : EntityWithId
{
    public string Name { get; set; }

    public int ActivityCategoryId { get; set; }
    public ActivityCategory ActivityCategory { get; set; }

    public ICollection<UserActivity> UserActivities { get; set; } = new List<UserActivity>();
}