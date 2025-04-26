namespace HotelBookingApi.DTOs;

public class CreateBookingDto
{
    public string GuestName { get; set; } = string.Empty;
    public string GuestEmail { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int RoomId { get; set; }
}