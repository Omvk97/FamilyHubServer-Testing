using System;
using API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<EventRepeatDetails> EventRepeatDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<WeekDay>();
            modelBuilder.HasPostgresEnum<FrequencyOption>();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Event>()
                .Property(e => e.Description)
                .HasDefaultValue("");

            modelBuilder.Entity<User>()
                .Property(u => u.ProfileColor)
                .HasDefaultValue("#808080");

            modelBuilder.Entity<UserEvent>()
            .HasKey(ue => new { ue.UserId, ue.EventId });


            modelBuilder.Entity<UserEvent>()
                .HasKey(ue => new { ue.UserId, ue.EventId });
            modelBuilder.Entity<UserEvent>()
                .HasOne(ue => ue.User)
                .WithMany(u => u.Events)
                .HasForeignKey(ue => ue.UserId);
            modelBuilder.Entity<UserEvent>()
                .HasOne(ue => ue.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(ue => ue.EventId);
        }
    }
}
