namespace HotelBookingApi.DTOs;

using System.ComponentModel.DataAnnotations;

public class CreateBookingDto
{
    [Required]
    [StringLength(100)]
    public string GuestName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string GuestEmail { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime CheckInDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime CheckOutDate { get; set; }

    [Range(1, int.MaxValue)]
    public int RoomId { get; set; }
}
