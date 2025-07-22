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
    public class CleaningService : ICleaningService
    {
        private readonly ICleaningRepository _cleaningRepository;
        private readonly IUserRepository _userRepository;

        public CleaningService(ICleaningRepository cleaningRepository, IUserRepository userRepository)
        {
            _cleaningRepository = cleaningRepository;
            _userRepository = userRepository;
        }

        public async Task<CleaningTaskDto> CreateCleaningTaskAsync(CreateCleaningTaskDto createDto)
        {
            var task = new CleaningTask
            {
                RoomId = createDto.RoomId,
                Status = CleaningStatus.EnAttente.ToString(),
                DateCreation = DateTime.UtcNow,
                AssignedToUserId = createDto.AssignedToUserId,
                Priority = createDto.Priority,
                Notes = createDto.Notes,
                TypeNettoyage = createDto.TypeNettoyage,
                DureeEstimeeMinutes = createDto.DureeEstimeeMinutes ?? GetEstimatedDuration(createDto.TypeNettoyage)
            };

            var createdTask = await _cleaningRepository.CreateAsync(task);
            return await ConvertToDto(createdTask);
        }

        public async Task<CleaningTaskDto> UpdateCleaningTaskAsync(UpdateCleaningTaskDto updateDto)
        {
            var task = await _cleaningRepository.GetByIdAsync(updateDto.Id);
            if (task == null)
            {
                throw new ArgumentException("Tâche de nettoyage introuvable");
            }

            if (!string.IsNullOrEmpty(updateDto.Status))
                task.Status = updateDto.Status;

            if (updateDto.AssignedToUserId.HasValue)
                task.AssignedToUserId = updateDto.AssignedToUserId.Value;

            if (!string.IsNullOrEmpty(updateDto.Notes))
                task.Notes = updateDto.Notes;

            if (updateDto.Priority.HasValue)
                task.Priority = updateDto.Priority.Value;

            if (updateDto.DateCompletion.HasValue)
                task.DateCompletion = updateDto.DateCompletion.Value;

            var updatedTask = await _cleaningRepository.UpdateAsync(task);
            return await ConvertToDto(updatedTask);
        }

        public async Task<IEnumerable<CleaningTaskDto>> GetPendingTasksAsync()
        {
            var tasks = await _cleaningRepository.GetTasksByStatusAsync(CleaningStatus.EnAttente.ToString());
            var dtos = new List<CleaningTaskDto>();

            foreach (var task in tasks.OrderByDescending(t => t.Priority).ThenBy(t => t.DateCreation))
            {
                dtos.Add(await ConvertToDto(task));
            }

            return dtos;
        }

        public async Task<IEnumerable<CleaningTaskDto>> GetTasksByUserIdAsync(int userId)
        {
            var allTasks = await _cleaningRepository.GetAllAsync();
            var userTasks = allTasks.Where(t => t.AssignedToUserId == userId);
            var dtos = new List<CleaningTaskDto>();

            foreach (var task in userTasks)
            {
                dtos.Add(await ConvertToDto(task));
            }

            return dtos;
        }

        public async Task<CleaningTaskDto> CompleteTaskAsync(int taskId, int userId, string notes = null)
        {
            var task = await _cleaningRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                throw new ArgumentException("Tâche de nettoyage introuvable");
            }

            if (task.AssignedToUserId != userId)
            {
                throw new UnauthorizedAccessException("Vous n'êtes pas assigné à cette tâche");
            }

            task.Status = CleaningStatus.Terminee.ToString();
            task.DateCompletion = DateTime.UtcNow;
            
            if (!string.IsNullOrEmpty(notes))
            {
                task.Notes = string.IsNullOrEmpty(task.Notes) ? notes : $"{task.Notes} | Complété: {notes}";
            }

            var updatedTask = await _cleaningRepository.UpdateAsync(task);
            return await ConvertToDto(updatedTask);
        }

        public async Task<CleaningTaskDto> CreateTaskForCheckoutAsync(int roomId, string notes = null)
        {
            var createDto = new CreateCleaningTaskDto
            {
                RoomId = roomId,
                Priority = true, // Les nettoyages post-checkout sont prioritaires
                Notes = notes ?? "Nettoyage suite au départ client",
                TypeNettoyage = BusinessRules.TypesNettoyage.Standard,
                DureeEstimeeMinutes = BusinessRules.MinutesNettoyageStandard
            };

            return await CreateCleaningTaskAsync(createDto);
        }

        private int GetEstimatedDuration(string typeNettoyage)
        {
            return typeNettoyage switch
            {
                BusinessRules.TypesNettoyage.Standard => BusinessRules.MinutesNettoyageStandard,
                BusinessRules.TypesNettoyage.Profond => BusinessRules.MinutesNettoyageProfond,
                BusinessRules.TypesNettoyage.Maintenance => 120,
                BusinessRules.TypesNettoyage.Urgent => 45,
                _ => BusinessRules.MinutesNettoyageStandard
            };
        }

        private async Task<CleaningTaskDto> ConvertToDto(CleaningTask task)
        {
            var dto = new CleaningTaskDto
            {
                Id = task.Id,
                RoomId = task.RoomId,
                Status = task.Status,
                DateCreation = task.DateCreation,
                DateCompletion = task.DateCompletion,
                AssignedToUserId = task.AssignedToUserId,
                Priority = task.Priority,
                Notes = task.Notes,
                TypeNettoyage = task.TypeNettoyage,
                DureeEstimeeMinutes = task.DureeEstimeeMinutes
            };

            // Charger les informations de la chambre (nécessiterait un RoomRepository)
            dto.RoomNumero = $"Room {task.RoomId}"; // Placeholder
            
            // Charger les informations de l'utilisateur assigné
            if (task.AssignedToUserId.HasValue)
            {
                var user = await _userRepository.GetByIdAsync(task.AssignedToUserId.Value);
                if (user != null)
                {
                    dto.AssignedToUserName = user.NomUtilisateur;
                    dto.AssignedToUserEmail = user.Email;
                }
            }

            return dto;
        }
    }
}