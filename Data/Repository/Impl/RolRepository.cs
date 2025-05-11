using Data.Repository.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Data.Repository.Impl
{
    /// <summary>
    /// Implementación del repositorio específico para la entidad Rol.
    /// </summary>
    public class RolRepository : GenericRepository<Rol>, IRolRepository
    {
        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="logger">Logger para registro de eventos.</param>
        public RolRepository(ApplicationDbContext context, ILogger<RolRepository> logger)
            : base(context, logger)
        {
        }

        /// <inheritdoc />
        public async Task<Rol> GetByNameAsync(string name)
        {
            try
            {
                return await _dbSet.FirstOrDefaultAsync(r => r.NameRol == name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar rol por nombre: {name}");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                var rol = await GetByIdAsync(id);
                if (rol == null)
                    return false;

                rol.Status = false;
                return await UpdateAsync(rol);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al realizar la eliminación lógica del rol con ID {id}");
                throw;
            }
        }
    }
}