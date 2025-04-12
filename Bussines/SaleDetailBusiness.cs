using Data;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business;

/// <summary>
/// Clase de negocio encargada de la lógica relacionada con los detalles de las selecciones del sistema.
/// </summary>
public class SaleDetailBusiness
{
    private readonly SeleDetailData _seleDetailData;
    private readonly ILogger _logger;

    public SaleDetailBusiness(SeleDetailData seleDetailData, ILogger logger)
    {
        _seleDetailData = seleDetailData;
        _logger = logger;
    }

    // Método para obtener todos los detalles de selecciones como DTOs
    public async Task<IEnumerable<SaleDetailDTO>> GetAllSeleDetailsAsync()
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
    public async Task<SaleDetailDTO> GetSeleDetailByIdAsync(int id)
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
    public async Task<SaleDetailDTO> CreateSeleDetailAsync(SaleDetailDTO seleDetailDto)
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
    private void ValidateSeleDetail(SaleDetailDTO seleDetailDto)
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
    private SaleDetailDTO MapToDTO(SaleDetail seleDetail)
    {
        return new SaleDetailDTO
        {
            Id = seleDetail.Id,
            Quantity = seleDetail.Quantity,
            Price = seleDetail.Price
        };
    }

    // Método para mapear de SeleDetailDTO a SeleDetail
    private SaleDetail MapToEntity(SaleDetailDTO seleDetailDto)
    {
        return new SaleDetail
        {
            Id = seleDetailDto.Id,
            Quantity = seleDetailDto.Quantity,
            Price = seleDetailDto.Price
        };
    }

    // Método para mapear una lista de SeleDetail a una lista de SeleDetailDTO
    private IEnumerable<SaleDetailDTO> MapToDTOList(IEnumerable<SaleDetail> seleDetails)
    {
        var seleDetailsDTO = new List<SaleDetailDTO>();
        foreach (var seleDetail in seleDetails)
        {
            seleDetailsDTO.Add(MapToDTO(seleDetail));
        }
        return seleDetailsDTO;
    }
}
