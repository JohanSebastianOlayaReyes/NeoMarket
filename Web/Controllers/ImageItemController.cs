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
    /// Controlador para la gestión de imágenes en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ImageItemController : ControllerBase
    {
        private readonly ImageItemBusiness _imageItemBusiness;
        private readonly ILogger<ImageItemController> _logger;

        /// <summary>
        /// Constructor del controlador de imágenes
        /// </summary>
        /// <param name="imageItemBusiness">Capa de negocio de imágenes</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public ImageItemController(ImageItemBusiness imageItemBusiness, ILogger<ImageItemController> logger)
        {
            _imageItemBusiness = imageItemBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las imágenes del sistema
        /// </summary>
        /// <returns>Lista de imágenes</returns>
        /// <response code="200">Retorna la lista de imágenes</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ImageItemDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllImageItems()
        {
            try
            {
                var imageItems = await _imageItemBusiness.GetAllImageItemsAsync();
                return Ok(imageItems);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener imágenes");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una imagen específica por su ID
        /// </summary>
        /// <param name="id">ID de la imagen</param>
        /// <returns>Imagen solicitada</returns>
        /// <response code="200">Retorna la imagen solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Imagen no encontrada</response>
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
                var imageItem = await _imageItemBusiness.GetImageItemByIdAsync(id);
                return Ok(imageItem);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la imagen con ID: {ImageItemId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Imagen no encontrada con ID: {ImageItemId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener imagen con ID: {ImageItemId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una nueva imagen en el sistema
        /// </summary>
        /// <param name="imageItemDto">Datos de la imagen a crear</param>
        /// <returns>Imagen creada</returns>
        /// <response code="201">Retorna la imagen creada</response>
        /// <response code="400">Datos de la imagen no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(ImageItemDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateImageItem([FromBody] ImageItemDTO imageItemDto)
        {
            try
            {
                var createdImageItem = await _imageItemBusiness.CreateImageItemAsync(imageItemDto);
                return CreatedAtAction(nameof(GetImageItemById), new { id = createdImageItem.Id }, createdImageItem);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear imagen");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear imagen");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
