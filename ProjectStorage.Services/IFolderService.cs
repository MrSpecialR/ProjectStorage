namespace ProjectStorage.Services
{
    using Models;

    public interface IFolderService
    {
        FolderInformationServiceModel GetFolder(string id);

        byte[] ZipFolder(string id);

        void Delete(string folderId);

        bool IsOwner(string userId, string folderId);
    }
}