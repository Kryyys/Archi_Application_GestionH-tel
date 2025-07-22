using System;
using System.ComponentModel.DataAnnotations;
using GestionHotel.Core.Enums;

namespace GestionHotel.Core.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        
        public int ClientId { get; set; }
        
        [Required]
        public DateTime DateDebut { get; set; }
        
        [Required]
        public DateTime DateFin { get; set; }
        
        public string Statut { get; set; } = ReservationStatus.EnAttente.ToString();
        
        public bool PaiementEffectue { get; set; } = false;
        
        public bool RemboursementEffectue { get; set; } = false;
        
        public decimal MontantTotal { get; set; }
        
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        
        public DateTime? DateModification { get; set; }
        
        public string Commentaires { get; set; }
        
        // Navigation properties
        public Client Client { get; set; }
    }
}