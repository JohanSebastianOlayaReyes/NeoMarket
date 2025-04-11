using Data;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business;

/// <summary>
/// Clase de negocio encargada de la lógica relacionada con el inventario del sistema.
/// </summary>
public class InventoryBusiness
{
    private readonly InventoryData _inventoryData;
    private readonly ILogger _logger;

    public InventoryBusiness(InventoryData inventoryData, ILogger logger)
    {
        _inventoryData = inventoryData;
        _logger = logger;
    }

    // Método para obtener todos los inventarios como DTOs
    public async Task<IEnumerable<InventoryDto>> GetAllInventoriesAsync()
    {
        try
        {
            var inventories = await _inventoryData.GetAllAsync();

            return MapToDTOList(inventories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los inventarios");
            throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de inventarios", ex);
        }
    }

    // Método para obtener un inventario por ID como DTO
    public async Task<InventoryDto> GetInventoryByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Se intentó obtener un inventario con ID inválido: {InventoryId}", id);
            throw new Utilities.Exceptions.ValidationException("id", "El ID del inventario debe ser mayor que cero");
        }

        try
        {
            var inventory = await _inventoryData.GetByIdAsync(id);
            if (inventory == null)
            {
                _logger.LogInformation("No se encontró ningún inventario con ID: {InventoryId}", id);
                throw new EntityNotFoundException("Inventory", id);
            }
              return MapToDTO(inventory);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el inventario con ID: {InventoryId}", id);
            throw new ExternalServiceException("Base de datos", $"Error al recuperar el inventario con ID {id}", ex);
        }
    }

    // Método para crear un inventario desde un DTO
    public async Task<InventoryDto> CreateInventoryAsync(InventoryDto inventoryDto)
    {
        try
        {
            ValidateInventory(inventoryDto);

            var inventory = MapToEntity(inventoryDto);

            var createdInventory = await _inventoryData.CreateAsync(inventory);

            return MapToDTO(createdInventory);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear nuevo inventario: {ProductName}", inventoryDto?.NameInventory ?? "null");
            throw new ExternalServiceException("Base de datos", "Error al crear el inventario", ex);
        }
    }

    // Método para validar el DTO
    private void ValidateInventory(InventoryDto inventoryDto)
    {
        if (inventoryDto == null)
        {
            throw new Utilities.Exceptions.ValidationException("El objeto inventario no puede ser nulo");
        }

        if (string.IsNullOrWhiteSpace(inventoryDto.NameInventory))
        {
            _logger.LogWarning("Se intentó crear/actualizar un inventario con ProductName vacío");
            throw new Utilities.Exceptions.ValidationException("ProductName", "El nombre del producto es obligatorio");
        }
    }
    // Método para mapear de Rol a RolDTO
    private InventoryDto MapToDTO(Inventory inventory)
    {
        return new InventoryDto
        {
            Id = inventory.Id,
            NameInventory = inventory.NameInventory,
            Status = inventory.Status,
            DescriptionInvetory = inventory.DescriptionInvetory,
            Observations = inventory.Observations,
            ZoneProduct = inventory.ZoneProduct
        };
    }

    // Método para mapear de RolDTO a Rol
    private Inventory MapToEntity(InventoryDto inventoryDto)
    {
        return new Inventory
        {
            Id = inventoryDto.Id,
            NameInventory = inventoryDto.NameInventory,
            Status = inventoryDto.Status,
            DescriptionInvetory = inventoryDto.DescriptionInvetory,
            Observations = inventoryDto.Observations,
            ZoneProduct = inventoryDto.ZoneProduct
        };
    }

    // Método para mapear una lista de Rol a una lista de RolDTO
    private IEnumerable<InventoryDto> MapToDTOList(IEnumerable<Inventory> inventories)
    {
        var inventoriesDto = new List<InventoryDto>();
        foreach (var inventory in inventories)
        {
            inventoriesDto.Add(MapToDTO(inventory));
        }
        return inventoriesDto;
    }
}
