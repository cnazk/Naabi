using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Models.DataBase;

public class ApplicationUser : IdentityUser
{
    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public bool IsPublic { get; set; } = true;
    [Required] public DateTime BirthDate { get; set; }
    public string? ImageUrl { get; set; }

    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
}