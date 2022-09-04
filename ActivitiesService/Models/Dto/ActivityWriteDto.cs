namespace ActivitiesService.Models.Dto;

public class ActivityWriteDto
{
    public string Name { get; set; }
    public bool Disabled { get; set; } = false;
    public string Unit { get; set; }
}