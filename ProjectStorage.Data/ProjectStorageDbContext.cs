namespace ProjectStorage.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ProjectStorageDbContext : IdentityDbContext<User>
    {
        public DbSet<File> Files { get; set; }
        public DbSet<Folder> Folders{ get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<FileType> FileTypes { get; set; }


        public ProjectStorageDbContext(DbContextOptions<ProjectStorageDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<File>()
                .HasOne(f => f.Project)
                .WithMany(p => p.Files)
                .HasForeignKey(f => f.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Folder>()
                .HasMany(fo => fo.Files)
                .WithOne(fi => fi.Folder)
                .HasForeignKey(fi => fi.FolderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Folder>()
                .HasOne(fo => fo.Parent)
                .WithMany(fo => fo.Subfolders)
                .HasForeignKey(fo => fo.ParentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<File>()
                .HasOne(f => f.FileType)
                .WithMany(f => f.Files)
                .HasForeignKey(f => f.FileTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
