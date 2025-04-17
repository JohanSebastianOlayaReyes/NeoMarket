using Data;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business;

/// <summary>
/// Clase de negocio encargada de la lógica relacionada con los módulos del sistema.
/// </summary>
public class ModuleBusiness
{
    private readonly ModuleData _moduleData;
    private readonly ILogger <ModuleBusiness> _logger;

    public ModuleBusiness(ModuleData moduleData, ILogger<ModuleBusiness> logger)
    {
        _moduleData = moduleData;
        _logger = logger;
    }

    // Método para obtener todos los módulos como DTOs
    public async Task<IEnumerable<ModuleDto>> GetAllModulesAsync()
    {
        try
        {
            var modules = await _moduleData.GetAllAsync();
            return MapToDTOList(modules);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los módulos");
            throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de módulos", ex);
        }
    }

    // Método para obtener un módulo por ID como DTO
    public async Task<ModuleDto> GetModuleByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Se intentó obtener un módulo con ID inválido: {ModuleId}", id);
            throw new ValidationException("id", "El ID del módulo debe ser mayor que cero");
        }

        try
        {
            var module = await _moduleData.GetByIdAsync(id);
            if (module == null)
            {
                _logger.LogInformation("No se encontró ningún módulo con ID: {ModuleId}", id);
                throw new EntityNotFoundException("Module", id);
            }

            return MapToDTO(module);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el módulo con ID: {ModuleId}", id);
            throw new ExternalServiceException("Base de datos", $"Error al recuperar el módulo con ID {id}", ex);
        }
    }

    // Método para crear un módulo desde un DTO
    public async Task<ModuleDto> CreateModuleAsync(ModuleDto moduleDto)
    {
        try
        {
            ValidateModule(moduleDto);

            var module = MapToEntity(moduleDto);

            var moduleCreado = await _moduleData.CreateAsync(module);

            return MapToDTO(moduleCreado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear nuevo módulo: {ModuleNombre}", moduleDto?.NameModule ?? "null");
            throw new ExternalServiceException("Base de datos", "Error al crear el módulo", ex);
        }
    }

    // Método para validar el DTO
    private void ValidateModule(ModuleDto moduleDto)
    {
        if (moduleDto == null)
        {
            throw new ValidationException("El objeto módulo no puede ser nulo");
        }

        if (string.IsNullOrWhiteSpace(moduleDto.NameModule))
        {
            _logger.LogWarning("Se intentó crear/actualizar un módulo con Name vacío");
            throw new ValidationException("Name", "El Name del módulo es obligatorio");
        }
    }

    // Método para mapear de Module a ModuleDto
    private ModuleDto MapToDTO(Module module)
    {
        return new ModuleDto
        {
            Id = module.Id,
            NameModule = module.NameModule,
            status = module.status
        };
    }

    // Método para mapear de ModuleDto a Module
    private Module MapToEntity(ModuleDto moduleDto)
    {
        return new Module
        {
            Id = moduleDto.Id,
            NameModule = moduleDto.NameModule,
            status = moduleDto.status
        };
    }

    // Método para mapear una lista de Module a una lista de ModuleDto
    private IEnumerable<ModuleDto> MapToDTOList(IEnumerable<Module> modules)
    {
        var modulesDTO = new List<ModuleDto>();
        foreach (var module in modules)
        {
            modulesDTO.Add(MapToDTO(module));
        }
        return modulesDTO;
    }
}

