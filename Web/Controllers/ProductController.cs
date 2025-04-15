﻿using Business;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de productos en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly ProductBusiness _productBusiness;
        private readonly ILogger<ProductController> _logger;

        /// <summary>
        /// Constructor del controlador de productos
        /// </summary>
        /// <param name="productBusiness">Capa de negocio de productos</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public ProductController(ProductBusiness productBusiness, ILogger<ProductController> logger)
        {
            _productBusiness = productBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los productos del sistema
        /// </summary>
        /// <returns>Lista de productos</returns>
        /// <response code="200">Retorna la lista de productos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productBusiness.GetAllProductsAsync();
                return Ok(products);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener productos");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un producto específico por su ID
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>Producto solicitado</returns>
        /// <response code="200">Retorna el producto solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Producto no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productBusiness.GetProductByIdAsync(id);
                return Ok(product);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el producto con ID: {ProductId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Producto no encontrado con ID: {ProductId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener producto con ID: {ProductId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo producto en el sistema
        /// </summary>
        /// <param name="productDto">Datos del producto a crear</param>
        /// <returns>Producto creado</returns>
        /// <response code="201">Retorna el producto creado</response>
        /// <response code="400">Datos del producto no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var createdProduct = await _productBusiness.CreateProductAsync(productDto);
                return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear producto");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear producto");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
