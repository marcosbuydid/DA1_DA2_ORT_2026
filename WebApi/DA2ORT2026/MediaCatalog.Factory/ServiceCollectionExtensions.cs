using MediaCatalog.DataAccess.InMemoryRepositories;
using MediaCatalog.DataAccess.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MediaCatalog.Factory
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(IServiceCollection serviceCollection)
        {
        }

        public static void AddDataAccess(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IRoleRepository, InMemoryRoleRepository>();
            serviceCollection.AddSingleton<IUserRepository, InMemoryUserRepository>();
            serviceCollection.AddSingleton<IMovieRepository, InMemoryMovieRepository>();
        }
    }
}
