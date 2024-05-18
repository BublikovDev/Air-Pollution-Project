using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Models.Auth.Tokens;
using Shared.Models.Map;
using Shared.Models.User;

namespace Server.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        //public DbSet<Country> Countries { get; set; }
        //public DbSet<Location> Locations { get; set; }
        //public DbSet<Sensor> Sensors { get; set; }
       


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ////modelBuilder.Entity<Country>()
            ////    .HasMany(c => c.Locations)
            ////    .WithOne(l => l.Country)
            ////    .HasForeignKey(l => l.CountryId);

            ////modelBuilder.Entity<Location>()
            ////    .HasMany(l => l.Sensors)
            ////    .WithOne(s => s.Location)
            ////    .HasForeignKey(s => s.LocationId);


            base.OnModelCreating(modelBuilder);
        }
    }
}
