using HotelBookingApi.Data;
using HotelBookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController(HotelBookingContext context) : ControllerBase
{
    private readonly HotelBookingContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
    {
        return await _context.Rooms
            .Include(r => r.Hotel)
                .ThenInclude(h => h.City)
                    .ThenInclude(c => c.Country)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Room>> GetRoom(int id)
    {
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
        var hotel = await _context.Hotels.FindAsync(dto.HotelId);
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