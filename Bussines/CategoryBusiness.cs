﻿using Data;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business;

/// <summary>
/// Clase de negocio encargada de la lógica relacionada con las categorías del sistema.
/// </summary>
public class CategoryBusiness
{
    private readonly CategoryData _categoryData;
    private readonly ILogger _logger;

    public CategoryBusiness(CategoryData categoryData, ILogger logger)
    {
        _categoryData = categoryData;
        _logger = logger;
    }

    // Método para obtener todas las categorías como DTOs
    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        try
        {
            var categories = await _categoryData.GetAllAsync();
            return MapToDTOList(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las categorías");
            throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de categorías", ex);
        }
    }

    // Método para obtener una categoría por ID como DTO
    public async Task<CategoryDto> GetCategoryByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Se intentó obtener una categoría con ID inválido: {CategoryId}", id);
            throw new Utilities.Exceptions.ValidationException("id", "El ID de la categoría debe ser mayor que cero");
        }

        try
        {
            var category = await _categoryData.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogInformation("No se encontró ninguna categoría con ID: {CategoryId}", id);
                throw new EntityNotFoundException("Category", id);
            }

            return MapToDTO(category);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la categoría con ID: {CategoryId}", id);
            throw new ExternalServiceException("Base de datos", $"Error al recuperar la categoría con ID {id}", ex);
        }
    }

    // Método para crear una categoría desde un DTO
    public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
    {
        try
        {
            ValidateCategory(categoryDto);

            var category = MapToEntity(categoryDto);

            var categoryCreated = await _categoryData.CreateAsync(category);

            return MapToDTO(categoryCreated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear nueva categoría: {CategoryName}", categoryDto?.NameCategory ?? "null");
            throw new ExternalServiceException("Base de datos", "Error al crear la categoría", ex);
        }
    }


    // Método para validar el DTO
    private void ValidateCategory(CategoryDto categoryDto)
    {
        if (categoryDto == null)
        {
            throw new Utilities.Exceptions.ValidationException("El objeto categoría no puede ser nulo");
        }

        if (string.IsNullOrWhiteSpace(categoryDto.NameCategory))
        {
            _logger.LogWarning("Se intentó crear/actualizar una categoría con Name vacío");
            throw new Utilities.Exceptions.ValidationException("Name", "El Name de la categoría es obligatorio");
        }
    }

    // Método para mapear de category a CategoryDto
    private CategoryDto MapToDTO(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            NameCategory = category.NameCategory,
            Description = category.Description,
        };
    }

    // Método para mapear de CategoryDto a category
    private Category MapToEntity(CategoryDto categoryDto)
    {
        return new Category
        {
            Id = categoryDto.Id,
            NameCategory = categoryDto.NameCategory,
            Description = categoryDto.Description,
        };
    }

    // Método para mapear una lista de Category a una lista de CategoryDto
    private IEnumerable<CategoryDto> MapToDTOList(IEnumerable<Category> categories)
    {
        var CategoryDto = new List<CategoryDto>();
        foreach (var category in categories)
        {
            CategoryDto.Add(MapToDTO(category));
        }
        return CategoryDto;
    }
}
