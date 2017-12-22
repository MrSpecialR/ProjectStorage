using ProjectStorage.Services.Implementations;

namespace ProjectStorage.Tests
{
    using AutoMapper;
    using Infrastructure.Configuration;

    public class Configuration
    {
        public static bool IsConfigured = false;

        public static void Configure()
        {
            if (!IsConfigured)
            {
                var category = typeof(CategoryService);
                var tempp = typeof(IMappableFrom<>);
                Mapper.Initialize(cfg => cfg.AddProfile<SetupAutoMapper>());
                IsConfigured = true;
            }
        }

    }
}