namespace ActivitiesService.Models.Dto;

public class UserActivityWriteDto
{
    public int ActivityId { get; set; }
    public string Input { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}