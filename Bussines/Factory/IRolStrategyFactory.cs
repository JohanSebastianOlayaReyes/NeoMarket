using Business.Strategies.Interfaces;

namespace Business.Factory
{
    /// <summary>
    /// Interfaz que define una fábrica para crear instancias de estrategias de roles.
    /// </summary>
    public interface IRolStrategyFactory
    {
        /// <summary>
        /// Crea la estrategia estándar para operaciones con roles.
        /// </summary>
        /// <returns>Instancia de la estrategia estándar.</returns>
        IRolStrategy CreateStrategy();
    }
}