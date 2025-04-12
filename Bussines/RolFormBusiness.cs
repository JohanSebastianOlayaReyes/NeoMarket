using Data;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business;

/// <summary>
/// Clase de negocio encargada de la lógica relacionada con los formularios de roles en el sistema.
/// </summary>
public class RolFormBusiness
{
    private readonly RolFormData _rolFormData;
    private readonly ILogger _logger;

    public RolFormBusiness(RolFormData rolFormData, ILogger logger)
    {
        _rolFormData = rolFormData;
        _logger = logger;
    }

    // Método para obtener todos los formularios de roles como DTOs
    public async Task<IEnumerable<RolFormDto>> GetAllRolFormsAsync()
    {
        try
        {
            var rolForms = await _rolFormData.GetAllAsync();
            return MapToDTOList(rolForms);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los formularios de roles");
            throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de formularios de roles", ex);
        }
    }

    // Método para obtener un formulario de rol por ID como DTO
    public async Task<RolFormDto> GetRolFormByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Se intentó obtener un formulario de rol con ID inválido: {RolFormId}", id);
            throw new Utilities.Exceptions.ValidationException("id", "El ID del formulario de rol debe ser mayor que cero");
        }

        try
        {
            var rolForm = await _rolFormData.GetByIdAsync(id);
            if (rolForm == null)
            {
                _logger.LogInformation("No se encontró ningún formulario de rol con ID: {RolFormId}", id);
                throw new EntityNotFoundException("Formulario de rol", id);
            }

            return MapToDTO(rolForm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el formulario de rol con ID: {RolFormId}", id);
            throw new ExternalServiceException("Base de datos", $"Error al recuperar el formulario de rol con ID {id}", ex);
        }
    }

    // Método para crear un formulario de rol desde un DTO
    public async Task<RolFormDto> CreateRolFormAsync(RolFormDto rolFormDto)
    {
        try
        {
            ValidateRolForm(rolFormDto);

            var rolForm = MapToEntity(rolFormDto);

            var rolFormCreado = await _rolFormData.CreateAsync(rolForm);
            return MapToDTO(rolFormCreado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear nuevo formulario de rol: {RolFormNombre}", rolFormDto?.Id);
            throw new ExternalServiceException("Base de datos", "Error al crear el formulario de rol", ex);
        }
    }

    // Método para validar el DTO
    private void ValidateRolForm(RolFormDto rolFormDto)
    {
        if (rolFormDto == null)
        {
            throw new Utilities.Exceptions.ValidationException("El objeto formulario de rol no puede ser nulo");
        }

        if (rolFormDto.Id < 0)
        {
            _logger.LogWarning("Se intentó crear/actualizar un formulario de rol con Name vacío");
            throw new Utilities.Exceptions.ValidationException("Name", "El Name del formulario de rol es obligatorio");
        }
    }

    // Método para mapear de RolForm a RolFormDTO
    private RolFormDto MapToDTO(RolForm rolForm)
    {
        return new RolFormDto
        {
            Id = rolForm.Id,
            Permision = rolForm.Permision
        };
    }

    // Método para mapear de RolFormDTO a RolForm
    private RolForm MapToEntity(RolFormDto rolFormDTO)
    {
        return new RolForm
        {
            Id = rolFormDTO.Id,
            Permision = rolFormDTO.Permision
        };
    }

    // Método para mapear una lista de RolForm a una lista de RolFormDTO
    private IEnumerable<RolFormDto> MapToDTOList(IEnumerable<RolForm> rolForms)
    {
        var rolFormsDTO = new List<RolFormDto>();
        foreach (var rolForm in rolForms)
        {
            rolFormsDTO.Add(MapToDTO(rolForm));
        }
        return rolFormsDTO;
    }
}
