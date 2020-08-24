using System;
using GoingPlaces.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoingPlaces.API.Data
{
    //We have to make this class available as a service because we are going to consume it as
    // a service - It will be done in our startup.cs class
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        //We need a constructor and it should be public and not PROTECTED
        //We have to provide DBContext options and provide the class for the type
        public DataContext(DbContextOptions<DataContext> options) : base(options){}
        
        //Tell the DB Context class about the Entity
        //The name of this will be created for Table when we scaffold it
    
        //If the DataContext is changed you have to do a new migration
        //Stop the server when you do a migration else the build will fail
        //Use: dotnet ef database update to apply the migration
       // public  DbSet<User> Users { get; set; }
        public DbSet<EndDestination> EndDestinations { get; set; }
        public DbSet<DailyDistanceEntry> DailyDistanceEntries { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<DailyDistanceEntry>()
                .HasOne(d => d.User)
                .WithMany(u => u.DailyDistanceEntries)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}