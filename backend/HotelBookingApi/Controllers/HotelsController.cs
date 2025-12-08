using HotelBookingApi.DTOs;
using HotelBookingApi.Models;
using HotelBookingApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController(IHotelRepository repository, ILogger<HotelsController> _logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels(string? country, string? city, DateTime? checkIn, DateTime? checkOut)
    {
        _logger.LogInformation("Getting hotel list (filters: country={Country}, city={City})", country, city);

        var hotels = await repository.GetHotelsAsync(country, city, checkIn, checkOut);
        return Ok(hotels);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HotelDto>> GetHotel(int id)
    {
        _logger.LogInformation("Getting hotel by id={HotelId}", id);

        var hotel = await repository.GetHotelAsync(id);
        return hotel is null ? NotFound() : Ok(hotel);
    }

    [HttpPost]
    public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await repository.CityExistsAsync(dto.CityId))
            return BadRequest("Specified city does not exist");

        _logger.LogInformation("Creating new hotel: {Name}", dto?.Name);

        var hotel = await repository.CreateHotelAsync(dto!);
        return hotel == null ? BadRequest("Could not create hotel") : CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutHotel(int id, UpdateHotelDto dto)
    {
        if (!ModelState.IsValid || id != dto.Id)
            return BadRequest(ModelState);

        if (!await repository.CityExistsAsync(dto.CityId))
            return BadRequest("Specified city does not exist");

        _logger.LogInformation("Updating hotel id={HotelId}", id);

        var success = await repository.UpdateHotelAsync(id, dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        _logger.LogInformation("Deleting hotel id={HotelId}", id);

        var success = await repository.DeleteHotelAsync(id);
        return success ? NoContent() : NotFound();
    }
}