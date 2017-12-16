using Microsoft.AspNetCore.Mvc;

namespace ProjectStorage.Web
{
    using AutoMapper;
    using Data;
    using Data.Models;
    using Infrastructure.Configuration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Services;
    using System.IO;

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

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ProjectStorageDbContext>()
                .AddDefaultTokenProviders();
            
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddAutoMapper(cfg => cfg.AddProfile(new SetupAutoMapper()));

            services.AddMvc(options =>
            {
                options.Filters.Add<ValidateAntiForgeryTokenAttribute>();
            });
        }
        

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, @"Uploads")),
                RequestPath = new PathString("/StaticFiles")
            });

            

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
