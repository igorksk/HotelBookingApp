namespace HotelBookingApi.Models;

public class Room
{
    public int Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public bool IsAvailable { get; set; } = true;
    public int HotelId { get; set; }
    public Hotel? Hotel { get; set; }
    public List<Booking> Bookings { get; set; } = new();
} 