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
                new Country { Name = "United States", Code = "US" },
                new Country { Name = "France", Code = "FR" },
                new Country { Name = "Japan", Code = "JP" },
                new Country { Name = "Australia", Code = "AU" },
                new Country { Name = "United Kingdom", Code = "GB" },
                new Country { Name = "Germany", Code = "DE" },
                new Country { Name = "Italy", Code = "IT" },
                new Country { Name = "United Arab Emirates", Code = "AE" }
            };

            context.Countries.AddRange(countries);
            context.SaveChanges();

            var cities = new List<City>
            {
                new City { Name = "New York", CountryId = countries[0].Id },
                new City { Name = "Miami", CountryId = countries[0].Id },
                new City { Name = "Denver", CountryId = countries[0].Id },
                new City { Name = "Paris", CountryId = countries[1].Id },
                new City { Name = "Tokyo", CountryId = countries[2].Id },
                new City { Name = "Sydney", CountryId = countries[3].Id },
                new City { Name = "London", CountryId = countries[4].Id },
                new City { Name = "Berlin", CountryId = countries[5].Id },
                new City { Name = "Rome", CountryId = countries[6].Id },
                new City { Name = "Dubai", CountryId = countries[7].Id }
            };

            context.Cities.AddRange(cities);
            context.SaveChanges();

            var hotels = new List<Hotel>
            {
                new Hotel
                {
                    Name = "Grand Hotel",
                    Address = "123 Main Street",
                    CityId = cities[0].Id,
                    Rating = 5,
                    Rooms = new List<Room>
                    {
                        new Room { RoomNumber = "101", Type = "Standard", PricePerNight = 100 },
                        new Room { RoomNumber = "102", Type = "Deluxe", PricePerNight = 150 },
                        new Room { RoomNumber = "103", Type = "Suite", PricePerNight = 250 }
                    }
                },
                new Hotel
                {
                    Name = "Seaside Resort",
                    Address = "456 Beach Road",
                    CityId = cities[1].Id,
                    Rating = 4,
                    Rooms = new List<Room>
                    {
                        new Room { RoomNumber = "201", Type = "Standard", PricePerNight = 120 },
                        new Room { RoomNumber = "202", Type = "Deluxe", PricePerNight = 180 }
                    }
                },
                new Hotel
                {
                    Name = "Mountain View Lodge",
                    Address = "789 Alpine Way",
                    CityId = cities[2].Id,
                    Rating = 4,
                    Rooms = new List<Room>
                    {
                        new Room { RoomNumber = "301", Type = "Standard", PricePerNight = 90 },
                        new Room { RoomNumber = "302", Type = "Deluxe", PricePerNight = 140 },
                        new Room { RoomNumber = "303", Type = "Suite", PricePerNight = 220 }
                    }
                },
                new Hotel
                {
                    Name = "Parisian Elegance",
                    Address = "12 Rue de Rivoli",
                    CityId = cities[3].Id,
                    Rating = 5,
                    Rooms = new List<Room>
                    {
                        new Room { RoomNumber = "401", Type = "Standard", PricePerNight = 150 },
                        new Room { RoomNumber = "402", Type = "Deluxe", PricePerNight = 250 },
                        new Room { RoomNumber = "403", Type = "Suite", PricePerNight = 400 }
                    }
                },
                new Hotel
                {
                    Name = "Tokyo Tower Hotel",
                    Address = "1-2-3 Minato-ku",
                    CityId = cities[4].Id,
                    Rating = 5,
                    Rooms = new List<Room>
                    {
                        new Room { RoomNumber = "501", Type = "Standard", PricePerNight = 200 },
                        new Room { RoomNumber = "502", Type = "Deluxe", PricePerNight = 300 },
                        new Room { RoomNumber = "503", Type = "Suite", PricePerNight = 500 }
                    }
                },
                new Hotel
                {
                    Name = "Sydney Harbour View",
                    Address = "100 Circular Quay",
                    CityId = cities[5].Id,
                    Rating = 4,
                    Rooms = new List<Room>
                    {
                        new Room { RoomNumber = "601", Type = "Standard", PricePerNight = 180 },
                        new Room { RoomNumber = "602", Type = "Deluxe", PricePerNight = 280 }
                    }
                },
                new Hotel
                {
                    Name = "London Bridge Hotel",
                    Address = "1 Bridge Street",
                    CityId = cities[6].Id,
                    Rating = 4,
                    Rooms = new List<Room>
                    {
                        new Room { RoomNumber = "701", Type = "Standard", PricePerNight = 160 },
                        new Room { RoomNumber = "702", Type = "Deluxe", PricePerNight = 240 },
                        new Room { RoomNumber = "703", Type = "Suite", PricePerNight = 350 }
                    }
                },
                new Hotel
                {
                    Name = "Berlin Central",
                    Address = "100 Friedrichstraße",
                    CityId = cities[7].Id,
                    Rating = 4,
                    Rooms = new List<Room>
                    {
                        new Room { RoomNumber = "801", Type = "Standard", PricePerNight = 110 },
                        new Room { RoomNumber = "802", Type = "Deluxe", PricePerNight = 170 }
                    }
                },
                new Hotel
                {
                    Name = "Rome Imperial",
                    Address = "Via del Corso 1",
                    CityId = cities[8].Id,
                    Rating = 5,
                    Rooms = new List<Room>
                    {
                        new Room { RoomNumber = "901", Type = "Standard", PricePerNight = 130 },
                        new Room { RoomNumber = "902", Type = "Deluxe", PricePerNight = 200 },
                        new Room { RoomNumber = "903", Type = "Suite", PricePerNight = 300 }
                    }
                },
                new Hotel
                {
                    Name = "Dubai Skyline",
                    Address = "Sheikh Zayed Road",
                    CityId = cities[9].Id,
                    Rating = 5,
                    Rooms = new List<Room>
                    {
                        new Room { RoomNumber = "1001", Type = "Standard", PricePerNight = 250 },
                        new Room { RoomNumber = "1002", Type = "Deluxe", PricePerNight = 400 },
                        new Room { RoomNumber = "1003", Type = "Suite", PricePerNight = 600 }
                    }
                }
            };

            context.Hotels.AddRange(hotels);
            context.SaveChanges();
        }
    }
} 