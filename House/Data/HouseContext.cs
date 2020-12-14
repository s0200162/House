﻿using House.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.Data
{
    public class HouseContext : DbContext
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("house");

            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Customer>().Property(p => p.Firstname).IsRequired();
            modelBuilder.Entity<Customer>().Property(p => p.Lastname).IsRequired();
            modelBuilder.Entity<Customer>().Property(p => p.Firstname).IsRequired();
            modelBuilder.Entity<Customer>().Property(p => p.Email).IsRequired();
            modelBuilder.Entity<Customer>().Property(p => p.ProfessionID).IsRequired();

            modelBuilder.Entity<Room>().ToTable("Room");
            modelBuilder.Entity<Room>().Property(p => p.Description).IsRequired();
            modelBuilder.Entity<Room>().Property(p => p.LocationID).IsRequired();
            modelBuilder.Entity<Room>().Property(p => p.PriceHour).IsRequired();

            modelBuilder.Entity<Reservation>().ToTable("Reservation");
            modelBuilder.Entity<Reservation>().Property(p => p.CustomerID).IsRequired();
            modelBuilder.Entity<Reservation>().Property(p => p.Date).IsRequired();
            modelBuilder.Entity<Reservation>().Property(p => p.BeginTime).IsRequired();
            modelBuilder.Entity<Reservation>().Property(p => p.EndTime).IsRequired();

            modelBuilder.Entity<Profession>().ToTable("Profession");
            modelBuilder.Entity<Profession>().Property(p => p.Description).IsRequired();

            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<Location>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Location>().Property(p => p.Adress).IsRequired();

            modelBuilder.Entity<Invoice>().ToTable("Invoice");
            modelBuilder.Entity<Invoice>().Property(p => p.Date).IsRequired();

        }
    }
}