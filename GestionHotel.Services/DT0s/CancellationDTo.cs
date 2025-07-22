using System;
using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Services.DTOs
{
    public class CancellationDto
    {
        [Required]
        public int ReservationId { get; set; }
        
        [Required]
        public string Raison { get; set; }
        
        public int? DemandeeParUserId { get; set; }
        
        public bool ForceRemboursement { get; set; } = false; // Pour les réceptionnistes
        
        public DateTime DateDemande { get; set; } = DateTime.UtcNow;
    }

    public class CancellationResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public bool RemboursementApplique { get; set; }
        public decimal MontantRembourse { get; set; }
        public decimal FraisAppliques { get; set; }
        public string RaisonRefusRemboursement { get; set; }
        public DateTime DateLimiteRemboursement { get; set; }
    }
}