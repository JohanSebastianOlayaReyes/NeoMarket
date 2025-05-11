using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces
{
    /// <summary>
    /// Interfaz genérica que define operaciones básicas de acceso a datos.
    /// </summary>
    /// <typeparam name="T">El tipo de entidad con la que trabaja el repositorio.</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Obtiene todas las entidades.
        /// </summary>
        /// <returns>Una colección de todas las entidades.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Obtiene una entidad por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>La entidad encontrada o null si no existe.</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Busca entidades que cumplan con una condición específica.
        /// </summary>
        /// <param name="predicate">Expresión que define la condición de búsqueda.</param>
        /// <returns>Colección de entidades que cumplen con la condición.</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Crea una nueva entidad en la base de datos.
        /// </summary>
        /// <param name="entity">Entidad a crear.</param>
        /// <returns>La entidad creada.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Actualiza una entidad existente en la base de datos.
        /// </summary>
        /// <param name="entity">Entidad con la información actualizada.</param>
        /// <returns>True si la actualización fue exitosa, False en caso contrario.</returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// Elimina una entidad de la base de datos.
        /// </summary>
        /// <param name="id">Identificador de la entidad a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa, False en caso contrario.</returns>
        Task<bool> DeleteAsync(int id);
    }
}