namespace PruebaTecnica3.Server.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Dni { get; set; }
        public required string Email { get; set; }
        public string Contrasena { get; set; }
        public required DateTime FechaNacimiento { get; set; }
        public bool? Estado { get; set; }
        public string? Rol { get; set; }
    }
}
