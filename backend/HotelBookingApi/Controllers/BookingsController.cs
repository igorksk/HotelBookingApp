using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBookingApi.Data;
using HotelBookingApi.Models;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly HotelBookingContext _context;

    public BookingsController(HotelBookingContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings()
    {
        var bookings = await _context.Bookings
            .Include(b => b.Room)
                .ThenInclude(r => r.Hotel)
                    .ThenInclude(h => h.City)
                        .ThenInclude(c => c.Country)
            .OrderByDescending(b => b.CheckInDate)
            .Select(b => new BookingDto
            {
                Id = b.Id,
                GuestName = b.GuestName,
                GuestEmail = b.GuestEmail,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                RoomNumber = b.Room.RoomNumber,
                RoomType = b.Room.Type,
                PricePerNight = b.Room.PricePerNight,
                HotelName = b.Room.Hotel.Name,
                HotelAddress = b.Room.Hotel.Address,
                HotelCity = b.Room.Hotel.City.Name,
                HotelCountry = b.Room.Hotel.City.Country.Name,
                CountryCode = b.Room.Hotel.City.Country.Code
            })
            .ToListAsync();

        return bookings;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDto>> GetBooking(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.Room)
                .ThenInclude(r => r.Hotel)
                    .ThenInclude(h => h.City)
                        .ThenInclude(c => c.Country)
            .Select(b => new BookingDto
            {
                Id = b.Id,
                GuestName = b.GuestName,
                GuestEmail = b.GuestEmail,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                RoomNumber = b.Room.RoomNumber,
                RoomType = b.Room.Type,
                PricePerNight = b.Room.PricePerNight,
                HotelName = b.Room.Hotel.Name,
                HotelAddress = b.Room.Hotel.Address,
                HotelCity = b.Room.Hotel.City.Name,
                HotelCountry = b.Room.Hotel.City.Country.Name,
                CountryCode = b.Room.Hotel.City.Country.Code
            })
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
        {
            return NotFound();
        }

        return booking;
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> PostBooking(CreateBookingDto createBookingDto)
    {
        var room = await _context.Rooms
            .Include(r => r.Hotel)
                .ThenInclude(h => h.City)
                    .ThenInclude(c => c.Country)
            .FirstOrDefaultAsync(r => r.Id == createBookingDto.RoomId);

        if (room == null)
        {
            return BadRequest("Room not found");
        }

        if (!room.IsAvailable)
        {
            return BadRequest("Room is not available");
        }

        // Check if the room is already booked for the given dates
        var existingBooking = await _context.Bookings
            .AnyAsync(b => b.RoomId == createBookingDto.RoomId &&
                          ((createBookingDto.CheckInDate >= b.CheckInDate && createBookingDto.CheckInDate < b.CheckOutDate) ||
                           (createBookingDto.CheckOutDate > b.CheckInDate && createBookingDto.CheckOutDate <= b.CheckOutDate) ||
                           (createBookingDto.CheckInDate <= b.CheckInDate && createBookingDto.CheckOutDate >= b.CheckOutDate)));

        if (existingBooking)
        {
            return BadRequest("Room is already booked for the selected dates");
        }

        var booking = new Booking
        {
            GuestName = createBookingDto.GuestName,
            GuestEmail = createBookingDto.GuestEmail,
            CheckInDate = createBookingDto.CheckInDate,
            CheckOutDate = createBookingDto.CheckOutDate,
            RoomId = createBookingDto.RoomId,
            Status = "Confirmed"
        };

        // Calculate total price
        var days = (booking.CheckOutDate - booking.CheckInDate).Days;
        booking.TotalPrice = room.PricePerNight * days;

        _context.Bookings.Add(booking);
        room.IsAvailable = false;
        await _context.SaveChangesAsync();

        var bookingDto = new BookingDto
        {
            Id = booking.Id,
            GuestName = booking.GuestName,
            GuestEmail = booking.GuestEmail,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status,
            RoomNumber = room.RoomNumber,
            RoomType = room.Type,
            PricePerNight = room.PricePerNight,
            HotelName = room.Hotel.Name,
            HotelAddress = room.Hotel.Address,
            HotelCity = room.Hotel.City.Name,
            HotelCountry = room.Hotel.City.Country.Name,
            CountryCode = room.Hotel.City.Country.Code
        };

        return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, bookingDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutBooking(int id, UpdateBookingDto updateBookingDto)
    {
        var booking = await _context.Bookings
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
        {
            return NotFound();
        }

        // Check if the room is available for the new dates
        if (booking.CheckInDate != updateBookingDto.CheckInDate || booking.CheckOutDate != updateBookingDto.CheckOutDate)
        {
            var existingBooking = await _context.Bookings
                .AnyAsync(b => b.RoomId == booking.RoomId && b.Id != id &&
                              ((updateBookingDto.CheckInDate >= b.CheckInDate && updateBookingDto.CheckInDate < b.CheckOutDate) ||
                               (updateBookingDto.CheckOutDate > b.CheckInDate && updateBookingDto.CheckOutDate <= b.CheckOutDate) ||
                               (updateBookingDto.CheckInDate <= b.CheckInDate && updateBookingDto.CheckOutDate >= b.CheckOutDate)));

            if (existingBooking)
            {
                return BadRequest("Room is already booked for the selected dates");
            }

            // Recalculate total price if dates changed
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
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookingExists(id))
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
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
        {
            return NotFound();
        }

        var room = await _context.Rooms.FindAsync(booking.RoomId);
        if (room != null)
        {
            room.IsAvailable = true;
        }

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool BookingExists(int id)
    {
        return _context.Bookings.Any(e => e.Id == id);
    }
} 