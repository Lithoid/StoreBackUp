using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Context;
using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repositories;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDataContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DataConnection"),
                    optionsBuilder => optionsBuilder.MigrationsAssembly("WebApp")));

            services.AddDbContext<AppIdentityContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("IdentityConnection"),
                    optionsBuilder => optionsBuilder.MigrationsAssembly("WebApp")));

            services.AddTransient<DbContext, AppDataContext>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IAssetRepository, AssetRepository>();
            services.AddTransient<ICartItemRepository, CartItemRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddSession();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<AppUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequiredLength = 5;
                })
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<AppIdentityContext>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            
            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Shop}/{action=List}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}