using PruebaTecnica3.Server.Models;
using System.Data.SqlClient;
namespace PruebaTecnica3.Server
{
    public class UsuarioService
    {
        private readonly string _connectionString;

        public UsuarioService(IConfiguration configuration)
        {
            this._connectionString = configuration.GetConnectionString("default");
        }

        public List<Usuario> Listar()
        {
            var usuarios = new List<Usuario>();
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                string query = "SELECT * FROM users";

                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    usuarios.Add(
                        new Usuario
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Dni = reader["Dni"].ToString(),
                            Email = reader["Email"].ToString(),
                            FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            Rol = reader["Rol"].ToString()

                        });
                }
                return usuarios;
            }
        }

        public Usuario Registrar(Usuario user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO users (Dni,Nombre,Apellido,Email,Contrasena,FechaNacimiento,Estado) VALUES (@Dni,@Nombre,@Apellido,@Email,@Contrasena,@FechaNacimiento,@Estado)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Dni", user.Dni);
                cmd.Parameters.AddWithValue("@Nombre", user.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", user.Apellido);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                string hashed = BCrypt.Net.BCrypt.HashPassword(user.Contrasena);
                cmd.Parameters.AddWithValue("@Contrasena", hashed);
                cmd.Parameters.AddWithValue("@FechaNacimiento", user.FechaNacimiento);
                cmd.Parameters.AddWithValue("@Estado", user.Estado);

                conn.Open();

                if (cmd.ExecuteNonQuery() > 0)
                {
                    conn.Close();
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public Usuario Login(string correo, string contrasena)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM users where Email = @Email";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Email", correo);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string contrasenaAlmacena = reader["Contrasena"].ToString();
                    if (BCrypt.Net.BCrypt.Verify(contrasena, contrasenaAlmacena))
                    {
                        return new Usuario
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Nombre"].ToString(),
                            Dni = reader["Nombre"].ToString(),
                            Email = reader["Nombre"].ToString(),
                            FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            Rol = reader["Rol"].ToString()
                        };

                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
        }
        //crud

        public bool Crear(Usuario user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO users (Dni,Nombre,Apellido,Email,Contrasena,FechaNacimiento,Estado, Rol) VALUES (@Dni,@Nombre,@Apellido,@Email,@Contrasena,@FechaNacimiento,@Estado, @Rol)";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Dni", user.Dni);
                    cmd.Parameters.AddWithValue("@Nombre", user.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", user.Apellido);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    string hashed = BCrypt.Net.BCrypt.HashPassword(user.Contrasena);
                    cmd.Parameters.AddWithValue("@Contrasena", hashed);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", user.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Estado", user.Estado);
                    cmd.Parameters.AddWithValue("@Rol", user.Rol);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601) // Violación de clave única
                {
                    return false;
                }
                else
                {

                    return false; ;
                }
            }
        }


        public bool Actualizar(Usuario user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM users where Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", user.Dni);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string query2 = "UPDATE users SET Dni = @Dni, Nombre = @Nombre, Apellido = @Apellido, Email = @Email, Contrasena = @Contrasena, FechaNacimiento = @FechaNacimiento, Estado = @Estado, Rol = @Rol WHERE Id = @Id";
                    SqlCommand cmd2 = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Dni", user.Dni);
                    cmd.Parameters.AddWithValue("@Nombre", user.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", user.Apellido);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    string hashed = BCrypt.Net.BCrypt.HashPassword(user.Contrasena);
                    cmd.Parameters.AddWithValue("@Contrasena", hashed);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", user.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Estado", user.Estado);
                    cmd.Parameters.AddWithValue("@Rol", user.Rol);  // Aquí actualizamos el rol
                    cmd.Parameters.AddWithValue("@Id", user.Id);

                    conn.Open();

                    int result = cmd2.ExecuteNonQuery();
                    conn.Close();

                    return result > 0;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Eliminar(Usuario user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM users where Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", user.Dni);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string query2 = "UPDATE users set Estado = 0 WHERE Id = @Id";
                    SqlCommand cmd2 = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Id", user.Id);

                    conn.Open();

                    int result = cmd2.ExecuteNonQuery();
                    conn.Close();

                    return result > 0;
                }
                else
                {
                    return false;
                }
            }
        }

        public Usuario BuscarPorId(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM users where Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Usuario
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Nombre"].ToString(),
                        Dni = reader["Nombre"].ToString(),
                        Email = reader["Nombre"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                        Estado = Convert.ToBoolean(reader["Estado"]),
                        Rol = reader["Rol"].ToString()

                    };
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
