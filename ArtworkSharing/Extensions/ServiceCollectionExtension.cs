using ArtworkSharing.Service.AutoMappings;
using AutoMapper;

namespace ArtworkSharing.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            var config = new MapperConfiguration(AutoMapperConfiguration.RegisterMaps);
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
    }
}
