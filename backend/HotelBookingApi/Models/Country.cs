namespace HotelBookingApi.Models;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public List<City> Cities { get; set; } = [];
}