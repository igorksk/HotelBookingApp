namespace HotelBookingApi.DTOs;

public class UpdateRoomDto
{
    public int Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public bool IsAvailable { get; set; }
    public int HotelId { get; set; }
}