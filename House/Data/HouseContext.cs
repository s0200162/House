using House.Areas.Identity.Data;
using House.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.Data
{
    public class HouseContext : IdentityDbContext<CustomUser>
    {
        public HouseContext(DbContextOptions<HouseContext> options)
            : base(options) 
        { 
        }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Profession> Profession { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Period> Period { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("house");

            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Customer>().Property(p => p.Firstname).IsRequired();
            modelBuilder.Entity<Customer>().Property(p => p.Lastname).IsRequired();
            modelBuilder.Entity<Customer>().Property(p => p.ProfessionID).IsRequired();

            modelBuilder.Entity<CustomUser>()
                .HasOne(c => c.Customer)
                .WithOne(c => c.CustomUser)
                .HasForeignKey<Customer>(c => c.UserID);

            modelBuilder.Entity<Room>().ToTable("Room");
            modelBuilder.Entity<Room>().Property(p => p.Description).IsRequired();
            modelBuilder.Entity<Room>().Property(p => p.LocationID).IsRequired();
            modelBuilder.Entity<Room>().Property(p => p.PriceHour).IsRequired();

            modelBuilder.Entity<Reservation>().ToTable("Reservation");
            modelBuilder.Entity<Reservation>().Property(p => p.CustomerID).IsRequired();
            modelBuilder.Entity<Reservation>().Property(p => p.RoomID).IsRequired();
            modelBuilder.Entity<Reservation>().Property(p => p.Date).IsRequired();
            modelBuilder.Entity<Reservation>().Property(p => p.PeriodID).IsRequired();

            modelBuilder.Entity<Profession>().ToTable("Profession");
            modelBuilder.Entity<Profession>().Property(p => p.Description).IsRequired();

            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<Location>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Location>().Property(p => p.Place).IsRequired();
            modelBuilder.Entity<Location>().Property(p => p.Adress).IsRequired();
            modelBuilder.Entity<Location>().Ignore(p => p.NameAndPlace);

            modelBuilder.Entity<Invoice>().ToTable("Invoice");
            modelBuilder.Entity<Invoice>().Property(p => p.Date).IsRequired();

            modelBuilder.Entity<Period>().ToTable("Period");
            modelBuilder.Entity<Period>().Property(p => p.Hour).IsRequired();

        }
    }
}
