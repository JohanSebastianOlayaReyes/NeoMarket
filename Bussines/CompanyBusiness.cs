﻿using Data;
using Entity.DTO;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Business;

/// <summary>
/// Clase de negocio encargada de la lógica relacionada con las compañías del sistema.
/// </summary>
public class CompanyBusiness
{
    private readonly CompanyData _companyData;
    private readonly ILogger _logger;

    public CompanyBusiness(CompanyData companyData, ILogger logger)
    {
        _companyData = companyData;
        _logger = logger;
    }

    // Método para obtener todas las compañías como DTOs
    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync()
    {
        try
        {
            var companies = await _companyData.GetAllAsync();
            return MapToDTOList(companies);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las compañías");
            throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de compañías", ex);
        }
    }

    // Método para obtener una compañía por ID como DTO
    public async Task<CompanyDto> GetCompanyByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Se intentó obtener una compañía con ID inválido: {CompanyId}", id);
            throw new Utilities.Exceptions.ValidationException("id", "El ID de la compañía debe ser mayor que cero");
        }

        try
        {
            var company = await _companyData.GetByIdAsync(id);
            if (company == null)
            {
                _logger.LogInformation("No se encontró ninguna compañía con ID: {CompanyId}", id);
                throw new EntityNotFoundException("Company", id);
            }

            return MapToDTO(company);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la compañía con ID: {CompanyId}", id);
            throw new ExternalServiceException("Base de datos", $"Error al recuperar la compañía con ID {id}", ex);
        }
    }

    // Método para crear una compañía desde un DTO
    public async Task<CompanyDto> CreateCompanyAsync(CompanyDto companyDto)
    {
        try
        {
            ValidateCompany(companyDto);

            var company = MapToEntity(companyDto);
            var createdCompany = await _companyData.CreateAsync(company);

            return MapToDTO(createdCompany);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear nueva compañía: {CompanyName}", companyDto?.NameCompany ?? "null");
            throw new ExternalServiceException("Base de datos", "Error al crear la compañía", ex);
        }
    }

    // Método para validar el DTO
    private void ValidateCompany(CompanyDto companyDto)
    {
        if (companyDto == null)
        {
            throw new Utilities.Exceptions.ValidationException("El objeto compañía no puede ser nulo");
        }

        if (string.IsNullOrWhiteSpace(companyDto.NameCompany))
        {
            _logger.LogWarning("Se intentó crear/actualizar una compañía con Name vacío");
            throw new Utilities.Exceptions.ValidationException("Name", "El Name de la compañía es obligatorio");
        }
    }

    // Método para mapear de Company a CompanyDto
    private CompanyDto MapToDTO(Company company)
    {
        return new CompanyDto
        {
            Id = company.Id,
            NameCompany = company.NameCompany,
            Description = company.Description,
            Status = company.Status,
            PhoneCompany = company.PhoneCompany,
            Logo = company.Logo,
            EmailCompany = company.EmailCompany,
            NitCompany = company.NitCompany
        };
    }

    // Método para mapear de CompanyDto a Company
    private Company MapToEntity(CompanyDto companyDto)
    {
        return new Company
        {
            Id = companyDto.Id,
            NameCompany = companyDto.NameCompany,
            Description = companyDto.Description,
            Status = companyDto.Status,
            PhoneCompany = companyDto.PhoneCompany,
            Logo = companyDto.Logo,
            EmailCompany = companyDto.EmailCompany,
            NitCompany = companyDto.NitCompany
        };
    }

    // Método para mapear una lista de Company a una lista de CompanyDto
    private IEnumerable<CompanyDto> MapToDTOList(IEnumerable<Company> companies)
    {
        var companiesDTO = new List<CompanyDto>();
        foreach (var company in companies)
        {
            companiesDTO.Add(MapToDTO(company));
        }
        return companiesDTO;
    }
}

