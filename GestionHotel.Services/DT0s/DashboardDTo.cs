using System;
using System.Collections.Generic;

namespace GestionHotel.Services.DTOs
{
    public class DashboardDto
    {
        public DateTime Date { get; set; } = DateTime.Today;
        
        // Statistiques générales
        public int TotalReservations { get; set; }
        public int ReservationsAujourdhui { get; set; }
        public int ArribeesAujourdhui { get; set; }
        public int DepartsAujourdhui { get; set; }
        public decimal ChiffresAffairesJour { get; set; }
        public decimal ChiffresAffairesMois { get; set; }
        
        // État des chambres
        public int ChambresDisponibles { get; set; }
        public int ChambresOccupees { get; set; }
        public int ChambresANettoyer { get; set; }
        public int ChambresHorsService { get; set; }
        
        // Paiements
        public int PaiementsEnAttente { get; set; }
        public decimal MontantPaiementsEnAttente { get; set; }
        
        // Notifications
        public int NotificationsEnvoyees { get; set; }
        public int NotificationsEnAttente { get; set; }
        
        // Listes détaillées
        public List<ReservationDto> ProchainArrivees { get; set; } = new List<ReservationDto>();
        public List<ReservationDto> ProchainDeparts { get; set; } = new List<ReservationDto>();
        public List<CleaningTaskDto> TachesNettoyageUrgentes { get; set; } = new List<CleaningTaskDto>();
    }

    public class ClientDashboardDto
    {
        public int ClientId { get; set; }
        public string ClientNom { get; set; }
        
        // Réservations du client
        public List<ReservationDto> ReservationsActuelles { get; set; } = new List<ReservationDto>();
        public List<ReservationDto> ReservationsPassees { get; set; } = new List<ReservationDto>();
        public List<ReservationDto> ReservationsFutures { get; set; } = new List<ReservationDto>();
        
        // Statistiques
        public int TotalReservations { get; set; }
        public decimal TotalDepense { get; set; }
        public DateTime? DernierSejour { get; set; }
        public DateTime? ProchainSejour { get; set; }
    }
}