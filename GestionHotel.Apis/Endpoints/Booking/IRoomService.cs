using GestionHotel.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRoomService
{
    Task<List<Room>> GetAvailableRooms(DateTime startDate, DateTime endDate);
}