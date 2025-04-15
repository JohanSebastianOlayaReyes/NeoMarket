using Business;
using Entity.DTO;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de compras en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BuyoutController : ControllerBase
    {
        private readonly BuyoutBusiness _BuyoutBusiness;
        private readonly ILogger<BuyoutController> _logger;

        /// <summary>
        /// Constructor del controlador de compras
        /// </summary>
        /// <param name="BuyoutBusiness">Capa de negocio de compras</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public BuyoutController(BuyoutBusiness BuyoutBusiness, ILogger<BuyoutController> logger)
        {
            _BuyoutBusiness = BuyoutBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las compras del sistema
        /// </summary>
        /// <returns>Lista de compras</returns>
        /// <response code="200">Retorna la lista de compras</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BuyoutDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllBuyouts()
        {
            try
            {
                var buyouts = await _BuyoutBusiness.GetAllBuyoutsAsync();
                return Ok(buyouts);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener compras");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una compra específica por su ID
        /// </summary>
        /// <param name="id">ID de la compra</param>
        /// <returns>Compra solicitada</returns>
        /// <response code="200">Retorna la compra solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Compra no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BuyoutDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBuyoutById(int id)
        {
            try
            {
                var buyout = await _BuyoutBusiness.GetBuyoutByIdAsync(id);
                return Ok(buyout);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la compra con ID: {BuyoutId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Compra no encontrada con ID: {BuyoutId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener compra con ID: {BuyoutId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una nueva compra en el sistema
        /// </summary>
        /// <param name="BuyoutDto">Datos de la compra a crear</param>
        /// <returns>Compra creada</returns>
        /// <response code="201">Retorna la compra creada</response>
        /// <response code="400">Datos de la compra no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(BuyoutDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateBuyout([FromBody] BuyoutDto BuyoutDto)
        {
            try
            {
                var createdBuyout = await _BuyoutBusiness.CreateBuyoutAsync(BuyoutDto);
                return CreatedAtAction(nameof(GetBuyoutById), new { id = createdBuyout.Id }, createdBuyout);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear compra");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear compra");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
