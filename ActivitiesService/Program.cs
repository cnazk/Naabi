using ActivitiesService.Models;
using ActivitiesService.Models.DataBase;
using Common;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Add services to the container.
if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("ActivitiesService"));
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

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

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
else
{
    using var scope = app.Services.CreateScope();
    var dataContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dataContext.ActivityCategories.Add(new ActivityCategory
    {
        Name = "ورزش",
        Id = 1,
        Disabled = false,
    });
    dataContext.ActivityCategories.Add(new ActivityCategory
    {
        Name = "بهبود زندگی",
        Id = 2,
        Disabled = false,
    });
    dataContext.Activities.Add(new Activity
    {
        Name = "پیاده‌روی",
        Disabled = false,
        Id = 1,
        ActivityCategoryId = 1,
        CanSelectTime = true,
        InputName = "چند دقیقه؟"
    });
    dataContext.Activities.Add(new Activity
    {
        Name = "دویدن",
        Disabled = false,
        Id = 2,
        ActivityCategoryId = 1,
        CanSelectTime = true,
        InputName = "چند دقیقه؟"
    });
    dataContext.Activities.Add(new Activity
    {
        Name = "خوردن آب",
        Disabled = false,
        Id = 3,
        ActivityCategoryId = 2,
        CanSelectTime = false,
        InputName = "چند لیوان؟"
    });
    dataContext.Activities.Add(new Activity
    {
        Name = "خواندن کتاب",
        Disabled = false,
        Id = 4,
        ActivityCategoryId = 2,
        CanSelectTime = false,
        InputName = "چند صفحه؟"
    });
    await dataContext.SaveChangesAsync();
}

app.Run();