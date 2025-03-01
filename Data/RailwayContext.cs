using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RailwayReservation.Models; // Ensure Models namespace is correct

namespace RailwayReservation.Data  // ✅ Ensure this matches your project name
{
    public class RailwayContext : IdentityDbContext<ApplicationUser>
    {
        public RailwayContext(DbContextOptions<RailwayContext> options) : base(options) { }

        public DbSet<Station> Stations { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<TrainSchedule> TrainSchedules { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Cancellation> Cancellations { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Required for Identity

            // Ensure FromStationId and ToStationId do not create multiple cascade paths
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.FromStation)
                .WithMany()
                .HasForeignKey(r => r.FromStationId)
                .OnDelete(DeleteBehavior.Restrict); // ✅ Prevents cascade delete

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.ToStation)
                .WithMany()
                .HasForeignKey(r => r.ToStationId)
                .OnDelete(DeleteBehavior.Restrict); // ✅ Prevents cascade delete
        }
    }
}
