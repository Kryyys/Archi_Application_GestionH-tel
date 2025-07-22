using System;
using System.ComponentModel.DataAnnotations;
using GestionHotel.Core.Enums;

namespace GestionHotel.Core.Models
{
    public class Payment
    {
        public int Id { get; set; }
        
        public int ReservationId { get; set; }
        
        [Range(0.01, double.MaxValue)]
        public decimal Montant { get; set; }
        
        public string Statut { get; set; } = PaymentStatus.EnAttente.ToString();
        
        public string TransactionId { get; set; }
        
        public string MethodePaiement { get; set; }
        
        public DateTime DatePaiement { get; set; } = DateTime.UtcNow;
        
        public string DetailsCarte { get; set; } // 4 derniers chiffres uniquement
        
        // Navigation property
        public Reservation Reservation { get; set; }
    }
}