using System;
using GestionHotel.Core.Enums;

namespace GestionHotel.Core.Models
{
    public class CleaningTask
    {
        public int Id { get; set; }
        
        public int RoomId { get; set; }
        
        public string Status { get; set; } = CleaningStatus.EnAttente.ToString();
        
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        
        public DateTime? DateCompletion { get; set; }
        
        public int? AssignedToUserId { get; set; }
        
        public bool Priority { get; set; } = false;
        
        public string Notes { get; set; }
        
        public string TypeNettoyage { get; set; } = "Standard"; // Standard, Profond, Maintenance
        
        public int? DureeEstimeeMinutes { get; set; } = 30;
        
        // Navigation properties
        public Room Room { get; set; }
        public User AssignedToUser { get; set; }
    }
}