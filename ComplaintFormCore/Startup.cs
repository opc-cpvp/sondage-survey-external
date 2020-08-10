using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ComplaintFormCore.Resources;
using GoC.WebTemplate.Components.Core.Services;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft;

namespace ComplaintFormCore
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

            //  This is for error handling
            services.AddProblemDetails(options =>
            {
                //// configure here
                //options.Map<OutOfCreditException>(exception => new OutOfCreditProblemDetails
                //{
                //    Title = exception.Message,
                //    Detail = exception.Description,
                //    Balance = exception.Balance,
                //    Status = StatusCodes.Status403Forbidden,
                //    Type = exception.Type
                //});

            });
            //services.AddTransient<ProblemDetailsFactory, OPCProblemDetailsFactory>();

            //  https://visualstudiomagazine.com/articles/2018/12/01/working-with-session.aspx
            services.AddSession(so =>
            {
                so.IdleTimeout = TimeSpan.FromSeconds(60);
            });

            services.AddControllersWithViews();
            services.AddMvc();

            services.AddLocalization(o => o.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("fr"),
                };
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddSingleton<SharedViewLocalizer>();

            services.AddModelAccessor();
            services.ConfigureGoCTemplateRequestLocalization();

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //  This is for error handling
            app.UseProblemDetails();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            app.UseHttpsRedirection();

           // app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "culture-route", pattern: "{culture=en}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",                    
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

          
        }
    }
}
