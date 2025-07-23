using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Apis.Endpoints.Booking
{
    public class BookingInput
    {
        [Required]
        public int ClientId { get; set; }
        
        [Required]
        public DateTime DateDebut { get; set; }
        
        [Required]
        public DateTime DateFin { get; set; }
        
        public List<int> ChambresIds { get; set; } = new();
        
        public string? Commentaires { get; set; }
    }
}