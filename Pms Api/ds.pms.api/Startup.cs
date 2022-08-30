using ds.pms.apicommon.Settings;
using ds.pms.apicommon.StartupConfigExtensions;
using ds.pms.bl.logger.LoggerTypeSettings.SeqConfigSetting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ds.pms.api
{
    public class Startup
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnv;
        private IConfiguration Configuration { get; }
        private string _loggingType;

        [System.Obsolete]
        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IConfiguration configuration)
        {
            _hostingEnv = env;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });
            services.Configure<ConnectionSettings>(Configuration.GetSection(typeof(ConnectionSettings).Name));
            services.Configure<IdentitySettings>(Configuration.GetSection(typeof(IdentitySettings).Name));

            var identitySettingsSection = Configuration.GetSection(typeof(IdentitySettings).Name);
            var identitySettings = identitySettingsSection.Get<IdentitySettings>();
            services.AddJwtConfig(identitySettings.JwtSecret, identitySettings.JwtExpirationSeconds);

            _loggingType = Configuration.GetValue<string>("LoggingType");
            //Add Swagger relates setting
            services.AddPmsApiSwaggerConfig(_hostingEnv);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            if (SeqConfiguration.IsSeqLogging(_loggingType))
                app.UseSerilogRequestLogging();

            app.UseAuthorization();

            // global cors policy

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials


            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UsePmsApiSwaggerConfig();
        }
    }
}
