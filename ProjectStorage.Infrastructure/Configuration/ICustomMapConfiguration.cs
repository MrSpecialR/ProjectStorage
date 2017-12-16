namespace ProjectStorage.Infrastructure.Configuration
{
    using AutoMapper;

    public interface ICustomMapConfiguration
    {
        void ConfigureMap(Profile profile);
    }
}