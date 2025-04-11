using Data;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business;

/// <summary>
/// Clase de negocio encargada de la lógica relacionada con los movimientos de inventario.
/// </summary>
public class MovimientInventoryBusiness
{
    private readonly MovimientInventoryData _movimientInventoryData;
    private readonly ILogger<MovimientInventoryBusiness> _logger;

    public MovimientInventoryBusiness(MovimientInventoryData movimientInventoryData, ILogger<MovimientInventoryBusiness> logger)
    {
        _movimientInventoryData = movimientInventoryData;
        _logger = logger;
    }

    // Método para obtener todos los movimientos de inventario como DTOs
    public async Task<IEnumerable<MovimientInventoryDto>> GetAllMovimientInventoryAsync()
    {
        try
        {
            var movimientInventorys = await _movimientInventoryData.GetAllAsync();
            return MapToDTOList(movimientInventorys);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los movimientos de inventario");
            throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de movimientos de inventario", ex);
        }
    }

    // Método para obtener un movimiento de inventario por ID como DTO
    public async Task<MovimientInventoryDto> GetMovimientInventoryByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Se intentó obtener un movimiento de inventario con ID inválido: {MovimientInventoryId}", id);
            throw new Utilities.Exceptions.ValidationException("id", "El ID del movimiento de inventario debe ser mayor que cero");
        }

        try
        {
            var movimientInventory = await _movimientInventoryData.GetByIdAsync(id);
            if (movimientInventory == null)
            {
                _logger.LogInformation("No se encontró ningún movimiento de inventario con ID: {MovimientInventoryId}", id);
                throw new EntityNotFoundException("MovimientInventory", id);
            }

            return MapToDTO(movimientInventory);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el movimiento de inventario con ID: {MovimientInventoryId}", id);
            throw new ExternalServiceException("Base de datos", $"Error al recuperar el movimiento de inventario con ID {id}", ex);
        }
    }

    // Método para crear un movimiento de inventario desde un DTO
    public async Task<MovimientInventoryDto> CreateMovimientInventoryAsync(MovimientInventoryDto movimientInventoryDto)
    {
        try
        {
            ValidateMovimientInventory(movimientInventoryDto);

            var movimientInventory = MapToEntity(movimientInventoryDto);

            var movimientInventoryCreado = await _movimientInventoryData.CreateAsync(movimientInventory);
            return MapToDTO(movimientInventoryCreado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear nuevo movimiento de inventario: {MovimientInventoryName}", movimientInventoryDto?.Quantity ?? "null");
            throw new ExternalServiceException("Base de datos", "Error al crear el movimiento de inventario", ex);
        }
    }

    // Método para validar el DTO
    private void ValidateMovimientInventory(MovimientInventoryDto movimientInventoryDto)
    {
        if (movimientInventoryDto == null)
        {
            throw new Utilities.Exceptions.ValidationException("El objeto movimiento de inventario no puede ser nulo");
        }

        if (string.IsNullOrWhiteSpace(movimientInventoryDto.Quantity))
        {
            _logger.LogWarning("Se intentó crear/actualizar un movimiento de inventario con Name vacío");
            throw new Utilities.Exceptions.ValidationException("Name", "El Name del movimiento de inventario es obligatorio");
        }
    }

    // Método para mapear de MovimientInventory a MovimientInventoryDTO
    private MovimientInventoryDto MapToDTO(MovimientInventory movimientInventory)
    {
        return new MovimientInventoryDto
        {
            Id = movimientInventory.Id,
            Quantity = movimientInventory.Quantity,
            Description = movimientInventory.Description,
            Date = movimientInventory.Date
        };
    }

    // Método para mapear de MovimientInventoryDTO a MovimientInventory
    private MovimientInventory MapToEntity(MovimientInventoryDto movimientInventoryDto)
    {
        return new MovimientInventory
        {
            Id = movimientInventoryDto.Id,
            Quantity = movimientInventoryDto.Quantity,
            Description = movimientInventoryDto.Description,
            Date = movimientInventoryDto.Date
        };
    }

    // Método para mapear una lista de MovimientInventory a una lista de MovimientInventoryDTO
    private IEnumerable<MovimientInventoryDto> MapToDTOList(IEnumerable<MovimientInventory> movimientInventorys)
    {
        var movimientInventoryDtos = new List<MovimientInventoryDto>();
        foreach (var movimientInventory in movimientInventorys)
        {
            movimientInventoryDtos.Add(MapToDTO(movimientInventory));
        }
        return movimientInventoryDtos;
    }
}


