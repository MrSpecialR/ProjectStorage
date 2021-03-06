﻿namespace ProjectStorage.Services.Models.Image
{
    using AutoMapper;
    using Infrastructure.Configuration;
    using System;
    using Data.Models;

    public class ImageListingServiceModel : ICustomMapConfiguration
    {
        public string Id { get; set; }

        public string Path { get; set; }

        public int CategoryId { get; set; }

        public string Title { get; set; }
        public string UploaderId { get; set; }
        public string UploaderName { get; set; }
        public DateTime UploadDate { get; set; }

        public void ConfigureMap(Profile profile)
        {
            profile.CreateMap<Image, ImageListingServiceModel>()
                .ForMember(c => c.Id, cfg => cfg.MapFrom(im => im.Id.ToString()))
                .ForMember(c => c.UploaderName, cfg => cfg.MapFrom(im => im.Uploader.UserName));
        }
    }
}