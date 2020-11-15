using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using StaticCacheHeader.Bsl.Articles;
using StaticCacheHeader.Bsl.Articles.Cached;
using StaticCacheHeader.Bsl.Caching;
using StaticCacheHeader.Bsl.Utils;

namespace StaticCacheHeader
{
    public class Startup
    {
        [Obsolete]
        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment evm)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(evm.ContentRootPath)
              .AddJsonFile("appsettings.json", true, true)
              .AddJsonFile($"appsettings.{evm.EnvironmentName}.json", true)
              .AddEnvironmentVariables();

            Configuration = builder.Build();

            AppSettings.Instance.SetConfiguration(Configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddScoped<ICacheBase, CacheMemoryHelper>();
            services.AddScoped<IArticlesBo, ArticlesBo>();
            services.AddScoped<IArticleCached, ArticleCached>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseCookiePolicy();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    // add time exprired header
                    const int durationInSeconds = (60 * 60 * 24) * 365; // Expired time. I set to 1 year
                    context.Context.Response.Headers["Cache-Control"] =
                        "public,max-age=" + durationInSeconds;
                }
            });
            
            app.UseRouting();   

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
