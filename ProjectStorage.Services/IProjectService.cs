namespace ProjectStorage.Services
{
    using Microsoft.AspNetCore.Http;

    public interface IProjectService
    {
        void Add(string userId, IFormFile file);
    }
}