using Buses.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba.Data
{
    public class BusContext:DbContext
    {
        public DbSet<Transport> Transports { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<MiniBus> MiniBus { get; set; }
        public DbSet<BusStop> BusStops { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public BusContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transport>().Property(t=>t.Id).UseIdentityColumn();
            modelBuilder.Entity<BusStop>().Property(t => t.Id).UseIdentityColumn();
            modelBuilder.Entity<BusStop>().HasAlternateKey(b => b.Location);
            modelBuilder.Entity<BusStop>()
                .HasMany(s => s.Routes)
                .WithMany(r => r.Stops)
                .UsingEntity(j => j.ToTable("RouteStops"));
            modelBuilder.Entity<Ticket>().Property(t => t.Price).HasPrecision(9, 2);
            modelBuilder.Entity<Ticket>().Property(t=>t.Number).UseIdentityColumn();
            modelBuilder.Entity<Journey>().HasKey(t=>t.Id);
            modelBuilder.Entity<Journey>().Property(t => t.Id).UseIdentityColumn();

        }
    }
}
