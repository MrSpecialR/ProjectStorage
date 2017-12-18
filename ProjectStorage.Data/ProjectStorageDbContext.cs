namespace ProjectStorage.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ProjectStorageDbContext : IdentityDbContext<User>
    {
        public DbSet<File> Files { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Image> Images { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<CategoryImage> CategoryImages { get; set; }
        public DbSet<UserFavouriteImages> UserFavouriteImages { get; set; }

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
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Folder>()
                .HasMany(fo => fo.Files)
                .WithOne(fi => fi.Folder)
                .HasForeignKey(fi => fi.FolderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Folder>()
                .HasOne(fo => fo.Parent)
                .WithMany(fo => fo.Subfolders)
                .HasForeignKey(fo => fo.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<File>()
                .HasOne(f => f.FileType)
                .WithMany(f => f.Files)
                .HasForeignKey(f => f.FileTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                .HasMany(u => u.Projects)
                .WithOne(p => p.Uploader)
                .HasForeignKey(p => p.UploaderId);

            builder.Entity<User>()
                .HasMany(u => u.UserImages)
                .WithOne(i => i.Uploader)
                .HasForeignKey(i => i.UploaderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserFavouriteImages>()
                .HasKey(li => new
                {
                    li.ImageId,
                    li.UserId
                });

            builder.Entity<User>()
                .HasMany(u => u.LikedImages)
                .WithOne(li => li.User)
                .HasForeignKey(li => li.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Image>()
                .HasMany(i => i.UsersLiked)
                .WithOne(li => li.Image)
                .HasForeignKey(li => li.ImageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CategoryImage>()
                .HasKey(ic => new
                {
                    ic.CategoryId,
                    ic.ImageId
                });

            builder.Entity<Category>()
                .HasMany(c => c.Images)
                .WithOne(ic => ic.Category)
                .HasForeignKey(ic => ic.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Image>()
                .HasMany(i => i.Categories)
                .WithOne(ic => ic.Image)
                .HasForeignKey(ic => ic.ImageId);

            base.OnModelCreating(builder);
        }
    }
}