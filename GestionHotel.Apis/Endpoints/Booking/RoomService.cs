using GestionHotel.Models;

public class RoomService : IRoomService
{
    private readonly List<Room> _rooms = new()
    {
        new Room { Id = 1, Type = "Simple", Capacity = 1, PricePerNight = 80, Status = RoomStatus.Neu f },
        new Room { Id = 2, Type = "Double", Capacity = 2, PricePerNight = 120, Status = RoomStatus.RienASignaler }
    };

    public Task<List<Room>> GetAvailableRooms(DateTime startDate, DateTime endDate)
    {
        // Simuler la logique métier pour l'instant
        return Task.FromResult(_rooms);
    }
}
