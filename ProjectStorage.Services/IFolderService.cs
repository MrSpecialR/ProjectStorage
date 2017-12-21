namespace ProjectStorage.Services
{
    using Models;

    public interface IFolderService
    {
        FolderInformationServiceModel GetFolder(string id);
        byte[] ZipFolder(string id);
    }
}