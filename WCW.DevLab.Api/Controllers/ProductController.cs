using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WCW.DevLab.Api.DTOs;

namespace WCW.DevLab.Api.Controllers
{
    /// <summary>
    /// Controlador API para la gestión de productos.
    /// Permite obtener productos desde la base de datos usando un procedimiento almacenado.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly string _connectionString;

        /// <summary>
        /// Constructor que recibe la configuración para obtener la cadena de conexión.
        /// </summary>
        public ProductController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        /// <summary>
        /// Obtiene todos los productos ejecutando el procedimiento almacenado 'sp_GetAllProducts'.
        /// </summary>
        /// <returns>Lista de productos o un error en caso de fallo.</returns>
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = new List<ProductDto>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("sp_GetAllProducts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new ProductDto
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                NombreProducto = reader["NombreProducto"]?.ToString() ?? string.Empty,
                                PrecioUnitario = reader["PrecioUnitario"] != DBNull.Value ? Convert.ToDecimal(reader["PrecioUnitario"]) : 0,
                                ImagenProducto = reader["ImagenProducto"]?.ToString() ?? string.Empty,
                                Ext = reader["Ext"]?.ToString() ?? string.Empty
                            });
                        }
                    }
                }

                return Ok(products);
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(500, $"Error en la base de datos: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene un producto por su identificador ejecutando el procedimiento almacenado 'sp_GetProductById'.
        /// </summary>
        /// <param name="id">Identificador único del producto.</param>
        /// <returns>El producto encontrado o un código 404 si no existe.</returns>
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            ProductDto? product = null;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("sp_GetProductById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdProducto", id);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new ProductDto
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                NombreProducto = reader["NombreProducto"]?.ToString() ?? string.Empty,
                                PrecioUnitario = reader["PrecioUnitario"] != DBNull.Value ? Convert.ToDecimal(reader["PrecioUnitario"]) : 0,
                                ImagenProducto = reader["ImagenProducto"]?.ToString() ?? string.Empty,
                                Ext = reader["Ext"]?.ToString() ?? string.Empty
                            };
                        }
                    }
                }

                if (product == null)
                    return NotFound($"No se encontró un producto con ID {id}.");

                return Ok(product);
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(500, $"Error en la base de datos: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}