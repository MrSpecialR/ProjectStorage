namespace ProjectStorage.Services
{
    using Models;

    public interface IFileService
    {
        FileServiceModel GetFileById(string id);

        void Delete(string id);

        bool IsOwner(string userId, string folderId);
    }
}