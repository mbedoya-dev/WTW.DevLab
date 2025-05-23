using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WCW.DevLab.Api.DTOs;

namespace WCW.DevLab.Api.Controllers
{
    /// <summary>
    /// Controlador API para la gestión de clientes.
    /// Permite obtener la lista de clientes desde la base de datos mediante un procedimiento almacenado.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        private readonly string _connectionString;

        /// <summary>
        /// Constructor que inyecta la configuración para obtener la cadena de conexión.
        /// </summary>
        /// <param name="configuration">Configuración de la aplicación</param>
        public ClientController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        /// <summary>
        /// Obtiene todos los clientes ejecutando el procedimiento almacenado 'sp_GetAllClients'.
        /// </summary>
        /// <returns>Lista de clientes en formato JSON o error 500 en caso de excepción.</returns>
        [HttpGet]
        public IActionResult GetAllClients()
        {
            var clients = new List<ClientDto>();

            try
            {
                // Establece conexión con la base de datos usando la cadena de conexión
                using (var connection = new SqlConnection(_connectionString))
                // Crea comando para ejecutar el procedimiento almacenado
                using (var command = new SqlCommand("sp_GetAllClients", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    // Ejecuta el lector para recorrer los resultados
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clients.Add(new ClientDto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                RazonSocial = reader.GetString(reader.GetOrdinal("RazonSocial")),
                                RFC = reader.GetString(reader.GetOrdinal("RFC")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
                            });
                        }
                    }
                }

                // Retorna la lista de clientes con código 200 OK
                return Ok(clients);
            }
            catch (SqlException sqlEx)
            {
                // Manejo específico para errores SQL
                // Retorna error 500 con mensaje de error SQL
                return StatusCode(500, $"Error al acceder a la base de datos: {sqlEx.Message}");
            }
            catch (System.Exception ex)
            {
                // Retorna otras excepciones como error 500
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
