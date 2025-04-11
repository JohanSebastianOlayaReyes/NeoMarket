using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class SeleData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SeleData> _logger;

        public SeleData(ApplicationDbContext context, ILogger<SeleData> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Crear un nuevo Sele
        public async Task<Sele> CreateAsync(Sele sele)
        {
            try
            {
                await _context.Set<Sele>().AddAsync(sele);
                await _context.SaveChangesAsync();
                return sele;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el Sele: {ex.Message}");
                throw;
            }
        }

        // Obtener todos los Sele
        public async Task<IEnumerable<Sele>> GetAllAsync()
        {
            try
            {
                return await _context.Set<Sele>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener todos los Sele: {ex.Message}");
                throw;
            }
        }

        // Obtener un Sele por ID
        public async Task<Sele?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Sele>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el Sele con ID {id}: {ex.Message}");
                throw;
            }
        }

        // Actualizar un Sele
        public async Task<bool> UpdateAsync(Sele sele)
        {
            try
            {
                _context.Set<Sele>().Update(sele);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el Sele: {ex.Message}");
                return false;
            }
        }

        // Eliminar un Sele por ID
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var sele = await _context.Set<Sele>().FindAsync(id);
                if (sele == null)
                    return false;

                _context.Set<Sele>().Remove(sele);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el Sele: {ex.Message}");
                return false;
            }
        }
    }
}


