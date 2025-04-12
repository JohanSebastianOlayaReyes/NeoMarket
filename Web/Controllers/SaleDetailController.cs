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
    /// Controlador para la gestión de detalles de selecciones en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SaleDetailController : ControllerBase
    {
        private readonly SaleDetailBusiness _SeleDetailBusiness;
        private readonly ILogger<SaleDetailController> _logger;

        /// <summary>
        /// Constructor del controlador de detalles de selecciones
        /// </summary>
        /// <param name="SeleDetailBusiness">Capa de negocio de detalles de selecciones</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public SaleDetailController(SaleDetailBusiness SeleDetailBusiness, ILogger<SaleDetailController> logger)
        {
            _SeleDetailBusiness = SeleDetailBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los detalles de selecciones del sistema
        /// </summary>
        /// <returns>Lista de detalles de selecciones</returns>
        /// <response code="200">Retorna la lista de detalles de selecciones</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SaleDetailDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllSeleDetails()
        {
            try
            {
                var seleDetails = await _SeleDetailBusiness.GetAllSeleDetailsAsync();
                return Ok(seleDetails);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener detalles de selecciones");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un detalle de selección específico por su ID
        /// </summary>
        /// <param name="id">ID del detalle de selección</param>
        /// <returns>Detalle de selección solicitado</returns>
        /// <response code="200">Retorna el detalle de selección solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Detalle de selección no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SaleDetailDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSeleDetailById(int id)
        {
            try
            {
                var seleDetail = await _SeleDetailBusiness.GetSeleDetailByIdAsync(id);
                return Ok(seleDetail);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el detalle de selección con ID: {SeleDetailId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Detalle de selección no encontrado con ID: {SeleDetailId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener detalle de selección con ID: {SeleDetailId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo detalle de selección en el sistema
        /// </summary>
        /// <param name="seleDetailDto">Datos del detalle de selección a crear</param>
        /// <returns>Detalle de selección creado</returns>
        /// <response code="201">Retorna el detalle de selección creado</response>
        /// <response code="400">Datos del detalle de selección no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(SaleDetailDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateSeleDetail([FromBody] SaleDetailDTO seleDetailDto)
        {
            try
            {
                var createdSeleDetail = await _SeleDetailBusiness.CreateSeleDetailAsync(seleDetailDto);
                return CreatedAtAction(nameof(GetSeleDetailById), new { id = createdSeleDetail.Id }, createdSeleDetail);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear detalle de selección");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear detalle de selección");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
