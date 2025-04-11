using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class RolFormData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RolFormData> _logger;

        public RolFormData(ApplicationDbContext context, ILogger<RolFormData> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Crear un nuevo RolForm
        public async Task<RolForm> CreateAsync(RolForm rolForm)
        {
            try
            {
                await _context.Set<RolForm>().AddAsync(rolForm);
                await _context.SaveChangesAsync();
                return rolForm;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el RolForm: {ex.Message}");
                throw;
            }
        }

        // Obtener todos los RolForms
        public async Task<IEnumerable<RolForm>> GetAllAsync()
        {
            try
            {
                return await _context.Set<RolForm>()
                    .Include(rf => rf.Rol)
                    .Include(rf => rf.Form)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener todos los RolForms: {ex.Message}");
                throw;
            }
        }

        // Obtener un RolForm por ID
        public async Task<RolForm?> GetByIdAsync(int rolId, int formId)
        {
            try
            {
                return await _context.Set<RolForm>()
                    .Include(rf => rf.Rol)
                    .Include(rf => rf.Form)
                    .FirstOrDefaultAsync(rf => rf.IdRol == rolId && rf.IdForm == formId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el RolForm con RolId {rolId} y FormId {formId}: {ex.Message}");
                throw;
            }
        }

        // Actualizar un RolForm
        public async Task<bool> UpdateAsync(RolForm rolForm)
        {
            try
            {
                _context.Set<RolForm>().Update(rolForm);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el RolForm: {ex.Message}");
                return false;
            }
        }

        // Eliminar un RolForm por RolId y FormId
        public async Task<bool> DeleteAsync(int rolId, int formId)
        {
            try
            {
                var rolForm = await _context.Set<RolForm>()
                    .FirstOrDefaultAsync(rf => rf.IdRol == rolId && rf.IdForm == formId);
                if (rolForm == null)
                    return false;

                _context.Set<RolForm>().Remove(rolForm);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el RolForm con RolId {rolId} y FormId {formId}: {ex.Message}");
                return false;
            }
        }
    }
}

