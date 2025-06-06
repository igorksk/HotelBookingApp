using HotelBookingApi.Models;

namespace HotelBookingApi.Repository.Interfaces
{
    public interface IRoomRepository
    {
        IQueryable<Room> GetRooms();
        Task<Room?> GetRoom(int id);
    }
}
