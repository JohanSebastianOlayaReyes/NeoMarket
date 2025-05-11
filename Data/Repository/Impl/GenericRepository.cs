using Data.Repository.Interfaces;
using Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository.Impl
{
    /// <summary>
    /// Implementación genérica del repositorio que proporciona operaciones CRUD básicas.
    /// </summary>
    /// <typeparam name="T">El tipo de entidad con la que trabaja el repositorio.</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly ILogger<GenericRepository<T>> _logger;

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="logger">Logger para registro de eventos.</param>
        public GenericRepository(ApplicationDbContext context, ILogger<GenericRepository<T>> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener todas las entidades de tipo {typeof(T).Name}");
                throw;
            }
        }

        /// <inheritdoc />
        public virtual async Task<T> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la entidad {typeof(T).Name} con ID {id}");
                throw;
            }
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar entidades de tipo {typeof(T).Name}");
                throw;
            }
        }

        /// <inheritdoc />
        public virtual async Task<T> AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al agregar una entidad de tipo {typeof(T).Name}");
                throw;
            }
        }

        /// <inheritdoc />
        public virtual async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar una entidad de tipo {typeof(T).Name}");
                throw;
            }
        }

        /// <inheritdoc />
        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity == null)
                    return false;

                _dbSet.Remove(entity);
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar la entidad {typeof(T).Name} con ID {id}");
                throw;
            }
        }
    }
}