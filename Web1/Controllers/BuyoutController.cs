using Business;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Utilities.Exceptions;
using ValidationException = Utilities.Exceptions.ValidationException;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de buyouts en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BuyoutController : ControllerBase
    {
        private readonly BuyoutBusiness _BuyoutBusiness;
        private readonly ILogger<BuyoutController> _logger;

        /// <summary>
        /// Constructor del controlador de buyouts
        /// </summary>
        /// <param name="BuyoutBusiness">Capa de negocio de buyouts</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public BuyoutController(BuyoutBusiness BuyoutBusiness, ILogger<BuyoutController> logger)
        {
            _BuyoutBusiness = BuyoutBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los buyouts del sistema
        /// </summary>
        /// <returns>Lista de buyouts</returns>
        /// <response code="200">Retorna la lista de buyouts</response>
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
                _logger.LogError(ex, "Error al obtener buyouts");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un buyout específico por su ID
        /// </summary>
        /// <param name="id">ID del buyout</param>
        /// <returns>Buyout solicitado</returns>
        /// <response code="200">Retorna el buyout solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Buyout no encontrado</response>
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
                _logger.LogWarning(ex, "Validación fallida para el buyout con ID: {BuyoutId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Buyout no encontrado con ID: {BuyoutId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener buyout con ID: {BuyoutId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo buyout en el sistema
        /// </summary>
        /// <param name="buyoutDto">Datos del buyout a crear</param>
        /// <returns>Buyout creado</returns>
        /// <response code="201">Retorna el buyout creado</response>
        /// <response code="400">Datos del buyout no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(BuyoutDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateBuyout([FromBody] BuyoutDto buyoutDto)
        {
            try
            {
                var createdBuyout = await _BuyoutBusiness.CreateBuyoutAsync(buyoutDto);
                return CreatedAtAction(nameof(GetBuyoutById), new { id = createdBuyout.Id }, createdBuyout);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear buyout");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear buyout");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
