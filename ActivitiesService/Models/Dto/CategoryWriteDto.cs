namespace ActivitiesService.Models.Dto;

public class CategoryWriteDto
{
    public string Name { get; set; }
    public bool Disabled { get; set; } = false;
}