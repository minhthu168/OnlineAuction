using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eProject.Repository;
using eProject.Service;
namespace eProject
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
            services.AddDbContext<Data.DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnectDB")));
            services.Configure<Models.MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddSession();
            services.AddTransient<ICategoryServices,CategoryServices>();
            services.AddTransient<IAuctionServices,AuctionServices>();
            services.AddTransient<IAuctionBid, AuctionBidServices>();
            services.AddTransient<IWinner, WinnerServices>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<INews, NewsServices>();
            services.AddTransient<IMessageServices, MessageServices>();

            services.AddControllersWithViews();
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
            app.UseSession();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
          );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=PageHome}/{action=Index}/{id?}");
            });
        }
    }
}
