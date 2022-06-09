using ActivitiesService.Models.DataBase;
using Microsoft.EntityFrameworkCore;

namespace ActivitiesService.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ActivityCategory> ActivityCategories { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<UserActivity> UserActivities { get; set; }
}