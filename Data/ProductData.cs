using Entity.Model;
using Entity.Contexts;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Data
{
    public class ProductData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Product> _logger;

        public ProductData(ApplicationDbContext context, ILogger<Product> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Crear un nuevo item
        public async Task<Product?> CreateAsync(Product item)
        {
            try
            {
                await _context.Set<Product>().AddAsync(item);
                await _context.SaveChangesAsync();
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el item: {ex.Message}");
                throw;
            }
        }

        // Obtener todos los items
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Set<Product>().ToListAsync();
        }

        // Obtener un item por ID
        public async Task<Product?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Product>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el item con ID {id}: {ex.Message}");
                throw;
            }
        }

        // Actualizar un item
        public async Task<bool> UpdateAsync(Product item)
        {
            try
            {
                _context.Set<Product>().Update(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el item: {ex.Message}");
                throw;
            }
        }

        // Eliminar un item
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var item = await GetByIdAsync(id);
                if (item == null)
                {
                    return false;
                }
                _context.Set<Product>().Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el item con ID {id}: {ex.Message}");
                throw;
            }
        }

    }
}
