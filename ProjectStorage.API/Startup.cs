namespace ProjectStorage.API
{
    using AutoMapper;
    using Data;
    using Infrastructure.Configuration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Services;
    using Services.Implementations;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ProjectStorageDbContext>(options =>
                options.UseSqlServer(ConnectionStrings.DefaultConnection));

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IFolderService, FolderService>();
            services.AddAutoMapper(cfg => cfg.AddProfile(new SetupAutoMapper()));

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}