using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RailwayReservation.Models;

namespace RailwayReservation.Data
{
    public class RailwayContext : IdentityDbContext<ApplicationUser>
    {
        public RailwayContext(DbContextOptions<RailwayContext> options) : base(options) { }

        public DbSet<Station> Stations { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<TrainSchedule> TrainSchedules { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Required for Identity

            // Configure TrainSchedule relationships
            modelBuilder.Entity<TrainSchedule>()
                .HasOne(ts => ts.Train)
                .WithMany()
                .HasForeignKey(ts => ts.TrainNo)
                .OnDelete(DeleteBehavior.Cascade); // If a train is deleted, delete schedules

            modelBuilder.Entity<TrainSchedule>()
                .HasOne(ts => ts.FromStation)
                .WithMany()
                .HasForeignKey(ts => ts.FromStationId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<TrainSchedule>()
                .HasOne(ts => ts.ToStation)
                .WithMany()
                .HasForeignKey(ts => ts.ToStationId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Configure Reservation relationships
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.TrainSchedule)
                .WithMany()
                .HasForeignKey(r => r.TrainScheduleId)
                .OnDelete(DeleteBehavior.Cascade); // If a train schedule is deleted, remove related reservations

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.FromStation)
                .WithMany()
                .HasForeignKey(r => r.FromStationId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.ToStation)
                .WithMany()
                .HasForeignKey(r => r.ToStationId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
        }
    }
}
