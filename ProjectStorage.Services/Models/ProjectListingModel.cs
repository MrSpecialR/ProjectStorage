namespace ProjectStorage.Services.Models
{
    using AutoMapper;
    using Data.Models;
    using Infrastructure.Configuration;
    using System;

    public class ProjectListingModel : ICustomMapConfiguration
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UploaderName { get; set; }

        public DateTime UploadDate { get; set; }

        public void ConfigureMap(Profile profile)
        {
            profile.CreateMap<Project, ProjectListingModel>()
                .ForMember(plm => plm.UploaderName, cfg => cfg.MapFrom(p => p.Uploader.UserName));
        }
    }
}