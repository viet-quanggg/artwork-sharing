using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.DAL;
using ArtworkSharing.DAL.Data;
using ArtworkSharing.Service.Services;

namespace ArtworkSharing.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Add needed instances for database
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddScoped<Func<ArtworkSharingContext>>((provider) => () => provider.GetService<ArtworkSharingContext>());
            services.AddScoped<DbFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        /// <summary>
        /// Add instances of in-use services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IArtworkService, ArtworkService>();
            return services;
        }
    }
}
