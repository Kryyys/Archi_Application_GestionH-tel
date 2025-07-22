using GestionHotel.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionHotel.Services.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationDto> CreateReservationAsync(CreateReservationDto createDto);
        Task<ReservationDto> GetReservationByIdAsync(int id);
        Task<IEnumerable<ReservationDto>> GetReservationsByClientIdAsync(int clientId);
        Task<ReservationDto> UpdateReservationAsync(UpdateReservationDto updateDto);
        Task<CancellationResultDto> CancelReservationAsync(CancellationDto cancellationDto);
        Task<CheckInOutResultDto> CheckInAsync(CheckInDto checkInDto);
        Task<CheckInOutResultDto> CheckOutAsync(CheckOutDto checkOutDto);
        Task<IEnumerable<ReservationDto>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ReservationDto>> GetTodayArrivalsAsync();
        Task<IEnumerable<ReservationDto>> GetTodayDeparturesAsync();
    }
}