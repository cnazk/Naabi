using System.ComponentModel.DataAnnotations;

namespace ActivitiesService.Models.DataBase;

public class EntityWithId
{
    [Key]
    public int Id { get; set; }
}