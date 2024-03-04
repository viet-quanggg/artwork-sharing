using ArtworkSharing.Core.Exceptions;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Services;
using ArtworkSharing.DAL;
using ArtworkSharing.DAL.Data;
using ArtworkSharing.Service.Services;
using Microsoft.Identity.Client;

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
            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<IArtistPackageService, ArtistPackageService>();
            services.AddScoped<IRefundRequestService, RefundRequestService>();
            services.AddScoped<IArtworkRequestService, ArtworkRequestService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UnitOfWork>();
            return services;
        }


        public static void AddConfigException(this IServiceCollection services)
        {
            services.AddControllers(_ =>
            {
                _.Filters.Add(new BusinessException());
            });
        }

        public static void UseException(this IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
