using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Core.Models
{
    public class RoomType
    {
        public int Id { get; set; }
        
        [Required]
        public string Nom { get; set; } // Simple, Double, Suite, etc.
        
        [Range(0.01, double.MaxValue)]
        public decimal Tarif { get; set; }
    }
}