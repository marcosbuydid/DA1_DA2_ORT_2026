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
           serviceCollection.AddScoped<ITokenService, JWTService>();
           serviceCollection.AddScoped<ISessionService, SessionService>();
        }

        public static void AddDataAccess(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IRoleRepository, InMemoryRoleRepository>();
            serviceCollection.AddSingleton<IUserRepository, InMemoryUserRepository>();
            serviceCollection.AddSingleton<IMovieRepository, InMemoryMovieRepository>();
            serviceCollection.AddSingleton<ISessionRepository, InMemorySessionRepository>();
        }
    }
}
