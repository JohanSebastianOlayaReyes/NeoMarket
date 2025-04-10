﻿using Data;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business;

/// <summary>
/// Clase de negocio encargada de la lógica relacionada con las notificaciones del sistema.
/// </summary>
public class NotificacationBusiness
{
    private readonly NotificationData _notificationData;
    private readonly ILogger _logger;

    public NotificacationBusiness(NotificationData notificationData, ILogger logger)
    {
        _notificationData = notificationData;
        _logger = logger;
    }

    // Método para obtener todas las notificaciones como DTOs
    public async Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync()
    {
        try
        {
            var notifications = await _notificationData.GetAllAsync();
            return MapToDTOList(notifications);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las notificaciones");
            throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de notificaciones", ex);
        }
    }

    // Método para obtener una notificación por ID como DTO
    public async Task<NotificationDto> GetNotificationByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Se intentó obtener una notificación con ID inválido: {NotificationId}", id);
            throw new Utilities.Exceptions.ValidationException("id", "El ID de la notificación debe ser mayor que cero");
        }

        try
        {
            var notification = await _notificationData.GetByIdAsync(id);
            if (notification == null)
            {
                _logger.LogInformation("No se encontró ninguna notificación con ID: {NotificationId}", id);
                throw new EntityNotFoundException("Notificación", id);
            }

            return MapToDTO(notification);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la notificación con ID: {NotificationId}", id);
            throw new ExternalServiceException("Base de datos", $"Error al recuperar la notificación con ID {id}", ex);
        }
    }

    // Método para crear una notificación desde un DTO
    public async Task<NotificationDto> CreateNotificationAsync(NotificationDto notificationDto)
    {
        try
        {
            ValidateNotification(notificationDto);

            var notification = MapToEntity(notificationDto);

            var notificationCreada = await _notificationData.CreateAsync(notification);
            return MapToDTO(notificationCreada);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear nueva notificación: {NotificationMessage}", notificationDto?.Message ?? "null");
            throw new ExternalServiceException("Base de datos", "Error al crear la notificación", ex);
        }
    }

    // Método para validar el DTO
    private void ValidateNotification(NotificationDto notificationDto)
    {
        if (notificationDto == null)
        {
            throw new Utilities.Exceptions.ValidationException("El objeto notificación no puede ser nulo");
        }

        if (string.IsNullOrWhiteSpace(notificationDto.Message))
        {
            _logger.LogWarning("Se intentó crear/actualizar una notificación con mensaje vacío");
            throw new Utilities.Exceptions.ValidationException("Message", "El mensaje de la notificación es obligatorio");
        }
    }

    // Método para mapear de Notification a NotificationDTO
    private NotificationDto MapToDTO(Notification notification)
    {
        return new NotificationDto
        {
            Id = notification.Id,
            Message = notification.Message,
            TypeAction = notification.TypeAction,
            Read = notification.Read
        };
    }

    // Método para mapear de NotificationDTO a Notification
    private Notification MapToEntity(NotificationDto notificationDto)
    {
        return new Notification
        {
            Id = notificationDto.Id,
            Message = notificationDto.Message,
            TypeAction = notificationDto.TypeAction,
            Read = notificationDto.Read
        };
    }

    // Método para mapear una lista de Notification a una lista de NotificationDTO
    private IEnumerable<NotificationDto> MapToDTOList(IEnumerable<Notification> notifications)
    {
        var notificationsDTO = new List<NotificationDto>();
        foreach (var notification in notifications)
        {
            notificationsDTO.Add(MapToDTO(notification));
        }
        return notificationsDTO;
    }
}
