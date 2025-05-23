using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WCW.DevLab.Api.DTOs;

namespace WCW.DevLab.Api.Controllers
{
    /// <summary>
    /// Controlador API para la gestión de facturas.
    /// Permite consultar y crear facturas utilizando procedimientos almacenados en SQL Server.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly string _connectionString;

        /// <summary>
        /// Constructor del controlador que recibe la configuración de la aplicación.
        /// </summary>
        /// <param name="configuration">Interfaz para acceder a los valores de configuración.</param>
        public InvoiceController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        /// <summary>
        /// Obtiene todas las facturas asociadas a un cliente específico.
        /// </summary>
        /// <param name="id">Identificador del cliente.</param>
        /// <returns>Listado de facturas</returns>
        [HttpGet("client/{id}")]
        public async Task<IActionResult> GetInvoicesByClient(int id)
        {
            var invoices = new List<InvoiceSummaryDto>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("sp_SearchInvoiceByClient", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdCliente", id);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            invoices.Add(new InvoiceSummaryDto
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                NumeroFactura = Convert.ToInt32(reader["NumeroFactura"]),
                                FechaEmisionFactura = Convert.ToDateTime(reader["FechaEmisionFactura"]),
                                NumeroTotalArticulos = Convert.ToInt32(reader["NumeroTotalArticulos"]),
                                SubTotalFacturas = Convert.ToDecimal(reader["SubTotalFacturas"]),
                                TotalImpuestos = Convert.ToDecimal(reader["TotalImpuestos"]),
                                TotalFactura = Convert.ToDecimal(reader["TotalFactura"]),
                                RazonSocial = reader["RazonSocial"]?.ToString() ?? string.Empty,
                                RFC = reader["RFC"]?.ToString() ?? string.Empty
                            });
                        }
                    }
                }

                return Ok(invoices);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Error en la base de datos: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene el detalle completo de una factura según su número.
        /// </summary>
        /// <param name="numero">Número de la factura.</param>
        /// <returns>Encabezado y detalle de la factura.</returns>
        [HttpGet("number/{numero}")]
        public async Task<IActionResult> GetInvoiceByNumber(int numero)
        {
            InvoiceHeaderDto? header = null;
            var details = new List<InvoiceItemDto>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("sp_SearchInvoiceByNumber", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@NumeroFactura", numero);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Encabezado
                        if (await reader.ReadAsync())
                        {
                            header = new InvoiceHeaderDto
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                NumeroFactura = Convert.ToInt32(reader["NumeroFactura"]),
                                FechaEmisionFactura = Convert.ToDateTime(reader["FechaEmisionFactura"]),
                                NumeroTotalArticulos = Convert.ToInt32(reader["NumeroTotalArticulos"]),
                                SubTotalFacturas = Convert.ToDecimal(reader["SubTotalFacturas"]),
                                TotalImpuestos = Convert.ToDecimal(reader["TotalImpuestos"]),
                                TotalFactura = Convert.ToDecimal(reader["TotalFactura"]),
                                RazonSocial = reader["RazonSocial"]?.ToString() ?? string.Empty,
                                RFC = reader["RFC"]?.ToString() ?? string.Empty
                            };
                        }

                        if (header == null)
                            return NotFound($"No se encontró una factura con el número {numero}.");

                        // Detalle
                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                details.Add(new InvoiceItemDto
                                {
                                    IdProducto = Convert.ToInt32(reader["IdProducto"]),
                                    NombreProducto = reader["NombreProducto"]?.ToString() ?? string.Empty,
                                    CantidadDeProducto = Convert.ToInt32(reader["CantidadDeProducto"]),
                                    PrecioUnitarioProducto = Convert.ToDecimal(reader["PrecioUnitarioProducto"]),
                                    SubtotalProducto = Convert.ToDecimal(reader["SubtotalProducto"]),
                                    Notas = reader["Notas"]?.ToString() ?? string.Empty,
                                    ImagenProducto = reader["ImagenProducto"]?.ToString() ?? string.Empty,
                                    Ext = reader["ext"]?.ToString() ?? string.Empty
                                });
                            }
                        }
                    }
                }

                var dto = new InvoiceDetailResultDto
                {
                    Encabezado = header,
                    Detalles = details
                };

                return Ok(dto);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Error en la base de datos: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }

        /// <summary>
        /// Crea una nueva factura y sus detalles asociados.
        /// </summary>
        /// <param name="request">Datos de la factura a registrar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceRequest request)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("sp_CreateInvoice", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros escalares
                    command.Parameters.AddWithValue("@FechaEmisionFactura", request.FechaEmisionFactura);
                    command.Parameters.AddWithValue("@IdCliente", request.IdCliente);
                    command.Parameters.AddWithValue("@NumeroFactura", request.NumeroFactura);
                    command.Parameters.AddWithValue("@NumeroTotalArticulos", request.NumeroTotalArticulos);
                    command.Parameters.AddWithValue("@SubTotalFacturas", request.SubTotalFacturas);
                    command.Parameters.AddWithValue("@TotalImpuestos", request.TotalImpuestos);
                    command.Parameters.AddWithValue("@TotalFactura", request.TotalFactura);

                    // Cargar detalles en un DataTable para el parámetro tipo tabla
                    var table = new DataTable();
                    table.Columns.Add("IdProducto", typeof(int));
                    table.Columns.Add("CantidadDeProducto", typeof(int));
                    table.Columns.Add("PrecioUnitarioProducto", typeof(decimal));
                    table.Columns.Add("SubtotalProducto", typeof(decimal));
                    table.Columns.Add("Notas", typeof(string));

                    foreach (var item in request.Detalles)
                    {
                        table.Rows.Add(item.IdProducto, item.CantidadDeProducto, item.PrecioUnitarioProducto, item.SubtotalProducto, item.Notas ?? string.Empty);
                    }

                    var detallesParam = new SqlParameter("@Detalles", SqlDbType.Structured)
                    {
                        TypeName = "DetalleFacturaType",
                        Value = table
                    };

                    command.Parameters.Add(detallesParam);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }

                return Ok(new { message = "Factura creada exitosamente" });
            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Error en la base de datos: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }
    }
}
