using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public class NotificationData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NotificationData> _logger;

        public NotificationData(ApplicationDbContext context, ILogger<NotificationData> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Crear una nueva Notification
        public async Task<Notification> CreateAsync(Notification notification)
        {
            try
            {
                await _context.Set<Notification>().AddAsync(notification);
                await _context.SaveChangesAsync();
                return notification;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear la Notification: {ex.Message}");
                throw;
            }
        }

        // Obtener todas las Notifications
        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            try
            {
                return await _context.Set<Notification>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener todas las Notifications: {ex.Message}");
                throw;
            }
        }

        // Obtener una Notification por ID
        public async Task<Notification?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Notification>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener la Notification con ID {id}: {ex.Message}");
                throw;
            }
        }

        // Actualizar una Notification
        public async Task<bool> UpdateAsync(Notification notification)
        {
            try
            {
                _context.Set<Notification>().Update(notification);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar la Notification: {ex.Message}");
                return false;
            }
        }

        // Eliminar una Notification por ID
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var notification = await _context.Set<Notification>().FindAsync(id);
                if (notification == null)
                    return false;

                _context.Set<Notification>().Remove(notification);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar la Notification: {ex.Message}");
                return false;
            }
        }
    }
}

