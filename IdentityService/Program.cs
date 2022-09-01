using System.Text;
using Common;
using IdentityService.Helpers;
using IdentityService.Models;
using IdentityService.Models.DataBase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll",
//         builder =>
//         {
//             builder.AllowAnyOrigin()
//                 .AllowAnyHeader()
//                 .AllowAnyMethod();
//         });
// });
builder.Services.AddCors();

// Add services to the container.
if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("IdentityService"));
else
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration["ConnectionString"]));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddErrorDescriber<PersianErrorDescriber>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IJwtTokenFactory, JwtTokenFactory>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer *****'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.Configure<IdentityOptions>(options => { options.Password.RequireNonAlphanumeric = false; });

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseCors(b => b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

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
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    userManager.CreateAsync(new ApplicationUser
    {
        Id = "caa445b9-56d9-432c-aae5-166f99ece010",
        Email = "cnazk98@gmail.com",
        BirthDate = new DateTime(1999, 1, 14),
        FirstName = "سینا",
        IsPublic = true,
        LastName = "ذاکر",
        PhoneNumber = "09107654560",
        UserName = "cnazk",
    }, "Sina9090");
}

app.Run();