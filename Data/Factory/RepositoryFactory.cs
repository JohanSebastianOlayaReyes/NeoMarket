using Data.Repository.Impl;
using Data.Repository.Interfaces;
using Entity.Context;
using Microsoft.Extensions.Logging;
using System;

namespace Data.Factory
{
    /// <summary>
    /// Implementación de la fábrica de repositorios.
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Constructor de la fábrica de repositorios.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="loggerFactory">Fábrica de loggers.</param>
        public RepositoryFactory(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <inheritdoc />
        public IGenericRepository<T> CreateRepository<T>() where T : class
        {
            var logger = _loggerFactory.CreateLogger<GenericRepository<T>>();
            return new GenericRepository<T>(_context, logger);
        }

        /// <inheritdoc />
        public IRolRepository CreateRolRepository()
        {
            var logger = _loggerFactory.CreateLogger<RolRepository>();
            return new RolRepository(_context, logger);
        }

        // Implementación de los demás métodos para crear repositorios específicos
        /*
        public IFormRepository CreateFormRepository()
        {
            var logger = _loggerFactory.CreateLogger<FormRepository>();
            return new FormRepository(_context, logger);
        }

        public IModuleRepository CreateModuleRepository()
        {
            var logger = _loggerFactory.CreateLogger<ModuleRepository>();
            return new ModuleRepository(_context, logger);
        }

        public IPersonRepository CreatePersonRepository()
        {
            var logger = _loggerFactory.CreateLogger<PersonRepository>();
            return new PersonRepository(_context, logger);
        }

        public IUserRepository CreateUserRepository()
        {
            var logger = _loggerFactory.CreateLogger<UserRepository>();
            return new UserRepository(_context, logger);
        }

        public IRolFormRepository CreateRolFormRepository()
        {
            var logger = _loggerFactory.CreateLogger<RolFormRepository>();
            return new RolFormRepository(_context, logger);
        }
        */
    }
}