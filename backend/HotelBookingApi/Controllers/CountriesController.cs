using HotelBookingApi.Models;
using HotelBookingApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController(ICountryRepository repository, ILogger<CountriesController> logger) : ControllerBase
{
    private readonly ICountryRepository _repository = repository;
    private readonly ILogger<CountriesController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
    {
        _logger.LogInformation("Getting all countries");
        try
        {
            var countries = await _repository.GetCountriesAsync();
            return Ok(countries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting countries");
            throw;
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Country>> GetCountry(int id)
    {
        _logger.LogInformation("Getting country by id={CountryId}", id);
        var country = await _repository.GetCountryAsync(id);
        return country == null ? NotFound() : country;
    }

    [HttpPost]
    public async Task<ActionResult<Country>> PostCountry(Country country)
    {
        _logger.LogInformation("Creating new country: {Name}", country?.Name);
        var created = await _repository.AddCountryAsync(country!);
        return CreatedAtAction(nameof(GetCountry), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCountry(int id, Country country)
    {
        _logger.LogInformation("Updating country id={CountryId}", id);
        if (id != country.Id) return BadRequest();

        var success = await _repository.UpdateCountryAsync(country);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        _logger.LogInformation("Deleting country id={CountryId}", id);
        var deleted = await _repository.DeleteCountryAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}