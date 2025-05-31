namespace HotelBookingApi.DTOs;

public class HotelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Rating { get; set; }
    public int CityId { get; set; }

    // City information
    public string CityName { get; set; } = string.Empty;

    // Country information
    public string CountryName { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;

    // Rooms information
    public List<RoomDto> Rooms { get; set; } = [];
}

public class RoomDto
{
    public int Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public bool IsAvailable { get; set; }
}