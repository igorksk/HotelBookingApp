using HotelBookingApi.DTOs;
using HotelBookingApi.Models;

namespace HotelBookingApi.Repository.Interfaces
{
    public interface IRoomRepository
    {
        IQueryable<Room> GetRooms();
        Task<Room?> GetRoom(int id);
        Task<Room?> GetRoomByIdAsync(int roomId);
        Task<Room?> CreateRoomAsync(CreateRoomDto dto);
        Task<bool> UpdateRoomAsync(int id, UpdateRoomDto dto);
        Task<bool> DeleteRoomAsync(int id);
        Task<bool> HotelExistsAsync(int hotelId);
        Task<bool> RoomExistsAsync(int id);
    }
}
