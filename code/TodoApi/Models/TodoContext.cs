using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TodoItem>()
            .Property(t => t.UserId)
            .IsRequired();

        modelBuilder.Entity<TodoItem>()
            .HasOne(u => u.User)
            .WithMany(t => t.TodoItems)
            .HasForeignKey(t => t.UserId);
    }
}
