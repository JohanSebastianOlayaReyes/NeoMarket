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
    /// Controlador para la gestión de roles en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RolController : ControllerBase
    {
        private readonly RolBusiness _rolBusiness;
        private readonly ILogger<RolController> _logger;

        /// <summary>
        /// Constructor del controlador de roles
        /// </summary>
        /// <param name="rolBusiness">Capa de negocio de roles</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public RolController(RolBusiness rolBusiness, ILogger<RolController> logger)
        {
            _rolBusiness = rolBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los roles del sistema
        /// </summary>
        /// <returns>Lista de roles</returns>
        /// <response code="200">Retorna la lista de roles</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _rolBusiness.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener roles");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un rol específico por su ID
        /// </summary>
        /// <param name="id">ID del rol</param>
        /// <returns>Rol solicitado</returns>
        /// <response code="200">Retorna el rol solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Rol no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RolDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRolById(int id)
        {
            try
            {
                var rol = await _rolBusiness.GetRolByIdAsync(id);
                return Ok(rol);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el rol con ID: {RolId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rol no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener rol con ID: {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo rol en el sistema
        /// </summary>
        /// <param name="rolDto">Datos del rol a crear</param>
        /// <returns>Rol creado</returns>
        /// <response code="201">Retorna el rol creado</response>
        /// <response code="400">Datos del rol no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(RolDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRol([FromBody] RolDto rolDto)
        {
            try
            {
                var createdRol = await _rolBusiness.CreateRolAsync(rolDto);
                return CreatedAtAction(nameof(GetRolById), new { id = createdRol.Id }, createdRol);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear rol");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear rol");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un rol existente en el sistema
        /// </summary>
        /// <param name="id">ID del rol a actualizar</param>
        /// <param name="rolDto">Datos actualizados del rol</param>
        /// <returns>Resultado de la operación</returns>
        /// <response code="200">Rol actualizado correctamente</response>
        /// <response code="400">Datos del rol no válidos</response>
        /// <response code="404">Rol no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRol(int id, [FromBody] RolDto rolDto)
        {
            if (id != rolDto.Id)
            {
                return BadRequest(new { message = "El ID del rol no coincide con el ID proporcionado en el cuerpo de la solicitud." });
            }

            try
            {
                var result = await _rolBusiness.UpdateRolAsync(rolDto);
                return Ok(new { message = "Rol actualizado correctamente", success = result });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar el rol con ID: {RolId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rol no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el rol con ID: {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza campos específicos de un rol
        /// </summary>
        /// <param name="id">ID del rol a actualizar</param>
        /// <param name="updatedFields">Campos a actualizar</param>
        /// <returns>Resultado de la operación</returns>
        /// <response code="200">Rol actualizado correctamente</response>
        /// <response code="400">Datos no válidos</response>
        /// <response code="404">Rol no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePartialRol(int id, [FromBody] RolDto updatedFields)
        {
            if (updatedFields == null)
            {
                return BadRequest(new { message = "Los datos proporcionados no pueden ser nulos." });
            }

            try
            {
                var result = await _rolBusiness.UpdatePartialRolAsync(id, updatedFields);
                return Ok(new { message = "Rol actualizado parcialmente correctamente", success = result });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar parcialmente el rol con ID: {RolId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rol no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar parcialmente el rol con ID: {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Realiza una eliminación lógica de un rol (marca como inactivo)
        /// </summary>
        /// <param name="id">ID del rol a eliminar lógicamente</param>
        /// <returns>Resultado de la operación</returns>
        /// <response code="200">Rol marcado como inactivo correctamente</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Rol no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpDelete("soft-delete/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SoftDeleteRol(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "El ID del rol debe ser mayor a 0." });
            }

            try
            {
                var result = await _rolBusiness.SoftDeleteRolAsync(id);
                return Ok(new { message = "Rol marcado como inactivo correctamente", success = result });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rol no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al realizar la eliminación lógica del rol con ID: {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un rol del sistema
        /// </summary>
        /// <param name="id">ID del rol a eliminar</param>
        /// <returns>Resultado de la operación</returns>
        /// <response code="200">Rol eliminado correctamente</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Rol no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteRol(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "El ID del rol debe ser mayor a 0." });
            }

            try
            {
                var result = await _rolBusiness.DeleteRolAsync(id);
                return Ok(new { message = "Rol eliminado correctamente", success = result });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rol no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el rol con ID: {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}