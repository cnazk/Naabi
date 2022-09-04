namespace ActivitiesService.Models.Dto;

public class UserActivityReadDto
{
    public ActivityReadDto Activity { get; set; }
    public string Input { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Id { get; set; }
}