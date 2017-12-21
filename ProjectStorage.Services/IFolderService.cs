namespace ProjectStorage.Services
{
    using Models;

    public interface IFolderService
    {
        FolderInformationServiceModel GetFolder(string id);
    }
}