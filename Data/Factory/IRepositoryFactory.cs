using Data.Repository.Interfaces;

namespace Data.Factory
{
    /// <summary>
    /// Interfaz que define una fábrica para crear instancias de repositorios.
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Crea un repositorio genérico para el tipo especificado.
        /// </summary>
        /// <typeparam name="T">Tipo de entidad para el repositorio.</typeparam>
        /// <returns>Instancia del repositorio genérico.</returns>
        IGenericRepository<T> CreateRepository<T>() where T : class;

        /// <summary>
        /// Crea un repositorio de roles.
        /// </summary>
        /// <returns>Instancia del repositorio de roles.</returns>
        IRolRepository CreateRolRepository();

        // Métodos adicionales para otros repositorios específicos
        // IFormRepository CreateFormRepository();
        // IModuleRepository CreateModuleRepository();
        // IPersonRepository CreatePersonRepository();
        // IUserRepository CreateUserRepository();
        // IRolFormRepository CreateRolFormRepository();
    }
}