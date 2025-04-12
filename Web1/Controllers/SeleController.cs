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
    /// Controlador para la gestión de selecciones en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SeleController : ControllerBase
    {
        private readonly SeleBusiness _SeleBusiness;
        private readonly ILogger<SeleController> _logger;

        /// <summary>
        /// Constructor del controlador de selecciones
        /// </summary>
        /// <param name="SeleBusiness">Capa de negocio de selecciones</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public SeleController(SeleBusiness SeleBusiness, ILogger<SeleController> logger)
        {
            _SeleBusiness = SeleBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las selecciones del sistema
        /// </summary>
        /// <returns>Lista de selecciones</returns>
        /// <response code="200">Retorna la lista de selecciones</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SeleDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllSeles()
        {
            try
            {
                var seles = await _SeleBusiness.GetAllSeleAsync();
                return Ok(seles);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener selecciones");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una selección específica por su ID
        /// </summary>
        /// <param name="id">ID de la selección</param>
        /// <returns>Selección solicitada</returns>
        /// <response code="200">Retorna la selección solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Selección no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SeleDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSeleById(int id)
        {
            try
            {
                var sele = await _SeleBusiness.GetSeleByIdAsync(id);
                return Ok(sele);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la selección con ID: {SeleId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Selección no encontrada con ID: {SeleId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener selección con ID: {SeleId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una nueva selección en el sistema
        /// </summary>
        /// <param name="seleDto">Datos de la selección a crear</param>
        /// <returns>Selección creada</returns>
        /// <response code="201">Retorna la selección creada</response>
        /// <response code="400">Datos de la selección no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(SeleDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateSele([FromBody] SeleDto seleDto)
        {
            try
            {
                var createdSele = await _SeleBusiness.CreateSeleAsync(seleDto);
                return CreatedAtAction(nameof(GetSeleById), new { id = createdSele.Id }, createdSele);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear selección");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear selección");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
