using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;
using Diploma.Bll.Common.Exceptions;
using Diploma.Bll.Common.Providers.KeysProvider;
using Diploma.Bll.Services.Access;
using Diploma.Bll.Services.Authorization;
using Diploma.Bll.Services.Authorization.Request;
using Diploma.Bll.Services.Authorization.Validation;
using Diploma.Bll.Services.Chats;
using Diploma.Bll.Services.Encryption;
using Diploma.Bll.Services.Messages;
using Diploma.Bll.Services.Token;
using Diploma.Bll.Services.Users;
using Diploma.Bll.Services.WebSocket;
using Diploma.Persistence;
using Diploma.Server.Swagger;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Diploma.Server
{
    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => 
                { 
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod(); 
                });
            });
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Diploma.Api", 
                    Version = "v1"
                }); 
                c.OperationFilter<AuthorizationOperationFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Authorization \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below"
                });
            });
            
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDataProtection();
            services
                .AddControllers()
                .AddJsonOptions(o => { o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = TokenService.TokenValidationParameters(_configuration);
                });
            
            services.AddFluentValidationAutoValidation();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IValidator<AuthRequest>, AuthRequestValidator>();
            
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccessManager, AccessManager>();
            services.AddScoped<ICryptoService, CryptoService>();
            services.AddScoped<IKeysProvider, KeysProvider>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddSingleton<IWebSocketService, WebSocketService>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSerilogRequestLogging();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Diploma.Api v1"));
            }

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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}