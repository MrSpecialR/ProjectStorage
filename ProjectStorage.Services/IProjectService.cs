namespace ProjectStorage.Services
{
    public interface IProjectService
    {
        void Add(string userId, byte[] file);
    }
}