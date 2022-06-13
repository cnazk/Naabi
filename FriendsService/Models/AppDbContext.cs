using FriendsService.Models.DataBase;
using Microsoft.EntityFrameworkCore;

namespace FriendsService.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<FriendRequest> FriendRequests { get; set; }
}