using System.ComponentModel.DataAnnotations;

namespace GestionHotel.Apis.Endpoints.Auth
{
    public class LoginInput
    {
        [Required]
        public string NomUtilisateur { get; set; }
        
        [Required]
        public string MotDePasse { get; set; }
    }
}