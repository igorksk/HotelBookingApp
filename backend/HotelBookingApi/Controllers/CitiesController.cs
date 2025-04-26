using HotelBookingApi.Models;
using HotelBookingApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController(ICityRepository repository, ILogger<CitiesController> logger) : ControllerBase
{
    private readonly ICityRepository _repository = repository;
    private readonly ILogger<CitiesController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<City>>> GetCities([FromQuery] int? countryId)
    {
        _logger.LogInformation("Getting city list (filter by country: {CountryId})", countryId);
        try
        {
            var cities = await _repository.GetCitiesAsync(countryId);
            return Ok(cities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting cities");
            throw;
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<City>> GetCity(int id)
    {
        _logger.LogInformation("Getting city by id={CityId}", id);
        var city = await _repository.GetCityAsync(id);
        return city == null ? NotFound() : city;
    }

    [HttpPost]
    public async Task<ActionResult<City>> PostCity(City city)
    {
        _logger.LogInformation("Creating new city: {Name}", city?.Name);
        var created = await _repository.AddCityAsync(city!);
        return CreatedAtAction(nameof(GetCity), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCity(int id, City city)
    {
        _logger.LogInformation("Updating city id={CityId}", id);
        if (id != city.Id) return BadRequest();

        var success = await _repository.UpdateCityAsync(city);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCity(int id)
    {
        _logger.LogInformation("Deleting city id={CityId}", id);
        var deleted = await _repository.DeleteCityAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}