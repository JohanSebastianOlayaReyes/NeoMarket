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
    /// Controlador para la gestión de notificaciones en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationBusiness _NotificationBusiness;
        private readonly ILogger<NotificationController> _logger;

        /// <summary>
        /// Constructor del controlador de notificaciones
        /// </summary>
        /// <param name="NotificationBusiness">Capa de negocio de notificaciones</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public NotificationController(NotificationBusiness NotificationBusiness, ILogger<NotificationController> logger)
        {
            _NotificationBusiness = NotificationBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las notificaciones del sistema
        /// </summary>
        /// <returns>Lista de notificaciones</returns>
        /// <response code="200">Retorna la lista de notificaciones</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NotificationDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllNotifications()
        {
            try
            {
                var notifications = await _NotificationBusiness.GetAllNotificationsAsync();
                return Ok(notifications);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener notificaciones");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una notificación específica por su ID
        /// </summary>
        /// <param name="id">ID de la notificación</param>
        /// <returns>Notificación solicitada</returns>
        /// <response code="200">Retorna la notificación solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Notificación no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NotificationDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            try
            {
                var notification = await _NotificationBusiness.GetNotificationByIdAsync(id);
                return Ok(notification);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la notificación con ID: {NotificationId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Notificación no encontrada con ID: {NotificationId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener notificación con ID: {NotificationId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una nueva notificación en el sistema
        /// </summary>
        /// <param name="notificationDto">Datos de la notificación a crear</param>
        /// <returns>Notificación creada</returns>
        /// <response code="201">Retorna la notificación creada</response>
        /// <response code="400">Datos de la notificación no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(NotificationDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationDto notificationDto)
        {
            try
            {
                var createdNotification = await _NotificationBusiness.CreateNotificationAsync(notificationDto);
                return CreatedAtAction(nameof(GetNotificationById), new { id = createdNotification.Id }, createdNotification);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear notificación");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear notificación");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
