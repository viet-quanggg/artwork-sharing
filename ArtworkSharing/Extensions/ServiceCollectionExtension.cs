using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.DAL;
using ArtworkSharing.DAL.Data;

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
            // Configure DbContext with Scoped lifetime   
            //services.AddDbContext<ArtworkSharingContext>(options =>
            //{
            //    options.UseSqlServer(AppSettings.ConnectionString,
            //        sqlOptions => sqlOptions.CommandTimeout(120));
            //    options.UseLazyLoadingProxies();
            //}
            //AddDbContext<ArtworkSharingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            //);

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
        //public static IServiceCollection AddServices(this IServiceCollection services)
        //{
        //    return services.AddScoped<IWorkService, WorkService>();
        //}
    }
}
