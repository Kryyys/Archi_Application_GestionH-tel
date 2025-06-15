namespace GestionHotel.Models;

public class Reservation
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public string Statut { get; set; }
}