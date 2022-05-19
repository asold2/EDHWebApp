using EDHWebApp.Model;
using Microsoft.EntityFrameworkCore;

namespace EDHWebApp.Persistance;

public class EDHContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<RegisteredUser> RegisteredUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = EDH.db");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<RegisteredUser>().ToTable("RegisteredUsers");
        modelBuilder.Entity<Admin>().ToTable("Admins");
        modelBuilder.Entity<Company>().ToTable("Companies");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EDHContext).Assembly);

        // modelBuilder.Entity<CourseAssignment>()
        //   .HasKey(c => new { c.CourseID, c.InstructorID });
    }
 
}