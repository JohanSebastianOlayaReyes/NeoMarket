using Business;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de ventas en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SaleController : ControllerBase
    {
        private readonly SaleBusiness _saleBusiness;
        private readonly ILogger<SaleController> _logger;

        /// <summary>
        /// Constructor del controlador de ventas
        /// </summary>
        /// <param name="saleBusiness">Capa de negocio de ventas</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public SaleController(SaleBusiness saleBusiness, ILogger<SaleController> logger)
        {
            _saleBusiness = saleBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las ventas del sistema
        /// </summary>
        /// <returns>Lista de ventas</returns>
        /// <response code="200">Retorna la lista de ventas</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SaleDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllSales()
        {
            try
            {
                var sales = await _saleBusiness.GetAllSeleAsync();
                return Ok(sales);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener ventas");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una venta específica por su ID
        /// </summary>
        /// <param name="id">ID de la venta</param>
        /// <returns>Venta solicitada</returns>
        /// <response code="200">Retorna la venta solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Venta no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SaleDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSaleById(int id)
        {
            try
            {
                var sale = await _saleBusiness.GetSeleByIdAsync(id);
                return Ok(sale);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la venta con ID: {SaleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Venta no encontrada con ID: {SaleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener venta con ID: {SaleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una nueva venta en el sistema
        /// </summary>
        /// <param name="saleDto">Datos de la venta a crear</param>
        /// <returns>Venta creada</returns>
        /// <response code="201">Retorna la venta creada</response>
        /// <response code="400">Datos de la venta no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(SaleDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateSale([FromBody] SaleDTO saleDto)
        {
            try
            {
                var createdSale = await _saleBusiness.CreateSeleAsync(saleDto);
                return CreatedAtAction(nameof(GetSaleById), new { id = createdSale.Id }, createdSale);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear venta");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear venta");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
