using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Apis.Endpoints.Booking
{
    public class PaymentInput
    {
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Montant { get; set; }
        
        [Required]
        public string MethodePaiement { get; set; }
        
        [Required]
        public string NumeroCarte { get; set; }
        
        [Required]
        public string NomPorteur { get; set; }
        
        [Required]
        public string DateExpiration { get; set; }
        
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string CodeCVV { get; set; }
    }
}