using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace ds.pms.apicommon.StartupConfigExtensions
{
    public static class SwaggerStartupExtension
    {
        public static void AddPmsApiSwaggerConfig(this IServiceCollection services, IHostingEnvironment _hostingEnv)
        {
            services.ConfigureGzipCompression();
            services.ConfigureJsonFormat();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "PMS API",
                    Description = "Property Management System API (ASP.NET Core 3.1)",
                    TermsOfService = new Uri("https://devsight.com/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Dev",
                        Url = new Uri("https://devsight.com/"),
                        Email = "info@devsight.com"
                    }
                });

                //c.DescribeAllEnumsAsStrings();
                c.CustomSchemaIds(x => x.FullName);
                // Set the comments path for the Swagger JSON and UI.
                c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml");

                c.ConfigureSwaggerSecurity();
            });
        }

        public static void UsePmsApiSwaggerConfig(this IApplicationBuilder app)
        {
            app.GlobalCorsAndJwtConfig();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PMS API V1");
                c.RoutePrefix = "swagger";
            });
        }

        private static void ConfigureJsonFormat(this IServiceCollection services)
        {
            //services.AddMvcCore().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options =>
            //{
            //    options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //    options.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
            //    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            //});
        }

        private static void ConfigureGzipCompression(this IServiceCollection services)
        {
            services.AddResponseCompression();
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
        }

        private static void GlobalCorsAndJwtConfig(this IApplicationBuilder app)
        {
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseCustomJwtConfig();
            app.UseResponseCompression();
            app
                .UseEndpoints(endpoints => { endpoints.MapControllers(); })
                .UseDefaultFiles()
                .UseStaticFiles();
        }

        private static void ConfigureSwaggerSecurity(this SwaggerGenOptions c)
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
        }
    }
}
