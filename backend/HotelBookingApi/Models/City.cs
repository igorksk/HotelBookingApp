namespace HotelBookingApi.Models;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public Country? Country { get; set; }
    public List<Hotel> Hotels { get; set; } = new();
} 