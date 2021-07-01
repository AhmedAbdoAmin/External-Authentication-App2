using External_Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace External_Authentication
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlDataContractSerializerFormatters();

            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_config.GetConnectionString("ExternalAuthDbConntection")));

            services.AddIdentity<ApplicationUser, ManagerUserRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication()
               .AddGoogle(Options =>
               {
                   Options.ClientId = "432692197743-va8guk0qchtd3o2uk989vb9els5mbvih.apps.googleusercontent.com";
                   Options.ClientSecret = "AHf9U-1QOa7rqrKXeoKvXUhu";
               })
               .AddFacebook(options =>
               {
                   options.ClientId = "1163267724191538";
                   options.ClientSecret = "24eb2795c622250f5f0a310bd1f5424e";
               });
            services.AddScoped<IPermissionStoreRepository<Permission>, PermissionRepository>();
            services.AddScoped<IPermissionStoreRepository<RolesHasPermission>, RolesHasPermissionRepository>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireClaim("Delete Role", "true")
                    );

                options.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireRole("Admin")
                  );

            });
            ////Access Denied
            services.ConfigureApplicationCookie(options =>
               options.AccessDeniedPath = new PathString("/Administration/AccessDenied")
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
