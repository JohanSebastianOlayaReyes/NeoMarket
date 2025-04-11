using Entity.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class ImageItemData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ImageItemData> _logger;

        public ImageItemData(ApplicationDbContext context, ILogger<ImageItemData> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Crear un nuevo ImageItem
        public async Task<ImageItem> CreateAsync(ImageItem imageItem)
        {
            try
            {
                await _context.Set<ImageItem>().AddAsync(imageItem);
                await _context.SaveChangesAsync();
                return imageItem;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el ImageItem: {ex.Message}");
                throw;
            }
        }

        // Obtener todos los ImageItems
        public async Task<IEnumerable<ImageItem>> GetAllAsync()
        {
            try
            {
                return await _context.Set<ImageItem>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener todos los ImageItems: {ex.Message}");
                throw;
            }
        }

        // Obtener un ImageItem por ID
        public async Task<ImageItem?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<ImageItem>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el ImageItem con ID {id}: {ex.Message}");
                throw;
            }
        }

        // Actualizar un ImageItem
        public async Task<bool> UpdateAsync(ImageItem imageItem)
        {
            try
            {
                _context.Set<ImageItem>().Update(imageItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el ImageItem: {ex.Message}");
                return false;
            }
        }

        // Eliminar un ImageItem por ID
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var imageItem = await _context.Set<ImageItem>().FindAsync(id);
                if (imageItem == null)
                    return false;

                _context.Set<ImageItem>().Remove(imageItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el ImageItem: {ex.Message}");
                return false;
            }
        }
    }
}
