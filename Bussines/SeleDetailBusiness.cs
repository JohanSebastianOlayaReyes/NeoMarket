using Data;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business;

/// <summary>
/// Clase de negocio encargada de la lógica relacionada con los detalles de las selecciones del sistema.
/// </summary>
public class SeleDetailBusiness
{
    private readonly SeleDetailData _seleDetailData;
    private readonly ILogger _logger;

    public SeleDetailBusiness(SeleDetailData seleDetailData, ILogger logger)
    {
        _seleDetailData = seleDetailData;
        _logger = logger;
    }

    // Método para obtener todos los detalles de selecciones como DTOs
    public async Task<IEnumerable<SeleDetailDto>> GetAllSeleDetailsAsync()
    {
        try
        {
            var seleDetails = await _seleDetailData.GetAllAsync();
            return MapToDTOList(seleDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los detalles de selecciones");
            throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de detalles de selecciones", ex);
        }
    }

    // Método para obtener un detalle de selección por ID como DTO
    public async Task<SeleDetailDto> GetSeleDetailByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Se intentó obtener un detalle de selección con ID inválido: {SeleDetailId}", id);
            throw new Utilities.Exceptions.ValidationException("id", "El ID del detalle de selección debe ser mayor que cero");
        }

        try
        {
            var seleDetail = await _seleDetailData.GetByIdAsync(id);
            if (seleDetail == null)
            {
                _logger.LogInformation("No se encontró ningún detalle de selección con ID: {SeleDetailId}", id);
                throw new EntityNotFoundException("Detalle de Selección", id);
            }

            return MapToDTO(seleDetail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el detalle de selección con ID: {SeleDetailId}", id);
            throw new ExternalServiceException("Base de datos", $"Error al recuperar el detalle de selección con ID {id}", ex);
        }
    }

    // Método para crear un detalle de selección desde un DTO
    public async Task<SeleDetailDto> CreateSeleDetailAsync(SeleDetailDto seleDetailDto)
    {
        try
        {
            ValidateSeleDetail(seleDetailDto);

            var seleDetail = MapToEntity(seleDetailDto);

            var seleDetailCreado = await _seleDetailData.CreateAsync(seleDetail);
            return MapToDTO(seleDetailCreado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear nuevo detalle de selección: {SeleDetailNombre}", seleDetailDto?.Quantity);
            throw new ExternalServiceException("Base de datos", "Error al crear el detalle de selección", ex);
        }
    }

    // Método para validar el DTO
    private void ValidateSeleDetail(SeleDetailDto seleDetailDto)
    {
        if (seleDetailDto == null)
        {
            throw new Utilities.Exceptions.ValidationException("El objeto detalle de selección no puede ser nulo");
        }

        if (seleDetailDto.Quantity < 0)
        {
            _logger.LogWarning("Se intentó crear/actualizar un detalle de selección con Name vacío");
            throw new Utilities.Exceptions.ValidationException("Name", "El Name del detalle de selección es obligatorio");
        }
    }

    // Método para mapear de SeleDetail a SeleDetailDTO
    private SeleDetailDto MapToDTO(SeleDetail seleDetail)
    {
        return new SeleDetailDto
        {
            Id = seleDetail.Id,
            Quantity = seleDetail.Quantity,
            Price = seleDetail.Price
        };
    }

    // Método para mapear de SeleDetailDTO a SeleDetail
    private SeleDetail MapToEntity(SeleDetailDto seleDetailDto)
    {
        return new SeleDetail
        {
            Id = seleDetailDto.Id,
            Quantity = seleDetailDto.Quantity,
            Price = seleDetailDto.Price
        };
    }

    // Método para mapear una lista de SeleDetail a una lista de SeleDetailDTO
    private IEnumerable<SeleDetailDto> MapToDTOList(IEnumerable<SeleDetail> seleDetails)
    {
        var seleDetailsDTO = new List<SeleDetailDto>();
        foreach (var seleDetail in seleDetails)
        {
            seleDetailsDTO.Add(MapToDTO(seleDetail));
        }
        return seleDetailsDTO;
    }
}
