using HotelBookingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Data;

public class HotelBookingContext(DbContextOptions<HotelBookingContext> options) : DbContext(options)
{
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>()
            .HasMany(c => c.Cities)
            .WithOne(c => c.Country)
            .HasForeignKey(c => c.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<City>()
            .HasMany(c => c.Hotels)
            .WithOne(h => h.City)
            .HasForeignKey(h => h.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes to improve query performance
        modelBuilder.Entity<Country>()
            .HasIndex(c => c.Name);

        modelBuilder.Entity<Country>()
            .HasIndex(c => c.Code);

        modelBuilder.Entity<City>()
            .HasIndex(c => c.Name);

        modelBuilder.Entity<City>()
            .HasIndex(c => c.CountryId);

        modelBuilder.Entity<Hotel>()
            .HasIndex(h => h.CityId);

        modelBuilder.Entity<Room>()
            .HasIndex(r => r.HotelId);

        modelBuilder.Entity<Room>()
            .HasIndex(r => r.IsAvailable);

        modelBuilder.Entity<Booking>()
            .HasIndex(b => b.RoomId);

        // Composite index to speed up date range checks for a room
        modelBuilder.Entity<Booking>()
            .HasIndex(b => new { b.RoomId, b.CheckInDate, b.CheckOutDate });
    }
}