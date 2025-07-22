using GestionHotel.Core.Exceptions;
using GestionHotel.Core.Models;
using GestionHotel.Data.Repositories;
using GestionHotel.Services.DTOs;
using GestionHotel.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace GestionHotel.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createDto)
        {
            // Vérifier que l'email n'existe pas déjà
            if (await _userRepository.EmailExistsAsync(createDto.Email))
            {
                throw new BusinessException("Un utilisateur avec cet email existe déjà");
            }

            // Vérifier que le nom d'utilisateur n'existe pas déjà
            if (await _userRepository.UsernameExistsAsync(createDto.NomUtilisateur))
            {
                throw new BusinessException("Ce nom d'utilisateur est déjà pris");
            }

            var user = new User
            {
                NomUtilisateur = createDto.NomUtilisateur,
                MotDePasseHache = BCrypt.Net.BCrypt.HashPassword(createDto.MotDePasse),
                Role = createDto.Role,
                Email = createDto.Email,
                Prenom = createDto.Prenom,
                Nom = createDto.Nom,
                Actif = true
            };

            var createdUser = await _userRepository.CreateAsync(user);
            return ConvertToDto(createdUser);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new BusinessException("Utilisateur introuvable");
            }

            return ConvertToDto(user);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new BusinessException("Utilisateur introuvable");
            }

            return ConvertToDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role)
        {
            var allUsers = await _userRepository.GetAllAsync();
            var usersByRole = allUsers.Where(u => u.Role == role && u.Actif);
            
            return usersByRole.Select(ConvertToDto).ToList();
        }

        public async Task<UserDto> UpdateUserAsync(int id, CreateUserDto updateDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new BusinessException("Utilisateur introuvable");
            }

            // Vérifier les doublons si les données ont changé
            if (updateDto.Email != user.Email && await _userRepository.EmailExistsAsync(updateDto.Email))
            {
                throw new BusinessException("Un utilisateur avec cet email existe déjà");
            }

            if (updateDto.NomUtilisateur != user.NomUtilisateur && await _userRepository.UsernameExistsAsync(updateDto.NomUtilisateur))
            {
                throw new BusinessException("Ce nom d'utilisateur est déjà pris");
            }

            user.NomUtilisateur = updateDto.NomUtilisateur;
            user.Role = updateDto.Role;
            user.Email = updateDto.Email;
            user.Prenom = updateDto.Prenom;
            user.Nom = updateDto.Nom;

            // Mettre à jour le mot de passe seulement s'il est fourni
            if (!string.IsNullOrEmpty(updateDto.MotDePasse))
            {
                user.MotDePasseHache = BCrypt.Net.BCrypt.HashPassword(updateDto.MotDePasse);
            }

            var updatedUser = await _userRepository.UpdateAsync(user);
            return ConvertToDto(updatedUser);
        }

        public async Task<bool> DeactivateUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            user.Actif = false;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        private UserDto ConvertToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                NomUtilisateur = user.NomUtilisateur,
                Role = user.Role,
                Email = user.Email,
                Prenom = user.Prenom,
                Nom = user.Nom,
                DateCreation = user.DateCreation,
                DerniereConnexion = user.DerniereConnexion,
                Actif = user.Actif
            };
        }
    }
}