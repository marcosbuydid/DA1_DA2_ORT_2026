using MediaCatalog.DataAccess.EFRepositories;
using MediaCatalog.DataAccess.InMemoryRepositories;
using MediaCatalog.Services.Interfaces.Repositories;
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
            /* InMemory Repositories */
            serviceCollection.AddSingleton<IRoleRepository, InMemoryRoleRepository>();
            serviceCollection.AddSingleton<IUserRepository, InMemoryUserRepository>();
            serviceCollection.AddSingleton<IMovieRepository, InMemoryMovieRepository>();
            serviceCollection.AddSingleton<ISessionRepository, InMemorySessionRepository>();

            /* Entity Framework Repositories */
            //serviceCollection.AddScoped<IMovieRepository, EFMovieRepository>();
            //serviceCollection.AddScoped<IRoleRepository, EFRoleRepository>();
            //serviceCollection.AddScoped<ISessionRepository, EFSessionRepository>();
            //serviceCollection.AddScoped<IUserRepository, EFUserRepository>();
        }
    }
}
