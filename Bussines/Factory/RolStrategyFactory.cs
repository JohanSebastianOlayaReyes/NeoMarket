using AutoMapper;
using Business.Strategies.Impl;
using Business.Strategies.Interfaces;
using Data.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Business.Factory
{
    /// <summary>
    /// Implementación de la fábrica para crear instancias de estrategias de roles.
    /// </summary>
    public class RolStrategyFactory : IRolStrategyFactory
    {
        private readonly IRolRepository _rolRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Constructor de la fábrica de estrategias.
        /// </summary>
        /// <param name="rolRepository">Repositorio de roles.</param>
        /// <param name="mapper">Mapper para conversión entre entidades y DTOs.</param>
        /// <param name="loggerFactory">Fábrica de loggers.</param>
        public RolStrategyFactory(
            IRolRepository rolRepository,
            IMapper mapper,
            ILoggerFactory loggerFactory)
        {
            _rolRepository = rolRepository ?? throw new ArgumentNullException(nameof(rolRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <inheritdoc />
        public IRolStrategy CreateStrategy()
        {
            var logger = _loggerFactory.CreateLogger<StandardRolStrategy>();
            return new StandardRolStrategy(_rolRepository, _mapper, logger);
        }
    }
}