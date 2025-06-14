namespace HotelBookingApi.DTOs;

public class CreateHotelDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Rating { get; set; }
    public int CityId { get; set; }
}