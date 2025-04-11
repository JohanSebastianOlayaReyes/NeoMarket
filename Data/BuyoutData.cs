using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class BuyoutData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BuyoutData> _logger;

        public BuyoutData(ApplicationDbContext context, ILogger<BuyoutData> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Crear un nuevo Buyout
        public async Task<Buyout> CreateAsync(Buyout buyout)
        {
            try
            {
                await _context.Set<Buyout>().AddAsync(buyout);
                await _context.SaveChangesAsync();
                return buyout;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el Buyout: {ex.Message}");
                throw;
            }
        }

        // Obtener todos los Buyouts
        public async Task<IEnumerable<Buyout>> GetAllAsync()
        {
            try
            {
                return await _context.Set<Buyout>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener todos los Buyouts: {ex.Message}");
                throw;
            }
        }

        // Obtener un Buyout por ID
        public async Task<Buyout?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Buyout>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el Buyout con ID {id}: {ex.Message}");
                throw;
            }
        }

        // Actualizar un Buyout
        public async Task<bool> UpdateAsync(Buyout buyout)
        {
            try
            {
                _context.Set<Buyout>().Update(buyout);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el Buyout: {ex.Message}");
                return false;
            }
        }

        // Eliminar un Buyout por ID
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var buyout = await _context.Set<Buyout>().FindAsync(id);
                if (buyout == null)
                    return false;

                _context.Set<Buyout>().Remove(buyout);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el Buyout: {ex.Message}");
                return false;
            }
        }
    }
}
