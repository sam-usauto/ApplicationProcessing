using LoggerMicroService.Helpers.Loggers.Serilog;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace LoggerMicroService
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
            var config = new SerilogConfig();
            Configuration.Bind("SerilogConfig", config);      //  Bind "SerilogConfig" Object to "SerilogConfig" config section

            // save the list of CORS sites to property.. 
            // easy to do it now when we have access to the config object (TrustScienceConfiguration)
            _corsList = config.CorsList;

            services.AddSingleton(config);

            // set the logger file locations
            CommonLogger.SetLoggers(config);

            services.AddControllers();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
