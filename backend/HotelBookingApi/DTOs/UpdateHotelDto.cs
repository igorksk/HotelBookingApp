namespace HotelBookingApi.DTOs;

using System.ComponentModel.DataAnnotations;

public class UpdateHotelDto
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(200, MinimumLength = 5)]
    public string Address { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; set; }

    [Range(1, int.MaxValue)]
    public int CityId { get; set; }
}
