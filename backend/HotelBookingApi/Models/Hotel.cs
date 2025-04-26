namespace HotelBookingApi.Models;

public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int CityId { get; set; }
    public City? City { get; set; }
    public int Rating { get; set; }
    public List<Room> Rooms { get; set; } = new();
} 