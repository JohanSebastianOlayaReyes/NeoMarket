using Business;
using Business.Factory;
using Business.Strategies.Impl;
using Business.Strategies.Interfaces;
using Data;
using Data.Factory;
using Data.Repository.Impl;
using Data.Repository.Interfaces;
using Entity.Context;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using Web.Infrastructure.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuraci�n de AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Registro de repositorios gen�ricos
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Registro de repositorios espec�ficos
builder.Services.AddScoped<IRolRepository, RolRepository>();
// Agregar aqu� m�s repositorios para otras entidades cuando los implementes
// builder.Services.AddScoped<IFormRepository, FormRepository>();
// builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
// etc.

// Registro de factory para repositorios
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();

// Registro de estrategias
builder.Services.AddScoped<IRolStrategy, StandardRolStrategy>();

// Registro de factory para estrategias
builder.Services.AddScoped<IRolStrategyFactory, RolStrategyFactory>();

// Registrar clases de Buyout
builder.Services.AddScoped<BuyoutData>();
builder.Services.AddScoped<BuyoutBusiness>();

// Registrar clases de Category
builder.Services.AddScoped<CategoryData>();
builder.Services.AddScoped<CategoryBusiness>();

// Registrar clases de Company
builder.Services.AddScoped<CompanyData>();
builder.Services.AddScoped<CompanyBusiness>();

// Registrar clases de Form
builder.Services.AddScoped<FormData>();
builder.Services.AddScoped<FormBusiness>();

// Registrar clases de ImageItem
builder.Services.AddScoped<ImageItemData>();
builder.Services.AddScoped<ImageItemBusiness>();

// Registrar clases de Inventory
builder.Services.AddScoped<InventoryData>();
builder.Services.AddScoped<InventoryBusiness>();

// Registrar clases de Module
builder.Services.AddScoped<ModuleData>();
builder.Services.AddScoped<ModuleBusiness>();

// Registrar clases de MovimientInventory
builder.Services.AddScoped<MovimientInventoryData>();
builder.Services.AddScoped<MovimientInventoryBusiness>();

// Registrar clases de Notification
builder.Services.AddScoped<NotificationData>();
builder.Services.AddScoped<NotificationBusiness>();

// Registrar clases de Person
builder.Services.AddScoped<PersonData>();
builder.Services.AddScoped<PersonBusiness>();

// Registrar clases de Product
builder.Services.AddScoped<ProductData>();
builder.Services.AddScoped<ProductBusiness>();

// Nueva versi�n de RolBusiness que usa el patr�n Strategy
builder.Services.AddScoped<RolBusiness>();

// Registrar clases de RolForm
builder.Services.AddScoped<RolFormData>();
builder.Services.AddScoped<RolFormBusiness>();

// Registrar clases de Sede
builder.Services.AddScoped<SedeData>();
builder.Services.AddScoped<SedeBusiness>();

// Registrar clases de Sele
builder.Services.AddScoped<SaleData>();
builder.Services.AddScoped<SaleBusiness>();

// Registrar clases de SeleDetail
builder.Services.AddScoped<SeleDetailData>();
builder.Services.AddScoped<SaleDetailBusiness>();

// Registrar clases de User
builder.Services.AddScoped<UserData>();
builder.Services.AddScoped<UserBusiness>();

// Agregar CORS 
// mecanismo de seguridad que permite o restringe las solicitudes de recursos que se originan desde un dominio diferente al del servidor.
var OrigenesPermitidos = builder.Configuration.GetValue<string>("OrigenesPermitidos")!.Split(",");
builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(politica =>
    {
        politica.WithOrigins(OrigenesPermitidos).AllowAnyHeader().AllowAnyMethod();
    });
});

// Agregar DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("MySqlConnection"))
);

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();