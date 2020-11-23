using ApplicationProcessing.Service.ScoringSolution.DTOs.Configuration;
using ApplicationProcessing.Service.ScoringSolution.Repositories;
using ApplicationProcessing.Service.ScoringSolution.Services;
using ApplicationWorker.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace ApplicationProcessing.Service.ScoringSolution
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
            var config = new ScoringSolutionConfiguration();
            Configuration.Bind("ScoringSolution", config);      //  Bind "ScoringSolutionConfiguration" Object to "ScoringSolution" config section

            // save the list of CORS sites to property.. 
            // easy to do it now when we have access to the config object (TrustScienceConfiguration)
            _corsList = config.CorsList;

            services.AddSingleton(config);      // add config to the injection manager

            services.AddTransient<IScoringSolutionRepository, ScoringSolutionRepository>();     // add ScoringSolutionRepository to the injection manager

            services.AddTransient<IScoringSolutionService, ApplicationProcessing.Service.ScoringSolution.Services.ScoringSolutionService>();

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
