using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class SeleDetailData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SeleDetailData> _logger;

        public SeleDetailData(ApplicationDbContext context, ILogger<SeleDetailData> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Crear un nuevo SeleDetail
        public async Task<SeleDetail> CreateAsync(SeleDetail seleDetail)
        {
            try
            {
                await _context.Set<SeleDetail>().AddAsync(seleDetail);
                await _context.SaveChangesAsync();
                return seleDetail;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el SeleDetail: {ex.Message}");
                throw;
            }
        }

        // Obtener todos los SeleDetails
        public async Task<IEnumerable<SeleDetail>> GetAllAsync()
        {
            try
            {
                return await _context.Set<SeleDetail>()
                    .Include(sd => sd.Sele) // Relación con Sele
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener todos los SeleDetails: {ex.Message}");
                throw;
            }
        }

        // Obtener un SeleDetail por ID
        public async Task<SeleDetail?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<SeleDetail>()
                    .Include(sd => sd.Sele) // Relación con Sele
                    .FirstOrDefaultAsync(sd => sd.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el SeleDetail con ID {id}: {ex.Message}");
                throw;
            }
        }

        // Actualizar un SeleDetail
        public async Task<bool> UpdateAsync(SeleDetail seleDetail)
        {
            try
            {
                _context.Set<SeleDetail>().Update(seleDetail);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el SeleDetail: {ex.Message}");
                return false;
            }
        }

        // Eliminar un SeleDetail por ID
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var seleDetail = await _context.Set<SeleDetail>().FindAsync(id);
                if (seleDetail == null)
                    return false;

                _context.Set<SeleDetail>().Remove(seleDetail);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el SeleDetail: {ex.Message}");
                return false;
            }
        }
    }
}


