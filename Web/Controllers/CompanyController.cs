using Business;
using Entity.DTOs;
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
    /// Controlador para la gestión de compañías en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyBusiness _companyBusiness;
        private readonly ILogger<CompanyController> _logger;

        /// <summary>
        /// Constructor del controlador de compañías
        /// </summary>
        /// <param name="companyBusiness">Capa de negocio de compañías</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public CompanyController(CompanyBusiness companyBusiness, ILogger<CompanyController> logger)
        {
            _companyBusiness = companyBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las compañías del sistema
        /// </summary>
        /// <returns>Lista de compañías</returns>
        /// <response code="200">Retorna la lista de compañías</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CompanyDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllCompanies()
        {
            try
            {
                var companies = await _companyBusiness.GetAllCompaniesAsync();
                return Ok(companies);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener compañías");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una compañía específica por su ID
        /// </summary>
        /// <param name="id">ID de la compañía</param>
        /// <returns>Compañía solicitada</returns>
        /// <response code="200">Retorna la compañía solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Compañía no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CompanyDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            try
            {
                var company = await _companyBusiness.GetCompanyByIdAsync(id);
                return Ok(company);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la compañía con ID: {CompanyId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Compañía no encontrada con ID: {CompanyId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener compañía con ID: {CompanyId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una nueva compañía en el sistema
        /// </summary>
        /// <param name="companyDto">Datos de la compañía a crear</param>
        /// <returns>Compañía creada</returns>
        /// <response code="201">Retorna la compañía creada</response>
        /// <response code="400">Datos de la compañía no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(CompanyDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyDto companyDto)
        {
            try
            {
                var createdCompany = await _companyBusiness.CreateCompanyAsync(companyDto);
                return CreatedAtAction(nameof(GetCompanyById), new { id = createdCompany.Id }, createdCompany);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear compañía");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear compañía");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
