﻿namespace ProjectStorage.Services.Implementations
{
    using Data;

    public class ImageService : IImageService
    {
        private readonly ProjectStorageDbContext db;

        public ImageService(ProjectStorageDbContext db)
        {
            this.db = db;
        }
    }
}