using HotelBookingApi.Data;
using HotelBookingApi.Models;
using HotelBookingApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController(HotelBookingContext context, ILogger<RoomsController> logger) : ControllerBase
{
    private readonly HotelBookingContext _context = context;
    private readonly ILogger<RoomsController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
    {
        _logger.LogInformation("Getting all rooms");
        return await _context.Rooms
            .Include(r => r.Hotel)
                .ThenInclude(h => h.City)
                    .ThenInclude(c => c.Country)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Room>> GetRoom(int id)
    {
        _logger.LogInformation("Getting room by id={RoomId}", id);
        var room = await _context.Rooms
            .Include(r => r.Hotel)
                .ThenInclude(h => h.City)
                    .ThenInclude(c => c.Country)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (room == null)
        {
            return NotFound();
        }

        return room;
    }

    [HttpPost]
    public async Task<ActionResult<Room>> PostRoom(CreateRoomDto dto)
    {
        _logger.LogInformation("Creating new room: {RoomNumber}", dto?.RoomNumber);
        var hotel = await _context.Hotels.FindAsync(dto!.HotelId);
        if (hotel == null)
        {
            return BadRequest("Hotel not found");
        }

        var room = new Room
        {
            RoomNumber = dto.RoomNumber,
            Type = dto.Type,
            PricePerNight = dto.PricePerNight,
            IsAvailable = dto.IsAvailable,
            HotelId = dto.HotelId,
            Hotel = hotel
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutRoom(int id, UpdateRoomDto dto)
    {
        _logger.LogInformation("Updating room id={RoomId}", id);
        if (id != dto.Id)
        {
            return BadRequest("Id mismatch");
        }

        var hotel = await _context.Hotels.FindAsync(dto.HotelId);
        if (hotel == null)
        {
            return BadRequest("Hotel not found");
        }

        var existingRoom = await _context.Rooms.FindAsync(id);
        if (existingRoom == null)
        {
            return NotFound();
        }

        existingRoom.RoomNumber = dto.RoomNumber;
        existingRoom.Type = dto.Type;
        existingRoom.PricePerNight = dto.PricePerNight;
        existingRoom.IsAvailable = dto.IsAvailable;
        existingRoom.HotelId = dto.HotelId;
        existingRoom.Hotel = hotel;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RoomExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        _logger.LogInformation("Deleting room id={RoomId}", id);
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
        {
            return NotFound();
        }

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool RoomExists(int id)
    {
        return _context.Rooms.Any(e => e.Id == id);
    }
}