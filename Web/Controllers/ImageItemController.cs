using Business;
using Data;
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
    /// Controlador para la gestión de elementos de imagen en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ImageItemController : ControllerBase
    {
        private readonly ImageItemBusiness _ImageItemBusiness;
        private readonly ILogger<ImageItemController> _logger;

        /// <summary>
        /// Constructor del controlador de elementos de imagen
        /// </summary>
        /// <param name="ImageItemBusiness">Capa de negocio de elementos de imagen</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public ImageItemController(ImageItemBusiness ImageItemBusiness, ILogger<ImageItemController> logger)
        {
            _ImageItemBusiness = ImageItemBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los elementos de imagen del sistema
        /// </summary>
        /// <returns>Lista de elementos de imagen</returns>
        /// <response code="200">Retorna la lista de elementos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ImageItemDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllImageItems()
        {
            try
            {
                var imageItems = await _ImageItemBusiness.GetAllImageItemsAsync();
                return Ok(imageItems);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener elementos de imagen");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un elemento de imagen específico por su ID
        /// </summary>
        /// <param name="id">ID del elemento de imagen</param>
        /// <returns>Elemento de imagen solicitado</returns>
        /// <response code="200">Retorna el elemento solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Elemento no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ImageItemDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetImageItemById(int id)
        {
            try
            {
                var imageItem = await _ImageItemBusiness.GetImageItemByIdAsync(id);
                return Ok(imageItem);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el elemento de imagen con ID: {ImageItemId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Elemento de imagen no encontrado con ID: {ImageItemId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener elemento de imagen con ID: {ImageItemId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo elemento de imagen en el sistema
        /// </summary>
        /// <param name="imageItemDto">Datos del elemento a crear</param>
        /// <returns>Elemento de imagen creado</returns>
        /// <response code="201">Retorna el elemento creado</response>
        /// <response code="400">Datos del elemento no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(ImageItemDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateImageItem([FromBody] ImageItemDTO imageItemDto)
        {
            try
            {
                var createdImageItem = await _ImageItemBusiness.CreateImageItemAsync(imageItemDto);
                return CreatedAtAction(nameof(GetImageItemById), new { id = createdImageItem.Id }, createdImageItem);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear elemento de imagen");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear elemento de imagen");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
