using EDHWebApi.Model;
using Microsoft.EntityFrameworkCore;

namespace EDHWebApi.Persistance;

public class EDHContext : DbContext
{

    protected readonly IConfiguration _configuration;
    
    public DbSet<CompanyUser> CompanyUsers { get; set; }
    public DbSet<CustomerCompany> CustomerCompanies { get; set; }



    public EDHContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // optionsBuilder.UseSqlite("Data Source = EDH.db");
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("WebApiDatabase"));
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EDHContext).Assembly);


        
      
        // modelBuilder.Entity<CourseAssignment>()
        //   .HasKey(c => new { c.CourseID, c.InstructorID });
    }
 
}