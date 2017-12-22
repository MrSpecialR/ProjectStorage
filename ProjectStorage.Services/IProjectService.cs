namespace ProjectStorage.Services
{
    using Microsoft.AspNetCore.Http;
    using Models;
    using System.Collections.Generic;

    public interface IProjectService
    {
        FolderInformationServiceModel GetProject(int id);

        int Add(string userId, IFormFile file, string fileTitle);

        void Delete(int projectId);

        IEnumerable<ProjectListingModel> GetAllProjects();

        byte[] ZipProject(int id);

        bool UserIsOwner(int id, string userID);
    }
}