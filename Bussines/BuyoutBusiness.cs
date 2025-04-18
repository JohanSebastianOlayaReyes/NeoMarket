using Data;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business;

/// <summary>
/// Clase de negocio encargada de la lógica relacionada con las compras del sistema.
/// </summary>
public class BuyoutBusiness
{
    private readonly BuyoutData _buyoutData;
    private readonly ILogger <BuyoutBusiness> _logger;

    public BuyoutBusiness(BuyoutData buyoutData, ILogger <BuyoutBusiness> logger)
    {
        _buyoutData = buyoutData;
        _logger = logger;
    }

    // Método para obtener todas las compras como DTOs
    public async Task<IEnumerable<BuyoutDto>> GetAllBuyoutsAsync()
    {
        try
        {
            var buyouts = await _buyoutData.GetAllAsync();
            return MapToDTOList(buyouts);
        }
        catch (Exception ex )
        {
            _logger.LogError(ex, "Error al obtener todas las compras");
            throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de compras", ex);
        }
    }

    // Método para obtener una compra por ID como DTO
    public async Task<BuyoutDto> GetBuyoutByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Se intentó obtener una compra con ID inválido: {BuyoutId}", id);
            throw new Utilities.Exceptions.ValidationException("id", "El ID de la compra debe ser mayor que cero");
        }

        try
        {
            var buyout = await _buyoutData.GetByIdAsync(id);
            if (buyout == null)
            {
                _logger.LogInformation("No se encontró ninguna compra con ID: {BuyoutId}", id);
                throw new EntityNotFoundException("Compra", id);
            }

            return MapToDTO(buyout);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la compra con ID: {BuyoutId}", id);
            throw new ExternalServiceException("Base de datos", $"Error al recuperar la compra con ID {id}", ex);
        }
    }

    // Método para crear una compra desde un DTO
    public async Task<BuyoutDto> CreateBuyoutAsync(BuyoutDto buyoutDto)
    {
        try
        {
            ValidateBuyout(buyoutDto);

            var buyout = MapToEntity(buyoutDto);

            var buyoutCreado = await _buyoutData.CreateAsync(buyout);
            return MapToDTO(buyoutCreado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear nueva compra: {BuyoutNombre}", buyoutDto?.Quantity);
            throw new ExternalServiceException("Base de datos", "Error al crear la compra", ex);
        }
    }

    // Método para validar el DTO
    private void ValidateBuyout(BuyoutDto buyoutDto)
    {
        if (buyoutDto == null)
        {
            throw new Utilities.Exceptions.ValidationException("El objeto compra no puede ser nulo");
        }

        if (buyoutDto.Quantity < 0)
        {
            _logger.LogWarning("Se intentó crear/actualizar una compra con Name vacío");
            throw new Utilities.Exceptions.ValidationException("Name", "El Name de la compra es obligatorio");
        }
    }

    // Método para mapear de Buyout a BuyoutDTO
    private BuyoutDto MapToDTO(Buyout buyout)
    {
        return new BuyoutDto
        {
            Id = buyout.Id,
            Quantity = buyout.Quantity,
            Date = buyout.Date
        };
    }

    // Método para mapear de BuyoutDTO a Buyout
    private Buyout MapToEntity(BuyoutDto buyoutDto)
    {
        return new Buyout
        {
            Id = buyoutDto.Id,
            Quantity = buyoutDto.Quantity,
            Date = buyoutDto.Date
        };
    }

    // Método para mapear una lista de Buyout a una lista de BuyoutDTO
    private IEnumerable<BuyoutDto> MapToDTOList(IEnumerable<Buyout> buyouts)
    {
        var buyoutsDTO = new List<BuyoutDto>();
        foreach (var buyout in buyouts)
        {
            buyoutsDTO.Add(MapToDTO(buyout));
        }
        return buyoutsDTO;
    }
}
