namespace ProjectStorage.Services.Models.Project
{
    using AutoMapper;
    using ProjectStorage.Infrastructure.Configuration;
    using System;

    public class ProjectListingModel : ICustomMapConfiguration
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UploaderName { get; set; }

        public DateTime UploadDate { get; set; }

        public void ConfigureMap(Profile profile)
        {
            profile.CreateMap<Data.Models.Project, ProjectListingModel>()
                .ForMember(plm => plm.UploaderName, cfg => cfg.MapFrom(p => p.Uploader.UserName));
        }
    }
}