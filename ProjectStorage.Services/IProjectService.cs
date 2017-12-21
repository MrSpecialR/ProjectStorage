namespace ProjectStorage.Services
{
    using Microsoft.AspNetCore.Http;
    using Models;
    using System.Collections.Generic;

    public interface IProjectService
    {
        FolderInformationServiceModel GetProject(int id);

        void Add(string userId, IFormFile file);

        void Delete(int projectId);
        IEnumerable<ProjectListingModel> GetAllProjects();

        byte[] ZipProject(int id);
    }
}