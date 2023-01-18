using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> TodoIteApplicationUsers { get; set; } = null!;

    public DbSet<ApplicationUserRole> ApplicationUserRole { get; set; } = default!;
}