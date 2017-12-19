namespace ProjectStorage.Services.Models
{
    using AutoMapper;
    using Data.Models;
    using Infrastructure.Configuration;

    public class ImageListingServiceModel : ICustomMapConfiguration
    {
        public string Id { get; set; }

        public string Path { get; set; }

        public string Title { get; set; }

        public void ConfigureMap(Profile profile)
        {
            profile.CreateMap<Image, ImageListingServiceModel>()
                .ForMember(c => c.Id, cfg => cfg.MapFrom(im => im.Id.ToString()));
        }
    }
}
