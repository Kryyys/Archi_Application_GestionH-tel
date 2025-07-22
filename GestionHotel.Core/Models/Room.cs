using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Core.Models
{
    public class Room
    {
        public int Id { get; set; }
        
        [Required]
        public string Numero { get; set; }
        
        public int TypeId { get; set; }
        
        [Range(1, 10)]
        public int Capacite { get; set; }
        
        public string Etat { get; set; } = "Disponible"; // Neuf, Refaite, A refaire, Rien a signaler, Gros dégats
        
        // Navigation property
        public RoomType Type { get; set; }
    }
}