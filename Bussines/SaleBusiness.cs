using Data;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business;

/// <summary>
/// Clase de negocio encargada de la lógica relacionada con las selecciones del sistema.
/// </summary>
public class SaleBusiness
{
    private readonly SaleData _seleData;
    private readonly ILogger _logger;

    public SaleBusiness(SaleData seleData, ILogger logger)
    {
        _seleData = seleData;
        _logger = logger;
    }

    // Método para obtener todas las selecciones como DTOs
    public async Task<IEnumerable<SaleDTO>> GetAllSeleAsync()
    {
        try
        {
            var selecciones = await _seleData.GetAllAsync();
            return MapToDTOList(selecciones);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las selecciones");
            throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de selecciones", ex);
        }
    }

    // Método para obtener una selección por ID como DTO
    public async Task<SaleDTO> GetSeleByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Se intentó obtener una selección con ID inválido: {SeleId}", id);
            throw new Utilities.Exceptions.ValidationException("id", "El ID de la selección debe ser mayor que cero");
        }

        try
        {
            var seleccion = await _seleData.GetByIdAsync(id);
            if (seleccion == null)
            {
                _logger.LogInformation("No se encontró ninguna selección con ID: {SeleId}", id);
                throw new EntityNotFoundException("Selección", id);
            }

            return MapToDTO(seleccion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la selección con ID: {SeleId}", id);
            throw new ExternalServiceException("Base de datos", $"Error al recuperar la selección con ID {id}", ex);
        }
    }

    // Método para crear una selección desde un DTO
    public async Task<SaleDTO> CreateSeleAsync(SaleDTO seleDto)
    {
        try
        {
            ValidateSele(seleDto);

            var seleccion = MapToEntity(seleDto);

            var seleccionCreada = await _seleData.CreateAsync(seleccion);
            return MapToDTO(seleccionCreada);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear nueva selección: {SeleNombre}", seleDto?.Totaly);
            throw new ExternalServiceException("Base de datos", "Error al crear la selección", ex);
        }
    }

    // Método para validar el DTO
    private void ValidateSele(SaleDTO seleDto)
    {
        if (seleDto == null)
        {
            throw new Utilities.Exceptions.ValidationException("El objeto selección no puede ser nulo");
        }

        if (seleDto.Totaly < 0)
        {
            _logger.LogWarning("Se intentó crear/actualizar una selección con un valor negativo en Totaly: {Totaly}", seleDto.Totaly);
            throw new Utilities.Exceptions.ValidationException("Totaly", "El valor de Totaly no puede ser negativo");
        }

    }

    // Método para mapear de Sele a SeleDTO
    private SaleDTO MapToDTO(Sale seleccion)
    {
        return new SaleDTO
        {
            Id = seleccion.Id,
            Date = seleccion.Date,
            Totaly = seleccion.Totaly,

        };
    }

    // Método para mapear de SeleDTO a Sele
    private Sale MapToEntity(SaleDTO seleDto)
    {
        return new Sale
        {
            Id = seleDto.Id,
            Date = seleDto.Date,
            Totaly = seleDto.Totaly
        };
    }

    // Método para mapear una lista de Sele a una lista de SeleDTO
    private IEnumerable<SaleDTO> MapToDTOList(IEnumerable<Sale> selecciones)
    {
        var seleccionesDTO = new List<SaleDTO>();
        foreach (var seleccion in selecciones)
        {
            seleccionesDTO.Add(MapToDTO(seleccion));
        }
        return seleccionesDTO;
    }
}
