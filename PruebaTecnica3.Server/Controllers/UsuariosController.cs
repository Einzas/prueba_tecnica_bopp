using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PruebaTecnica3.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PruebaTecnica3.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private IConfiguration _configuration;

        public UsuariosController(UsuarioService usuarioService, IConfiguration configuration)
        {
            _usuarioService = usuarioService;
            _configuration = configuration;

        }
        [HttpGet("listar")]
        public IActionResult Listar()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var rToken = JWT.ValidarToken(identity);

            if (rToken.status != 200) return BadRequest(rToken);

            var rol = validarRol(rToken.rol);

            if (rol.status == 200)
            {


                if (_usuarioService.Listar() != null)
                {
                    return Ok(new
                    {
                        status = 200,
                        mensaje = "Datos Obtenidos",
                        data = _usuarioService.Listar()
                    });
                }
                else
                {
                    return Ok(new
                    {
                        status = 200,
                        mensaje = "No se encontraron datos",
                        data = ""
                    });

                }
            }
            else
            {
                return Ok(rol);
            }

        }

        [HttpPost("registro")]
        public IActionResult Registrar([FromBody] Usuario user)
        {
            if (_usuarioService.Registrar(user) != null)
            {
                var token = CrearToken(user);
                return Ok(new
                {
                    status = 200,
                    mensaje = "Registro Exitoso",
                    token
                });
            }
            else
            {
                return BadRequest(new
                {
                    status = 400,
                    mensaje = "Hubo un error en el registro"
                });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            Usuario usuario = _usuarioService.Login(login.correo, login.contrasena);
            if (usuario != null)
            {
                var token = CrearToken(usuario);
                return Ok(new
                {
                    token,
                    data = usuario,
                    status = 200
                });
            }
            return Ok(new
            {
                status = 201,
                mensaje = "Credenciales incorrectas"
            });
        }
        [HttpPost("crear")]
        public IActionResult Crear([FromBody] Usuario user)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var rToken = JWT.ValidarToken(identity);

            if (rToken.status != 200) return BadRequest(rToken);

            var rol = validarRol(rToken.rol);

            if (rol.status == 200)
            {
                if (_usuarioService.Crear(user) != false)
                {
                    return Ok(new
                    {
                        status = 200,
                        mensaje = "Usuario creado correctamente"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        status = 400,
                        mensaje = "No se creo el usuario"
                    });
                }
            }
            else
            {
                return Ok(rol);
            }
        }


        private string CrearToken(Usuario user)
        {
            var config = _configuration.GetSection("JWT").Get<JWT>();

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, config.Subject),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        new Claim("Id", user.Id.ToString()),
        new Claim("Nombre", user.Nombre),
        new Claim("Rol", user.Rol) // Asegúrate de agregar el rol si es necesario
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key));
            var signingKey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                config.Issuer,
                config.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(35),
                signingCredentials: signingKey
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private object validarRol(string rol)
        {
            if (rol == "admin")
            {
                return new
                {
                    status = 200,
                    mensaje = "Correcto",
                };
            }
            else
            {
                return new
                {
                    status = 400,
                    mensaje = "No posees los permisos para realizar esta accion",
                };
            }
        }



    }


}
