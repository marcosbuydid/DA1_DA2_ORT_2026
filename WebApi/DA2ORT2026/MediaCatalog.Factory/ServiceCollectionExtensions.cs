using MediaCatalog.DataAccess.InMemoryRepositories;
using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Services;
using MediaCatalog.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MediaCatalog.Factory
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(IServiceCollection serviceCollection)
        {
           serviceCollection.AddScoped<ISecureDataService, SecureDataService>();
           serviceCollection.AddScoped<IRoleService, RoleService>();
           serviceCollection.AddScoped<IUserService, UserService>();
           serviceCollection.AddScoped<IMovieService, MovieService>();
        }

        public static void AddDataAccess(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IRoleRepository, InMemoryRoleRepository>();
            serviceCollection.AddSingleton<IUserRepository, InMemoryUserRepository>();
            serviceCollection.AddSingleton<IMovieRepository, InMemoryMovieRepository>();
        }
    }
}
