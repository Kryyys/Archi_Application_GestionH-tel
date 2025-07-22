using System;
using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Core.Models
{
    public class Refund
    {
        public int Id { get; set; }
        
        public int PaymentId { get; set; }
        
        public int ReservationId { get; set; }
        
        [Range(0.01, double.MaxValue)]
        public decimal Montant { get; set; }
        
        public string Raison { get; set; }
        
        public DateTime DateRemboursement { get; set; } = DateTime.UtcNow;
        
        public int? ApprouveParUserId { get; set; }
        
        // Navigation properties
        public Payment Payment { get; set; }
        public Reservation Reservation { get; set; }
        public User ApprouveParUser { get; set; }
    }
}