using System;
using GestionHotel.Core.Enums;

namespace GestionHotel.Core.Models
{
    public class Notification
    {
        public int Id { get; set; }
        
        public int ClientId { get; set; }
        
        public string Type { get; set; } = NotificationType.Email.ToString();
        
        public string Message { get; set; }
        
        public DateTime DateEnvoi { get; set; } = DateTime.UtcNow;
        
        public bool Envoye { get; set; } = false;
        
        public string Destinataire { get; set; } // Email ou numéro de téléphone
        
        public string Sujet { get; set; }
        
        // Navigation property
        public Client Client { get; set; }
    }
}