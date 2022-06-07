using Microsoft.EntityFrameworkCore;

namespace ActivitiesService.Models;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}