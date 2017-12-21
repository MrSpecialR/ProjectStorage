namespace ProjectStorage.Services
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;

    public static class Zipper
    {
        public static void ProcessDirectory(string targetDirectory, ZipArchive archive)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                ProcessFile(fileName, archive);
            }

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                ProcessDirectory(subdirectory, archive);
            }
        }

        private static void ProcessFile(string path, ZipArchive archive)
        {
            var filePath = String.Join("/", path.Split(new[] { "~/../../Uploads/Projects//" }, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault().Replace('\\', '/').Split('/').Skip(1));
            archive.CreateEntryFromFile(path, filePath);
        }
    }
}