using Entity.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Strategies.Interfaces
{
    /// <summary>
    /// Interfaz que define la estrategia para operaciones con roles.
    /// </summary>
    public interface IRolStrategy
    {
        /// <summary>
        /// Obtiene todos los roles.
        /// </summary>
        /// <returns>Lista de roles.</returns>
        Task<IEnumerable<RolDto>> GetAllRolesAsync();

        /// <summary>
        /// Obtiene un rol por su ID.
        /// </summary>
        /// <param name="id">ID del rol.</param>
        /// <returns>El rol encontrado.</returns>
        Task<RolDto> GetRolByIdAsync(int id);

        /// <summary>
        /// Crea un nuevo rol.
        /// </summary>
        /// <param name="rolDto">Datos del rol a crear.</param>
        /// <returns>El rol creado.</returns>
        Task<RolDto> CreateRolAsync(RolDto rolDto);

        /// <summary>
        /// Actualiza un rol existente.
        /// </summary>
        /// <param name="rolDto">Datos actualizados del rol.</param>
        /// <returns>True si la actualización fue exitosa, False en caso contrario.</returns>
        Task<bool> UpdateRolAsync(RolDto rolDto);

        /// <summary>
        /// Actualiza parcialmente un rol.
        /// </summary>
        /// <param name="id">ID del rol a actualizar.</param>
        /// <param name="updatedFields">Campos a actualizar.</param>
        /// <returns>True si la actualización fue exitosa, False en caso contrario.</returns>
        Task<bool> UpdatePartialRolAsync(int id, RolDto updatedFields);

        /// <summary>
        /// Realiza una eliminación lógica de un rol.
        /// </summary>
        /// <param name="id">ID del rol a eliminar lógicamente.</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario.</returns>
        Task<bool> SoftDeleteRolAsync(int id);

        /// <summary>
        /// Elimina un rol permanentemente.
        /// </summary>
        /// <param name="id">ID del rol a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa, False en caso contrario.</returns>
        Task<bool> DeleteRolAsync(int id);
    }
}