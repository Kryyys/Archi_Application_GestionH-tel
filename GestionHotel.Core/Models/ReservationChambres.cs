namespace GestionHotel.Core.Models
{
    public class ReservationChambre
    {
        public int Id { get; set; }
        
        public int ReservationId { get; set; }
        
        public int ChambreaId { get; set; }
        
        // Navigation properties
        public Reservation Reservation { get; set; }
        public Room Chambre { get; set; }
    }
}