using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ResourceBookingSystem.Models;

namespace ResourceBookingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Setup one-to-many relationship
            modelBuilder.Entity<Resource>()
                .HasMany(r => r.Bookings)
                .WithOne(b => b.Resource)
                .HasForeignKey(b => b.ResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed data
            modelBuilder.Entity<Resource>().HasData(
                new Resource
                {
                    Id = 1,
                    Name = "Conference Room A",
                    Description = "Spacious room with projector and Wi-Fi",
                    Location = "Building 1, Floor 2",
                    Capacity = 10,
                    IsAvailable = true
                },
                new Resource
                {
                    Id = 2,
                    Name = "Computer Lab",
                    Description = "Lab with 20 high-performance PCs",
                    Location = "Building 2, Floor 1",
                    Capacity = 20,
                    IsAvailable = true
                },
                new Resource
                {
                    Id = 3,
                    Name = "Boardroom",
                    Description = "Executive boardroom with teleconferencing facilities",
                    Location = "Main Admin Building",
                    Capacity = 12,
                    IsAvailable = true
                }
            );
        }


    }
}
