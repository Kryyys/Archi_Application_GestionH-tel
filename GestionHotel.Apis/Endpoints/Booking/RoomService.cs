using GestionHotel.Data;
using GestionHotel.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestionHotel.Services
{
    public class RoomService
    {
        private readonly SupabaseClient _supabaseClient;

        public RoomService(SupabaseClient supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<List<Room>> GetAllRoomsAsync()
        {
            var json = await _supabaseClient.GetDataAsync("rooms"); // le nom de ta table Supabase
            return JsonSerializer.Deserialize<List<Room>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
