using HotelBookingApi.Models;

namespace HotelBookingApi.Data;

public static class SeedData
{
    public static void Initialize(HotelBookingContext context)
    {
        if (!context.Countries.Any())
        {
            var countries = new List<Country>
            {
                new() { Name = "United States", Code = "US" },
                new() { Name = "France", Code = "FR" },
                new() { Name = "Japan", Code = "JP" },
                new() { Name = "Australia", Code = "AU" },
                new() { Name = "United Kingdom", Code = "GB" },
                new() { Name = "Germany", Code = "DE" },
                new() { Name = "Italy", Code = "IT" },
                new() { Name = "United Arab Emirates", Code = "AE" }
            };

            context.Countries.AddRange(countries);
            context.SaveChanges();

            var cities = new List<City>
            {
                new() { Name = "New York", CountryId = countries[0].Id },
                new() { Name = "Miami", CountryId = countries[0].Id },
                new() { Name = "Denver", CountryId = countries[0].Id },
                new() { Name = "Paris", CountryId = countries[1].Id },
                new() { Name = "Tokyo", CountryId = countries[2].Id },
                new() { Name = "Sydney", CountryId = countries[3].Id },
                new() { Name = "London", CountryId = countries[4].Id },
                new() { Name = "Berlin", CountryId = countries[5].Id },
                new() { Name = "Rome", CountryId = countries[6].Id },
                new() { Name = "Dubai", CountryId = countries[7].Id }
            };

            context.Cities.AddRange(cities);
            context.SaveChanges();

            var hotels = new List<Hotel>
            {
                new() {
                    Name = "Grand Hotel",
                    Address = "123 Main Street",
                    CityId = cities[0].Id,
                    Rating = 5,
                    Rooms =
                    [
                        new() { RoomNumber = "101", Type = "Standard", PricePerNight = 100 },
                        new() { RoomNumber = "102", Type = "Deluxe", PricePerNight = 150 },
                        new() { RoomNumber = "103", Type = "Suite", PricePerNight = 250 }
                    ]
                },
                new() {
                    Name = "Seaside Resort",
                    Address = "456 Beach Road",
                    CityId = cities[1].Id,
                    Rating = 4,
                    Rooms =
                    [
                        new() { RoomNumber = "201", Type = "Standard", PricePerNight = 120 },
                        new() { RoomNumber = "202", Type = "Deluxe", PricePerNight = 180 }
                    ]
                },
                new() {
                    Name = "Mountain View Lodge",
                    Address = "789 Alpine Way",
                    CityId = cities[2].Id,
                    Rating = 4,
                    Rooms =
                    [
                        new() { RoomNumber = "301", Type = "Standard", PricePerNight = 90 },
                        new() { RoomNumber = "302", Type = "Deluxe", PricePerNight = 140 },
                        new() { RoomNumber = "303", Type = "Suite", PricePerNight = 220 }
                    ]
                },
                new() {
                    Name = "Parisian Elegance",
                    Address = "12 Rue de Rivoli",
                    CityId = cities[3].Id,
                    Rating = 5,
                    Rooms =
                    [
                        new() { RoomNumber = "401", Type = "Standard", PricePerNight = 150 },
                        new() { RoomNumber = "402", Type = "Deluxe", PricePerNight = 250 },
                        new() { RoomNumber = "403", Type = "Suite", PricePerNight = 400 }
                    ]
                },
                new() {
                    Name = "Tokyo Tower Hotel",
                    Address = "1-2-3 Minato-ku",
                    CityId = cities[4].Id,
                    Rating = 5,
                    Rooms =
                    [
                        new() { RoomNumber = "501", Type = "Standard", PricePerNight = 200 },
                        new() { RoomNumber = "502", Type = "Deluxe", PricePerNight = 300 },
                        new() { RoomNumber = "503", Type = "Suite", PricePerNight = 500 }
                    ]
                },
                new() {
                    Name = "Sydney Harbour View",
                    Address = "100 Circular Quay",
                    CityId = cities[5].Id,
                    Rating = 4,
                    Rooms =
                    [
                        new() { RoomNumber = "601", Type = "Standard", PricePerNight = 180 },
                        new() { RoomNumber = "602", Type = "Deluxe", PricePerNight = 280 }
                    ]
                },
                new() {
                    Name = "London Bridge Hotel",
                    Address = "1 Bridge Street",
                    CityId = cities[6].Id,
                    Rating = 4,
                    Rooms =
                    [
                        new() { RoomNumber = "701", Type = "Standard", PricePerNight = 160 },
                        new() { RoomNumber = "702", Type = "Deluxe", PricePerNight = 240 },
                        new() { RoomNumber = "703", Type = "Suite", PricePerNight = 350 }
                    ]
                },
                new() {
                    Name = "Berlin Central",
                    Address = "100 Friedrichstraße",
                    CityId = cities[7].Id,
                    Rating = 4,
                    Rooms =
                    [
                        new() { RoomNumber = "801", Type = "Standard", PricePerNight = 110 },
                        new() { RoomNumber = "802", Type = "Deluxe", PricePerNight = 170 }
                    ]
                },
                new() {
                    Name = "Rome Imperial",
                    Address = "Via del Corso 1",
                    CityId = cities[8].Id,
                    Rating = 5,
                    Rooms =
                    [
                        new() { RoomNumber = "901", Type = "Standard", PricePerNight = 130 },
                        new() { RoomNumber = "902", Type = "Deluxe", PricePerNight = 200 },
                        new() { RoomNumber = "903", Type = "Suite", PricePerNight = 300 }
                    ]
                },
                new() {
                    Name = "Dubai Skyline",
                    Address = "Sheikh Zayed Road",
                    CityId = cities[9].Id,
                    Rating = 5,
                    Rooms =
                    [
                        new() { RoomNumber = "1001", Type = "Standard", PricePerNight = 250 },
                        new() { RoomNumber = "1002", Type = "Deluxe", PricePerNight = 400 },
                        new() { RoomNumber = "1003", Type = "Suite", PricePerNight = 600 }
                    ]
                }
            };

            context.Hotels.AddRange(hotels);
            context.SaveChanges();
        }
    }
}