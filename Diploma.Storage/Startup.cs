using System.Collections.Generic;
using System.Net;
using Diploma.Storage.Common.Exceptions;
using Diploma.Storage.Common.Providers.FileHash;
using Diploma.Storage.Services.Signature;
using Diploma.Storage.Services.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Diploma.Storage
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
            services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
            });
            
            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });
        
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Diploma.Storage", Version = "v1" });
            });

            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<ISignatureService, SignatureService>();
            services.AddScoped<IFileHashProvider, FileHashProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Diploma.Storage v1"));
            }

            app.UseSerilogRequestLogging();
            
            app.UseCors();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
            
            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var error = context.Features.Get<IExceptionHandlerPathFeature>().Error;
                var statusCode = HttpStatusCode.InternalServerError;
                var fields = new Dictionary<string, List<string>>();

                if (error is RequestException exception)
                {
                    statusCode = exception.StatusCode;
                    fields = exception.Details;
                }

                if (statusCode == HttpStatusCode.InternalServerError)
                {
                    logger.LogError(error,
                        $"Внутренняя ошибка в методе {0} {1}", context.Request.Method, context.Request.Path);
                }

                context.Response.StatusCode = (int) statusCode;
                await context.Response.WriteAsJsonAsync(new { fields, error = error.Message });
            }));
            
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}