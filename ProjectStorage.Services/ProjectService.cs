namespace ProjectStorage.Services
{
    using Data;

    public class ProjectService : IProjectService
    {
        private readonly ProjectStorageDbContext db;

        public ProjectService(ProjectStorageDbContext db)
        {
            this.db = db;
        }


        public void Add(string userId, byte[] file)
        {
            
        }
    }
}