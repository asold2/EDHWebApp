﻿using EDHWebApp.Model;
using Microsoft.EntityFrameworkCore;

namespace EDHWebApp.Persistance;

public class EDHContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }

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