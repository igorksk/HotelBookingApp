using HotelBookingApi.DTOs;
using HotelBookingApi.Models;
using HotelBookingApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController(IRoomRepository roomRepository, ILogger<RoomsController> logger) : ControllerBase
{
    private readonly IRoomRepository _roomRepository = roomRepository;
    private readonly ILogger<RoomsController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
    {
        _logger.LogInformation("Getting all rooms");
        return await _roomRepository.GetRooms().ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Room>> GetRoom(int id)
    {
        _logger.LogInformation("Getting room by id={RoomId}", id);
        var room = await _roomRepository.GetRoom(id);
        return room is null ? NotFound() : Ok(room);
    }

    [HttpPost]
    public async Task<ActionResult<Room>> PostRoom(CreateRoomDto dto)
    {
        _logger.LogInformation("Creating new room: {RoomNumber}", dto?.RoomNumber);
        if (dto == null || !await _roomRepository.HotelExistsAsync(dto.HotelId))
            return BadRequest("Hotel not found");

        var room = await _roomRepository.CreateRoomAsync(dto);
        return room == null
            ? BadRequest("Could not create room")
            : CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutRoom(int id, UpdateRoomDto dto)
    {
        _logger.LogInformation("Updating room id={RoomId}", id);
        if (id != dto.Id)
            return BadRequest("Id mismatch");

        var success = await _roomRepository.UpdateRoomAsync(id, dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        _logger.LogInformation("Deleting room id={RoomId}", id);
        var success = await _roomRepository.DeleteRoomAsync(id);
        return success ? NoContent() : NotFound();
    }
}