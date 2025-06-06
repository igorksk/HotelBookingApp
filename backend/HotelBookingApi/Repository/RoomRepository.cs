using HotelBookingApi.Data;
using HotelBookingApi.Models;
using HotelBookingApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Repository
{
    public class RoomRepository(HotelBookingContext context) : IRoomRepository
    {
        private readonly HotelBookingContext _context = context;

        public IQueryable<Room> GetRooms()
        {
            return _context.Rooms
                .Include(r => r.Hotel)
                    .ThenInclude(h => h.City)
                        .ThenInclude(c => c.Country);
        }

        public async Task<Room?> GetRoom(int id)
        {
            var room = await GetRooms()
                .FirstOrDefaultAsync(r => r.Id == id);

            return room;
        }
    }
}
