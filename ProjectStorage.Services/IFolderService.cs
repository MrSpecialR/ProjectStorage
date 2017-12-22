using ProjectStorage.Services.Models.Folder;

namespace ProjectStorage.Services
{
    using Models;
    using System.Collections.Generic;

    public interface IFolderService
    {

        IEnumerable<FolderListingServiceModel> GetFolders();
        FolderInformationServiceModel GetFolder(string id);

        byte[] ZipFolder(string id);

        void Delete(string folderId);

        bool IsOwner(string userId, string folderId);
    }
}