using EDHWebApi.Model;
using Microsoft.EntityFrameworkCore;

namespace EDHWebApi.Persistance;

public class EDHContext : DbContext
{
    
    
    
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }

    // public EDHContext()
    // {
    //     if (this.Users.FirstOrDefault(u => u.UserName == "admin")==null || this.Users.FirstOrDefault(u => u.UserName == "Admin")==null)
    //     {
    //         this.Users.Add(new User("admin", "admin", "admin@email.com", true, true, "Admin", "admin", "password", 0, 0));
    //     }   
    // }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = EDH.db");
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Company>().ToTable("Companies");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EDHContext).Assembly);


        
      
        // modelBuilder.Entity<CourseAssignment>()
        //   .HasKey(c => new { c.CourseID, c.InstructorID });
    }
 
}