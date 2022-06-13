using Common;
using FriendsService.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("FriendsService"));
else
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration["ConnectionString"]));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

if (app.Environment.IsProduction())
{
    var @try = 0;
    using var scope = app.Services.CreateScope();
    var dataContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    while (@try < 5)
    {
        try
        {
            Helper.Log($"Trying to migrate database for {@try} time...");
            dataContext.Database.Migrate();
            break;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            @try++;
            Thread.Sleep(5000);
        }
    }
}

app.Run();