using HotelBookingApi.DTOs;
using HotelBookingApi.Models;
using HotelBookingApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController(IBookingRepository bookingRepository, IRoomRepository roomRepository, ILogger<BookingsController> logger) : ControllerBase
{
    private readonly IBookingRepository _bookingRepository = bookingRepository;
    private readonly IRoomRepository _roomRepository = roomRepository;
    private readonly ILogger<BookingsController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings()
    {
        _logger.LogInformation("Getting all bookings");
        var bookings = await _bookingRepository.GetBookings().ToListAsync();

        return bookings.ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDto>> GetBooking(int id)
    {
        _logger.LogInformation("Getting booking by id={BookingId}", id);
        var booking = await _bookingRepository.GetBooking(id);

        if (booking == null)
        {
            return NotFound();
        }

        return booking;
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> PostBooking(CreateBookingDto createBookingDto)
    {
        _logger.LogInformation("Creating new booking for guest: {GuestName}", createBookingDto?.GuestName);
        var room = await _roomRepository.GetRoom(createBookingDto!.RoomId);

        if (room == null)
        {
            return BadRequest("Room not found");
        }

        if (!room.IsAvailable)
        {
            return BadRequest("Room is not available");
        }

        // Check if the room is already booked for the given dates
        if (await _bookingRepository.IsRoomBookedAsync(createBookingDto.RoomId, createBookingDto.CheckInDate, createBookingDto.CheckOutDate))
        {
            return BadRequest("Room is already booked for the selected dates");
        }

        var booking = new Booking
        {
            GuestName = createBookingDto!.GuestName,
            GuestEmail = createBookingDto.GuestEmail,
            CheckInDate = createBookingDto.CheckInDate,
            CheckOutDate = createBookingDto.CheckOutDate,
            RoomId = createBookingDto.RoomId,
            Status = "Confirmed"
        };

        // Calculate total price
        var days = (booking.CheckOutDate - booking.CheckInDate).Days;
        booking.TotalPrice = room.PricePerNight * days;

        await _bookingRepository.CreateBookingAsync(booking, room);

        var bookingDto = new BookingDto
        {
            Id = booking.Id,
            GuestName = booking.GuestName,
            GuestEmail = booking.GuestEmail,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status,
            RoomNumber = room?.RoomNumber ?? string.Empty,
            RoomType = room?.Type ?? string.Empty,
            PricePerNight = room?.PricePerNight ?? 0,
            HotelName = room?.Hotel?.Name ?? string.Empty,
            HotelAddress = room?.Hotel?.Address ?? string.Empty,
            HotelCity = room?.Hotel?.City?.Name ?? string.Empty,
            HotelCountry = room?.Hotel?.City?.Country?.Name ?? string.Empty,
            CountryCode = room?.Hotel?.City?.Country?.Code ?? string.Empty
        };

        return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, bookingDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutBooking(int id, UpdateBookingDto updateBookingDto)
    {
        _logger.LogInformation("Updating booking id={BookingId}", id);

        var booking = await _bookingRepository.GetBookingWithRoomAsync(id);
        if (booking == null)
        {
            return NotFound();
        }

        // Check if the room is available for the new dates
        if (booking.CheckInDate != updateBookingDto.CheckInDate || booking.CheckOutDate != updateBookingDto.CheckOutDate)
        {
            if (await _bookingRepository.IsRoomBookedAsync(booking.RoomId, updateBookingDto.CheckInDate, updateBookingDto.CheckOutDate, id))
            {
                return BadRequest("Room is already booked for the selected dates");
            }

            var days = (updateBookingDto.CheckOutDate - updateBookingDto.CheckInDate).Days;
            booking.TotalPrice = booking.Room.PricePerNight * days;
        }

        booking.GuestName = updateBookingDto.GuestName;
        booking.GuestEmail = updateBookingDto.GuestEmail;
        booking.CheckInDate = updateBookingDto.CheckInDate;
        booking.CheckOutDate = updateBookingDto.CheckOutDate;
        booking.Status = updateBookingDto.Status;

        try
        {
            _bookingRepository.UpdateBooking(booking);
            await _bookingRepository.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _bookingRepository.BookingExistsAsync(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        _logger.LogInformation("Deleting booking id={BookingId}", id);

        var booking = await _bookingRepository.GetBookingByIdAsync(id);
        if (booking == null)
        {
            return NotFound();
        }

        var room = await _roomRepository.GetRoomByIdAsync(booking.RoomId);
        if (room != null)
        {
            room.IsAvailable = true;
        }

        _bookingRepository.RemoveBooking(booking);
        await _bookingRepository.SaveChangesAsync();

        return NoContent();
    }
}