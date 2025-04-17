using Data;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business;

/// <summary>
/// Clase de negocio encargada de la lógica relacionada con los usuarios del sistema.
/// </summary>
public class UserBusiness
{
    private readonly UserData _userData;
    private readonly ILogger <UserBusiness> _logger;

    public UserBusiness(UserData userData, ILogger <UserBusiness> logger)
    {
        _userData = userData;
        _logger = logger;
    }

    // Método para obtener todos los usuarios como DTOs
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        try
        {
            var users = await _userData.GetAllAsync();
            return MapToDTOList(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los usuarios");
            throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de usuarios", ex);
        }
    }

    // Método para obtener un usuario por ID como DTO
    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Se intentó obtener un usuario con ID inválido: {UserId}", id);
            throw new ValidationException("id", "El ID del usuario debe ser mayor que cero");
        }

        try
        {
            var user = await _userData.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogInformation("No se encontró ningún usuario con ID: {UserId}", id);
                throw new EntityNotFoundException("User", id);
            }

            return MapToDTO(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el usuario con ID: {UserId}", id);
            throw new ExternalServiceException("Base de datos", $"Error al recuperar el usuario con ID {id}", ex);
        }
    }

    // Método para crear un usuario desde un DTO
    public async Task<UserDto> CreateUserAsync(UserDto userDto)
    {
        try
        {
            ValidateUser(userDto);

            var user = MapToEntity(userDto);

            var userCreado = await _userData.CreateAsync(user);

            return MapToDTO(userCreado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear nuevo usuario: {UserNombre}", userDto?.UserName ?? "null");
            throw new ExternalServiceException("Base de datos", "Error al crear el usuario", ex);
        }
    }

    // Método para validar el DTO
    private void ValidateUser(UserDto userDto)
    {
        if (userDto == null)
        {
            throw new ValidationException("El objeto usuario no puede ser nulo");
        }

        if (string.IsNullOrWhiteSpace(userDto.UserName))
        {
            _logger.LogWarning("Se intentó crear/actualizar un usuario con Name vacío");
            throw new ValidationException("Name", "El Name del usuario es obligatorio");
        }
    }

    // Método para mapear de User a UserDto
    private UserDto MapToDTO(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Password = user.Password,
            Status = user.Status
        };
    }

    // Método para mapear de UserDto a User
    private User MapToEntity(UserDto userDto)
    {
        return new User
        {
            Id = userDto.Id,
            UserName = userDto.UserName,
            Password = userDto.Password,
            Status = userDto.Status
        };
    }

    // Método para mapear una lista de User a una lista de UserDto
    private IEnumerable<UserDto> MapToDTOList(IEnumerable<User> users)
    {
        var usersDTO = new List<UserDto>();
        foreach (var user in users)
        {
            usersDTO.Add(MapToDTO(user));
        }
        return usersDTO;
    }
}