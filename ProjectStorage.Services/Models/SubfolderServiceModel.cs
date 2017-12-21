namespace ProjectStorage.Services.Models
{
    using AutoMapper;
    using Data.Models;
    using Infrastructure.Configuration;

    public class SubfolderServiceModel : ICustomMapConfiguration
    {
        public string Id { get; set; }

        public string FolderName { get; set; }

        public void ConfigureMap(Profile profile)
        {
            profile.CreateMap<Folder, SubfolderServiceModel>()
                .ForMember(c => c.Id, cfg => cfg.MapFrom(im => im.Id.ToString()));
        }
    }
}