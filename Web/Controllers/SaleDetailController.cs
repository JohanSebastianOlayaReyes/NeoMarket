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
    /// Controlador para la gestión de detalles de ventas en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SaleDetailController : ControllerBase
    {
        private readonly SaleDetailBusiness _saleDetailBusiness;
        private readonly ILogger<SaleDetailController> _logger;

        /// <summary>
        /// Constructor del controlador de detalles de ventas
        /// </summary>
        /// <param name="saleDetailBusiness">Capa de negocio de detalles de ventas</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public SaleDetailController(SaleDetailBusiness saleDetailBusiness, ILogger<SaleDetailController> logger)
        {
            _saleDetailBusiness = saleDetailBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los detalles de ventas del sistema
        /// </summary>
        /// <returns>Lista de detalles de ventas</returns>
        /// <response code="200">Retorna la lista de detalles de ventas</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SaleDetailDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllSaleDetails()
        {
            try
            {
                var saleDetails = await _saleDetailBusiness.GetAllSeleDetailsAsync();
                return Ok(saleDetails);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener detalles de ventas");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un detalle de venta específico por su ID
        /// </summary>
        /// <param name="id">ID del detalle de venta</param>
        /// <returns>Detalle de venta solicitado</returns>
        /// <response code="200">Retorna el detalle de venta solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Detalle de venta no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SaleDetailDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSaleDetailById(int id)
        {
            try
            {
                var saleDetail = await _saleDetailBusiness.GetSeleDetailByIdAsync(id);
                return Ok(saleDetail);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el detalle de venta con ID: {SaleDetailId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Detalle de venta no encontrado con ID: {SaleDetailId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener detalle de venta con ID: {SaleDetailId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo detalle de venta en el sistema
        /// </summary>
        /// <param name="saleDetailDto">Datos del detalle de venta a crear</param>
        /// <returns>Detalle de venta creado</returns>
        /// <response code="201">Retorna el detalle de venta creado</response>
        /// <response code="400">Datos del detalle de venta no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(SaleDetailDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateSaleDetail([FromBody] SaleDetailDTO saleDetailDto)
        {
            try
            {
                var createdSaleDetail = await _saleDetailBusiness.CreateSeleDetailAsync(saleDetailDto);
                return CreatedAtAction(nameof(GetSaleDetailById), new { id = createdSaleDetail.Id }, createdSaleDetail);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear detalle de venta");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear detalle de venta");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
