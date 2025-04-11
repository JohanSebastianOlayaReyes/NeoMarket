using Data;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business;

/// <summary>
/// Clase de negocio encargada de la lógica relacionada con los productos.
/// </summary>
public class ProductBusiness
{
    private readonly ProductData _productData;
    private readonly ILogger<ProductBusiness> _logger;

    public ProductBusiness(ProductData productData, ILogger<ProductBusiness> logger)
    {
        _productData = productData;
        _logger = logger;
    }

    // Método para obtener todos los productos como DTOs
    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        try
        {
            var products = await _productData.GetAllAsync();
            return MapToDTOList(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los productos");
            throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de productos", ex);
        }
    }

    // Método para obtener un producto por ID como DTO
    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Se intentó obtener un producto con ID inválido: {ProductId}", id);
            throw new Utilities.Exceptions.ValidationException("id", "El ID del producto debe ser mayor que cero");
        }

        try
        {
            var product = await _productData.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogInformation("No se encontró ningún producto con ID: {ProductId}", id);
                throw new EntityNotFoundException("Product", id);
            }

            return MapToDTO(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el producto con ID: {ProductId}", id);
            throw new ExternalServiceException("Base de datos", $"Error al recuperar el producto con ID {id}", ex);
        }
    }

    // Método para crear un producto desde un DTO
    public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
    {
        try
        {
            ValidateProduct(productDto);

            var product = MapToEntity(productDto);

            var productCreado = await _productData.CreateAsync(product);
            return MapToDTO(productCreado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear nuevo producto: {ProductName}", productDto?.NameProduct ?? "null");
            throw new ExternalServiceException("Base de datos", "Error al crear el producto", ex);
        }
    }

    // Método para validar el DTO
    private void ValidateProduct(ProductDto productDto)
    {
        if (productDto == null)
        {
            throw new Utilities.Exceptions.ValidationException("El objeto producto no puede ser nulo");
        }

        if (string.IsNullOrWhiteSpace(productDto.NameProduct))
        {
            _logger.LogWarning("Se intentó crear/actualizar un producto con Name vacío");
            throw new Utilities.Exceptions.ValidationException("Name", "El Name del producto es obligatorio");
        }
    }

    // Método para mapear de Product a ProductDTO
    private ProductDto MapToDTO(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            NameProduct = product.NameProduct,
            Description = product.Description,
            Price = product.Price,
            Status = product.Status
        };
    }

    // Método para mapear de ProductDTO a Product
    private Product MapToEntity(ProductDto productDto)
    {
        return new Product
        {
            Id = productDto.Id,
            NameProduct = productDto.NameProduct,
            Description = productDto.Description,
            Price = productDto.Price,
            Status = productDto.Status
        };
    }

    // Método para mapear una lista de Product a una lista de ProductDTO
    private IEnumerable<ProductDto> MapToDTOList(IEnumerable<Product> products)
    {
        var productDtos = new List<ProductDto>();
        foreach (var product in products)
        {
            productDtos.Add(MapToDTO(product));
        }
        return productDtos;
    }
}

