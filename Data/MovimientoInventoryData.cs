using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class MovimientInventoryData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MovimientInventoryData> _logger;

        public MovimientInventoryData(ApplicationDbContext context, ILogger<MovimientInventoryData> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Crear un nuevo MovimientInventory
        public async Task<MovimientInventory> CreateAsync(MovimientInventory movimientInventory)
        {
            try
            {
                await _context.Set<MovimientInventory>().AddAsync(movimientInventory);
                await _context.SaveChangesAsync();
                return movimientInventory;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el MovimientInventory: {ex.Message}");
                throw;
            }
        }

        // Obtener todos los MovimientInventory
        public async Task<IEnumerable<MovimientInventory>> GetAllAsync()
        {
            try
            {
                return await _context.Set<MovimientInventory>()
                    .Include(mi => mi.Inventory)
                    .Include(mi => mi.Product)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener todos los MovimientInventory: {ex.Message}");
                throw;
            }
        }

        // Obtener un MovimientInventory por ID
        public async Task<MovimientInventory?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<MovimientInventory>()
                    .Include(mi => mi.Inventory)
                    .Include(mi => mi.Product)
                    .FirstOrDefaultAsync(mi => mi.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el MovimientInventory con ID {id}: {ex.Message}");
                throw;
            }
        }

        // Actualizar un MovimientInventory
        public async Task<bool> UpdateAsync(MovimientInventory movimientInventory)
        {
            try
            {
                _context.Set<MovimientInventory>().Update(movimientInventory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el MovimientInventory: {ex.Message}");
                return false;
            }
        }

        // Eliminar un MovimientInventory por ID
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var movimientInventory = await _context.Set<MovimientInventory>().FindAsync(id);
                if (movimientInventory == null)
                    return false;

                _context.Set<MovimientInventory>().Remove(movimientInventory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el MovimientInventory: {ex.Message}");
                return false;
            }
        }
    }
}

