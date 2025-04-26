namespace HotelBookingApi.DTOs;

public class UpdateHotelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Rating { get; set; }
    public int CityId { get; set; }
}