using Entity.Model;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces
{
    /// <summary>
    /// Interfaz que define operaciones específicas para el repositorio de Roles.
    /// </summary>
    public interface IRolRepository : IGenericRepository<Rol>
    {
        /// <summary>
        /// Busca un rol por su nombre.
        /// </summary>
        /// <param name="name">Nombre del rol a buscar.</param>
        /// <returns>El rol encontrado o null si no existe.</returns>
        Task<Rol> GetByNameAsync(string name);

        /// <summary>
        /// Realiza una eliminación lógica del rol (cambia su estado a inactivo).
        /// </summary>
        /// <param name="id">Identificador del rol a desactivar.</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario.</returns>
        Task<bool> SoftDeleteAsync(int id);
    }
}