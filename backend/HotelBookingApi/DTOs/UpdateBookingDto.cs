namespace HotelBookingApi.DTOs;

using System.ComponentModel.DataAnnotations;

public class UpdateBookingDto
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

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Confirmed";
}
