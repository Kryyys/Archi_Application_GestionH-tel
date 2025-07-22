using GestionHotel.Services.Interfaces;
using GestionHotel.Services.DTOs;
using System;
using System.Threading.Tasks;

namespace GestionHotel.Apis.Endpoints.Reception
{
    public class ReceptionHandler
    {
        private readonly IReservationService _reservationService;
        private readonly IRoomService _roomService;

        public ReceptionHandler(IReservationService reservationService, IRoomService roomService)
        {
            _reservationService = reservationService;
            _roomService = roomService;
        }

        public async Task<ReceptionResult> HandleGetDashboardAsync()
        {
            var arrivals = await _reservationService.GetTodayArrivalsAsync();
            var departures = await _reservationService.GetTodayDeparturesAsync();

            return new ReceptionResult
            {
                Success = true,
                Dashboard = new
                {
                    Date = DateTime.Today,
                    ArrivalsToday = arrivals,
                    DeparturesToday = departures
                }
            };
        }

        public async Task<ReceptionResult> HandleCheckInAsync(CheckInOutInput input)
        {
            var checkInDto = new CheckInDto
            {
                ReservationId = input.ReservationId,
                DateArrivee = input.DateArrivee ?? DateTime.Now,
                NotesReception = input.Notes,
                PaiementRestantEffectue = input.PaiementEffectue,
                MontantPaiementRestant = input.MontantPaiement
            };

            var result = await _reservationService.CheckInAsync(checkInDto);
            
            return new ReceptionResult
            {
                Success = result.Success,
                Message = result.Message,
                CheckInOutResult = result
            };
        }

        public async Task<ReceptionResult> HandleCheckOutAsync(CheckInOutInput input)
        {
            var checkOutDto = new CheckOutDto
            {
                ReservationId = input.ReservationId,
                DateDepart = input.DateDepart ?? DateTime.Now,
                NotesReception = input.Notes,
                PaiementComplementaireEffectue = input.PaiementEffectue,
                MontantPaiementComplementaire = input.MontantPaiement,
                DegatsSignales = input.DegatsSignales,
                DescriptionDegats = input.DescriptionDegats,
                MontantDegats = input.MontantDegats,
                NoteSatisfaction = input.NoteSatisfaction,
                CommentaireSatisfaction = input.CommentaireSatisfaction
            };

            var result = await _reservationService.CheckOutAsync(checkOutDto);
            
            return new ReceptionResult
            {
                Success = result.Success,
                Message = result.Message,
                CheckInOutResult = result
            };
        }

        public async Task<ReceptionResult> HandleGetTodayReservationsAsync()
        {
            var arrivals = await _reservationService.GetTodayArrivalsAsync();
            var departures = await _reservationService.GetTodayDeparturesAsync();

            return new ReceptionResult
            {
                Success = true,
                TodayReservations = new { Arrivals = arrivals, Departures = departures }
            };
        }
    }

    public class ReceptionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Dashboard { get; set; }
        public object TodayReservations { get; set; }
        public CheckInOutResultDto CheckInOutResult { get; set; }
    }
}