using Business.Factory;
using Business.Strategies.Interfaces;
using Entity.DTO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los roles del sistema.
    /// Utiliza el patrón Strategy para separar la lógica de negocio.
    /// </summary>
    public class RolBusiness
    {
        private readonly IRolStrategy _strategy;
        private readonly ILogger<RolBusiness> _logger;

        /// <summary>
        /// Constructor de la clase de negocio para roles.
        /// </summary>
        /// <param name="strategyFactory">Fábrica de estrategias para roles.</param>
        /// <param name="logger">Logger para registro de eventos.</param>
        public RolBusiness(IRolStrategyFactory strategyFactory, ILogger<RolBusiness> logger)
        {
            if (strategyFactory == null) throw new ArgumentNullException(nameof(strategyFactory));
            _strategy = strategyFactory.CreateStrategy();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene todos los roles como DTOs.
        /// </summary>
        /// <returns>Lista de roles como DTOs.</returns>
        public async Task<IEnumerable<RolDto>> GetAllRolesAsync()
        {
            try
            {
                return await _strategy.GetAllRolesAsync();
            }
            catch (Exception ex) when (!(ex is ValidationException || ex is EntityNotFoundException))
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
            }
        }

        /// <summary>
        /// Obtiene un rol por ID como DTO.
        /// </summary>
        /// <param name="id">ID del rol a obtener.</param>
        /// <returns>DTO del rol encontrado.</returns>
        public async Task<RolDto> GetRolByIdAsync(int id)
        {
            try
            {
                return await _strategy.GetRolByIdAsync(id);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el rol con ID: {RolId}", id);
                throw;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "No se encontró el rol con ID: {RolId}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el rol con ID {id}", ex);
            }
        }

        /// <summary>
        /// Crea un nuevo rol.
        /// </summary>
        /// <param name="rolDto">DTO con la información del rol a crear.</param>
        /// <returns>DTO del rol creado.</returns>
        public async Task<RolDto> CreateRolAsync(RolDto rolDto)
        {
            try
            {
                return await _strategy.CreateRolAsync(rolDto);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear rol");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", rolDto?.NameRol ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);
            }
        }

        /// <summary>
        /// Actualiza un rol existente.
        /// </summary>
        /// <param name="rolDto">DTO con la información actualizada.</param>
        /// <returns>True si la actualización fue exitosa, False en caso contrario.</returns>
        public async Task<bool> UpdateRolAsync(RolDto rolDto)
        {
            try
            {
                return await _strategy.UpdateRolAsync(rolDto);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar el rol con ID: {RolId}", rolDto.Id);
                throw;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "No se encontró el rol con ID: {RolId} para actualizar", rolDto.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el rol con ID: {RolId}", rolDto.Id);
                throw new ExternalServiceException("Base de datos", $"Error al actualizar el rol con ID {rolDto.Id}", ex);
            }
        }

        /// <summary>
        /// Actualiza campos específicos de un rol.
        /// </summary>
        /// <param name="id">ID del rol a actualizar.</param>
        /// <param name="updatedFields">DTO con los campos a actualizar.</param>
        /// <returns>True si la actualización fue exitosa, False en caso contrario.</returns>
        public async Task<bool> UpdatePartialRolAsync(int id, RolDto updatedFields)
        {
            try
            {
                return await _strategy.UpdatePartialRolAsync(id, updatedFields);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar parcialmente el rol con ID: {RolId}", id);
                throw;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "No se encontró el rol con ID: {RolId} para actualización parcial", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar parcialmente el rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al actualizar parcialmente el rol con ID {id}", ex);
            }
        }

        /// <summary>
        /// Realiza una eliminación lógica de un rol (marca como inactivo).
        /// </summary>
        /// <param name="id">ID del rol a eliminar lógicamente.</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario.</returns>
        public async Task<bool> SoftDeleteRolAsync(int id)
        {
            try
            {
                return await _strategy.SoftDeleteRolAsync(id);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al realizar eliminación lógica del rol con ID: {RolId}", id);
                throw;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "No se encontró el rol con ID: {RolId} para eliminación lógica", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al realizar la eliminación lógica del rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al realizar la eliminación lógica del rol con ID {id}", ex);
            }
        }

        /// <summary>
        /// Elimina un rol permanentemente.
        /// </summary>
        /// <param name="id">ID del rol a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa, False en caso contrario.</returns>
        public async Task<bool> DeleteRolAsync(int id)
        {
            try
            {
                return await _strategy.DeleteRolAsync(id);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al eliminar el rol con ID: {RolId}", id);
                throw;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "No se encontró el rol con ID: {RolId} para eliminar", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar el rol con ID {id}", ex);
            }
        }
    }
}