using GestionHotel.Services.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionHotel.Services.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationDto> SendNotificationAsync(CreateNotificationDto notificationDto);
        Task SendBulkNotificationAsync(BulkNotificationDto bulkDto);
        Task SendPreStayNotificationAsync(int reservationId);
        Task SendPostStayNotificationAsync(int reservationId);
        Task<IEnumerable<NotificationDto>> GetNotificationsByClientIdAsync(int clientId);
        Task ProcessScheduledNotificationsAsync();
    }
}
