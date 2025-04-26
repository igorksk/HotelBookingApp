using HotelBookingApi.Data;
using HotelBookingApi.DTOs;
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
            return await GetRooms().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Room?> GetRoomByIdAsync(int roomId)
        {
            return await _context.Rooms.FindAsync(roomId);
        }

        public async Task<Room?> CreateRoomAsync(CreateRoomDto dto)
        {
            var hotel = await _context.Hotels.FindAsync(dto.HotelId);
            if (hotel == null)
                return null;

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

            return room;
        }

        public async Task<bool> UpdateRoomAsync(int id, UpdateRoomDto dto)
        {
            if (id != dto.Id)
                return false;

            var existingRoom = await _context.Rooms.FindAsync(id);
            if (existingRoom == null)
                return false;

            var hotel = await _context.Hotels.FindAsync(dto.HotelId);
            if (hotel == null)
                return false;

            existingRoom.RoomNumber = dto.RoomNumber;
            existingRoom.Type = dto.Type;
            existingRoom.PricePerNight = dto.PricePerNight;
            existingRoom.IsAvailable = dto.IsAvailable;
            existingRoom.HotelId = dto.HotelId;
            existingRoom.Hotel = hotel;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
                return false;

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HotelExistsAsync(int hotelId)
        {
            return await _context.Hotels.AnyAsync(h => h.Id == hotelId);
        }

        public async Task<bool> RoomExistsAsync(int id)
        {
            return await _context.Rooms.AnyAsync(r => r.Id == id);
        }
    }
}
