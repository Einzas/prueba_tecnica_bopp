using System.Security.Claims;

namespace PruebaTecnica3.Server.Models
{
    public class JWT
    {
        public required string Key { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string Subject { get; set; }

        public static dynamic ValidarToken(ClaimsIdentity identity)
        {
            try
            {
                if (identity.Claims.Count() == 0)
                {
                    return new
                    {
                        status = 400,
                        mensaje = "Token Invalido",
                        identity
                    };
                }

                var id = identity.Claims.FirstOrDefault(x => x.Type == "Id").Value;

                if (id != null)
                {
                    return new
                    {
                        status = 200,
                        id = id,
                        rol = identity.Claims.FirstOrDefault(x => x.Type == "Rol").Value
                    };
                }

                else
                {
                    return new
                    {
                        status = 400,
                        mensaje = "Error: no se encontro id",
                        data = ""
                    };
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    status = 400,
                    mensaje = "Error: " + ex.Message,
                    data = ""
                };
            }
        }
    }
}
