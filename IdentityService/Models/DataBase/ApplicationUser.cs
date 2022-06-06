using Microsoft.AspNetCore.Identity;

namespace IdentityService.Models.DataBase;

public class ApplicationUser: IdentityUser
{
    public DateTime RegisterDate { get; set; } = DateTime.Now;
}