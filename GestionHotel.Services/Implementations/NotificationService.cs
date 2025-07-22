using GestionHotel.Core.Constants;
using GestionHotel.Core.Enums;
using GestionHotel.Core.Models;
using GestionHotel.Data.Repositories;
using GestionHotel.Services.DTOs;
using GestionHotel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionHotel.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IReservationRepository _reservationRepository;
        // Note: Un NotificationRepository serait nécessaire pour persister les notifications

        public NotificationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<NotificationDto> SendNotificationAsync(CreateNotificationDto notificationDto)
        {
            // Simuler l'envoi de notification
            var success = await SimulateSendNotificationAsync(notificationDto);

            var notification = new NotificationDto
            {
                ClientId = notificationDto.ClientId,
                Type = notificationDto.Type,
                Message = notificationDto.Message,
                DateEnvoi = DateTime.UtcNow,
                Envoye = success,
                Destinataire = notificationDto.Destinataire,
                Sujet = notificationDto.Sujet
            };

            // Ici, on sauvegarderait en base avec un NotificationRepository
            // notification = await _notificationRepository.CreateAsync(notification);

            return notification;
        }

        public async Task SendBulkNotificationAsync(BulkNotificationDto bulkDto)
        {
            foreach (var clientId in bulkDto.ClientIds)
            {
                var individualNotification = new CreateNotificationDto
                {
                    ClientId = clientId,
                    Type = bulkDto.Type,
                    Message = bulkDto.MessageTemplate,
                    Sujet = bulkDto.Sujet,
                    Destinataire = "client@email.com", // À récupérer depuis le client
                    DateEnvoiProgrammee = bulkDto.DateEnvoiProgrammee
                };

                await SendNotificationAsync(individualNotification);
            }
        }

        public async Task SendPreStayNotificationAsync(int reservationId)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null) return;

            var message = BusinessRules.MessageTemplates.RappelArrivee
                .Replace("{clientName}", "Client") // À récupérer
                .Replace("{dateDebut}", reservation.DateDebut.ToString("dd/MM/yyyy"))
                .Replace("{roomNumber}", "XXX"); // À récupérer

            var notification = new CreateNotificationDto
            {
                ClientId = reservation.ClientId,
                Type = NotificationType.Email.ToString(),
                Message = message,
                Sujet = "Rappel de votre séjour - Demain",
                Destinataire = "client@email.com" // À récupérer
            };

            await SendNotificationAsync(notification);
        }

        public async Task SendPostStayNotificationAsync(int reservationId)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null) return;

            var message = BusinessRules.MessageTemplates.DemandeAvis
                .Replace("{clientName}", "Client"); // À récupérer

            var notification = new CreateNotificationDto
            {
                ClientId = reservation.ClientId,
                Type = NotificationType.Email.ToString(),
                Message = message,
                Sujet = "Votre avis nous intéresse",
                Destinataire = "client@email.com" // À récupérer
            };

            // Programmer l'envoi 6h après le checkout
            notification.DateEnvoiProgrammee = DateTime.UtcNow.AddHours(BusinessRules.HeuresNotificationPostSejour);
            
            await SendNotificationAsync(notification);
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsByClientIdAsync(int clientId)
        {
            // À implémenter avec un NotificationRepository
            return new List<NotificationDto>();
        }

        public async Task ProcessScheduledNotificationsAsync()
        {
            // Traiter les notifications programmées
            // Cette méthode serait appelée par un job en arrière-plan
            
            // 1. Récupérer les réservations avec des arrivées demain
            var tomorrow = DateTime.Today.AddDays(1);
            var reservations = await _reservationRepository.GetReservationsByDateRangeAsync(tomorrow, tomorrow);
            
            foreach (var reservation in reservations.Where(r => r.Statut == ReservationStatus.Confirmee.ToString()))
            {
                await SendPreStayNotificationAsync(reservation.Id);
            }
        }

        private async Task<bool> SimulateSendNotificationAsync(CreateNotificationDto notification)
        {
            // Simulation de l'envoi
            await Task.Delay(100);

            // Simulation d'un échec occasionnel (5% de chance)
            var random = new Random();
            return random.Next(100) >= 5;
        }
    }
}
