using AutoMapper;
using Business.Strategies.Interfaces;
using Data.Repository.Interfaces;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Business.Strategies.Impl
{
    /// <summary>
    /// Implementación estándar de la estrategia para operaciones con roles.
    /// </summary>
    public class StandardRolStrategy : IRolStrategy
    {
        private readonly IRolRepository _rolRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StandardRolStrategy> _logger;

        /// <summary>
        /// Constructor de la estrategia estándar.
        /// </summary>
        /// <param name="rolRepository">Repositorio de roles.</param>
        /// <param name="mapper">Mapper para la conversión entre entidades y DTOs.</param>
        /// <param name="logger">Logger para registro de eventos.</param>
        public StandardRolStrategy(IRolRepository rolRepository, IMapper mapper, ILogger<StandardRolStrategy> logger)
        {
            _rolRepository = rolRepository ?? throw new ArgumentNullException(nameof(rolRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<RolDto>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _rolRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<RolDto>>(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
            }
        }

        /// <inheritdoc />
        public async Task<RolDto> GetRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un rol con ID inválido: {RolId}", id);
                throw new ValidationException("id", "El ID del rol debe ser mayor que cero");
            }

            try
            {
                var rol = await _rolRepository.GetByIdAsync(id);
                if (rol == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {RolId}", id);
                    throw new EntityNotFoundException("Rol", id);
                }

                return _mapper.Map<RolDto>(rol);
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el rol con ID {id}", ex);
            }
        }

        /// <inheritdoc />
        public async Task<RolDto> CreateRolAsync(RolDto rolDto)
        {
            try
            {
                ValidateRol(rolDto);

                var rol = _mapper.Map<Rol>(rolDto);
                var createdRol = await _rolRepository.AddAsync(rol);

                return _mapper.Map<RolDto>(createdRol);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", rolDto?.NameRol ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);
            }
        }

        /// <inheritdoc />
        public async Task<bool> UpdateRolAsync(RolDto rolDto)
        {
            try
            {
                ValidateRol(rolDto);

                // Verificar si el rol existe
                var existingRol = await _rolRepository.GetByIdAsync(rolDto.Id);
                if (existingRol == null)
                {
                    _logger.LogWarning("No se encontró el rol con ID {RolId} para actualizar", rolDto.Id);
                    throw new EntityNotFoundException("Rol", rolDto.Id);
                }

                // Mapear los cambios al modelo
                var rol = _mapper.Map<Rol>(rolDto);

                // Actualizar el rol
                return await _rolRepository.UpdateAsync(rol);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el rol con ID {RolId}", rolDto.Id);
                throw new ExternalServiceException("Base de datos", $"Error al actualizar el rol con ID {rolDto.Id}", ex);
            }
        }

        /// <inheritdoc />
        public async Task<bool> UpdatePartialRolAsync(int id, RolDto updatedFields)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó actualizar un rol con un ID inválido: {RolId}", id);
                throw new ValidationException("id", "El ID del rol debe ser mayor a 0");
            }

            try
            {
                // Obtener el rol existente
                var existingRol = await _rolRepository.GetByIdAsync(id);
                if (existingRol == null)
                {
                    _logger.LogInformation("No se encontró el rol con ID {RolId} para actualización parcial", id);
                    throw new EntityNotFoundException("Rol", id);
                }

                // Actualizar solo los campos proporcionados
                if (!string.IsNullOrWhiteSpace(updatedFields.NameRol))
                {
                    existingRol.NameRol = updatedFields.NameRol;
                }
                if (updatedFields.Description != null)
                {
                    existingRol.Description = updatedFields.Description;
                }
                if (updatedFields.Status != existingRol.Status)
                {
                    existingRol.Status = updatedFields.Status;
                }

                // Guardar los cambios
                return await _rolRepository.UpdateAsync(existingRol);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar parcialmente el rol con ID {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al actualizar parcialmente el rol con ID {id}", ex);
            }
        }

        /// <inheritdoc />
        public async Task<bool> SoftDeleteRolAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó realizar una eliminación lógica con un ID inválido: {RolId}", id);
                throw new ValidationException("id", "El ID del rol debe ser mayor a 0");
            }

            try
            {
                // El repositorio maneja la verificación de existencia
                return await _rolRepository.SoftDeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al realizar la eliminación lógica del rol con ID {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al realizar la eliminación lógica del rol con ID {id}", ex);
            }
        }

        /// <inheritdoc />
        public async Task<bool> DeleteRolAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un rol con un ID inválido: {RolId}", id);
                throw new ValidationException("id", "El ID del rol debe ser mayor a 0");
            }

            try
            {
                return await _rolRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el rol con ID {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el rol con ID {id}", ex);
            }
        }

        /// <summary>
        /// Valida los datos de un rol.
        /// </summary>
        /// <param name="rolDto">DTO del rol a validar.</param>
        protected virtual void ValidateRol(RolDto rolDto)
        {
            if (rolDto == null)
            {
                throw new ValidationException("El objeto rol no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(rolDto.NameRol))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con nombre vacío");
                throw new ValidationException("NameRol", "El nombre del rol es obligatorio");
            }
        }
    }
}