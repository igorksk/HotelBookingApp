namespace HotelBookingApi.DTOs;

using System.ComponentModel.DataAnnotations;

public class UpdateRoomDto
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 1)]
    public string RoomNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Type { get; set; } = string.Empty;

    [Range(0.0, double.MaxValue)]
    public decimal PricePerNight { get; set; }

    public bool IsAvailable { get; set; }

    [Range(1, int.MaxValue)]
    public int HotelId { get; set; }
}
