namespace ProjectStorage.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Models;
    using System.Linq;

    public class FileService : IFileService
    {
        private readonly ProjectStorageDbContext db;

        public FileService(ProjectStorageDbContext db)
        {
            this.db = db;
        }

        public FileServiceModel GetFileById(string id)
        {
            var file = this.db.Files.Where(f => f.Id.ToString() == id).ProjectTo<FileServiceModel>().FirstOrDefault();
            if (file != null)
            {
                file.Content = System.IO.File.ReadAllBytes(file.Path);
            }

            return file;
        }

        public void Delete(string id)
        {
            var file = this.db.Files.FirstOrDefault(f => f.Id.ToString() == id);
            if (file == null)
            {
                return;
            }

            this.db.Remove(file);
            this.db.SaveChanges();
        }

        public bool IsOwner(string userId, string fileId)
        {
            var uploader = this.db.Files.Where(f => f.Id.ToString() == fileId).Select(f => f.Project.UploaderId).FirstOrDefault();
            return uploader == userId;
        }
    }
}