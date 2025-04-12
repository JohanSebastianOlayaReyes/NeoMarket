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
    /// Controlador para la gestión de inventarios en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryBusiness _inventoryBusiness;
        private readonly ILogger<InventoryController> _logger;

        /// <summary>
        /// Constructor del controlador de inventarios
        /// </summary>
        /// <param name="inventoryBusiness">Capa de negocio de inventarios</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public InventoryController(InventoryBusiness inventoryBusiness, ILogger<InventoryController> logger)
        {
            _inventoryBusiness = inventoryBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los inventarios del sistema
        /// </summary>
        /// <returns>Lista de inventarios</returns>
        /// <response code="200">Retorna la lista de inventarios</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InventoryDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllInventories()
        {
            try
            {
                var inventories = await _inventoryBusiness.GetAllInventoriesAsync();
                return Ok(inventories);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener inventarios");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un inventario específico por su ID
        /// </summary>
        /// <param name="id">ID del inventario</param>
        /// <returns>Inventario solicitado</returns>
        /// <response code="200">Retorna el inventario solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Inventario no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InventoryDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetInventoryById(int id)
        {
            try
            {
                var inventory = await _inventoryBusiness.GetInventoryByIdAsync(id);
                return Ok(inventory);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el inventario con ID: {InventoryId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Inventario no encontrado con ID: {InventoryId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener inventario con ID: {InventoryId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo inventario en el sistema
        /// </summary>
        /// <param name="inventoryDto">Datos del inventario a crear</param>
        /// <returns>Inventario creado</returns>
        /// <response code="201">Retorna el inventario creado</response>
        /// <response code="400">Datos del inventario no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(InventoryDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryDto inventoryDto)
        {
            try
            {
                var createdInventory = await _inventoryBusiness.CreateInventoryAsync(inventoryDto);
                return CreatedAtAction(nameof(GetInventoryById), new { id = createdInventory.Id }, createdInventory);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear inventario");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear inventario");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
