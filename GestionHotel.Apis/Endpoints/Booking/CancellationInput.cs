using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Apis.Endpoints.Booking
{
    public class CancellationInput
    {
        [Required]
        public string Raison { get; set; }
        
        public bool ForceRemboursement { get; set; } = false;
    }
}