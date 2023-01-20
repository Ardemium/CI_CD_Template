using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config.GetValue<string>("JWT:Issuer"),
        ValidAudience = config.GetValue<string>("JWT:Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("JWT:SigningKey")))
    };
});

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { 
    Title = "My API", 
    Version = "v1" 
  });
  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
    In = ParameterLocation.Header, 
    Description = "Insert \"Bearer <JWT>\" into field",
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer"
  });
  c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   { 
     new OpenApiSecurityScheme 
     { 
       Reference = new OpenApiReference 
       { 
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer" 
       } 
      },
      new string[] { } 
    } 
  });
});

builder.Services.AddSwaggerGen();

using (var serviceScope = builder.Services.BuildServiceProvider().CreateScope())
{
    var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
    var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

    // Create the "root" role if it doesn't already exist
    if (!await roleManager.RoleExistsAsync("root"))
    {
        var role = new IdentityRole("root");
        await roleManager.CreateAsync(role);
    }

    // Create the "admin" role if it doesn't already exist
    if (!await roleManager.RoleExistsAsync("admin"))
    {
        var role = new IdentityRole("admin");
        await roleManager.CreateAsync(role);
    }

    // Create the root user if it doesn't already exist
    var rootUser = await userManager.FindByNameAsync("root");
    if (rootUser == null)
    {
        rootUser = new ApplicationUser { UserName = "root"};
        await userManager.CreateAsync(rootUser, "Root123!");
        await userManager.AddToRoleAsync(rootUser, "root");
    }
}


var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
