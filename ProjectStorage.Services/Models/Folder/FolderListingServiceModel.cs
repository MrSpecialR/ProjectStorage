namespace ProjectStorage.Services.Models.Folder
{
    using Data.Models;
    using Infrastructure.Configuration;

    public class FolderListingServiceModel : IMappableFrom<Folder>
    {
        public string Id { get; set; }

        public string FolderName { get; set; }
    }
}