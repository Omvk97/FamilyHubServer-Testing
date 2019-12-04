using System;
using System.Collections.Generic;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Credential> Credentials { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Recipe> Recipes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Credential>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Event>()
                .Property(e => e.Description)
                .HasDefaultValue("");

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

            modelBuilder.Entity<EventRepeatDetails>()
                .Property(e => e.WeekDays)
                .HasConversion(v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<ICollection<DayOfWeek>>(v));

            modelBuilder.Entity<EventRepeatDetails>()
                .Property(e => e.Exceptions)
                .HasConversion(v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<ICollection<EventRepeatDetails.RepeatException>>(v));
                
        }
    }
}
