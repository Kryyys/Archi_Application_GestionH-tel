using System;

namespace GestionHotel.Apis.Endpoints.Booking
{
    public class GetAvailableRoomsInput
    {
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public int? NombrePersonnes { get; set; }
        public string? TypeChambre { get; set; }
        public decimal? BudgetMax { get; set; }
    }
}