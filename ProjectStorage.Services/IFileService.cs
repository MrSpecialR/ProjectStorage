namespace ProjectStorage.Services
{
    using Models.File;
    using System.Collections.Generic;

    public interface IFileService
    {
        FileServiceModel GetFileById(string id);

        void Delete(string id);

        bool IsOwner(string userId, string folderId);
        IEnumerable<FileServiceModel> GetFiles();
    }
}