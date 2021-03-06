using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationWorkerDataLayer.Interfaces;
using ApplicationWorkerDataLayer.Repositories;
using Common.DTOs.Configurations.ApplicationWorker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ApplicationWorker
{
    public class Startup
    {
        private string _corsList = string.Empty;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ApplicationProcessingConfig();
            Configuration.Bind("ApplicationProcessing", config);      //  Bind "ApplicationProcessingConfig" Object to "ApplicationProcessing" config section

            // save the list of CORS sites to property.. 
            // easy to do it now when we have access to the config object (TrustScienceConfiguration)
            _corsList = config.CorsList;

            services.AddSingleton(config);

            services.AddTransient<IApplicationRepository, ApplicationRepository>();     //add TrustScienceService to services collection


            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Note:
            // converting list Cors list to array enable list of Cors to work
            var origin = this._corsList;
            string[] sites = origin.Split(',');

            // remove empty sites.. Otherwise it will failed
            sites = sites.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            // remove all '/' From the end of the URL if exists
            sites = sites.Select(x => FormatCorsList(x)).ToArray();

            app.UseCors(builder =>
            {
                builder
                .WithOrigins(sites)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        // make sure the list does not ends with "/"
        private string FormatCorsList(string corsList)
        {
            corsList = corsList.Trim();
            if (corsList.EndsWith('/'))
            {
                corsList = corsList.Remove(corsList.Length - 1, 1);
            }
            return corsList;
        }
    }
}
