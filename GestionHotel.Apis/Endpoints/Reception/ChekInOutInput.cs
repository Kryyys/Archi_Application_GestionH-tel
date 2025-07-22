using System;
using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Apis.Endpoints.Reception
{
    public class CheckInOutInput
    {
        [Required]
        public int ReservationId { get; set; }
        
        public DateTime? DateArrivee { get; set; }
        public DateTime? DateDepart { get; set; }
        
        public string Notes { get; set; }
        
        public bool PaiementEffectue { get; set; } = false;
        public decimal? MontantPaiement { get; set; }
        
        // Pour checkout
        public bool DegatsSignales { get; set; } = false;
        public string DescriptionDegats { get; set; }
        public decimal? MontantDegats { get; set; }
        
        [Range(1, 5)]
        public int? NoteSatisfaction { get; set; }
        public string CommentaireSatisfaction { get; set; }
    }
}