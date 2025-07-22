using System;
using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Services.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        
        public int ReservationId { get; set; }
        
        [Range(0.01, double.MaxValue)]
        public decimal Montant { get; set; }
        
        public string Statut { get; set; }
        
        public string TransactionId { get; set; }
        
        public string MethodePaiement { get; set; }
        
        public DateTime DatePaiement { get; set; }
        
        public string DetailsCarte { get; set; }
        
        // Informations de la réservation
        public string ReservationReference { get; set; }
        public string ClientNom { get; set; }
        public string ClientEmail { get; set; }
    }

    public class CreatePaymentDto
    {
        [Required]
        public int ReservationId { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Montant { get; set; }
        
        [Required]
        public string MethodePaiement { get; set; }
        
        // Informations de carte bancaire (simulées)
        public string NumeroCarte { get; set; }
        public string NomPorteur { get; set; }
        public string DateExpiration { get; set; }
        public string CodeCVV { get; set; }
    }

    public class PaymentResultDto
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }
        public PaymentDto Payment { get; set; }
    }
}