using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Apis.Endpoints.Cleaning
{
    public class CleaningInput
    {
        [Required]
        public int RoomId { get; set; }
        
        public bool Priority { get; set; } = false;
        
        public string Notes { get; set; }
        
        public string TypeNettoyage { get; set; } = "Standard";
        
        public int? AssignedToUserId { get; set; }
    }
}