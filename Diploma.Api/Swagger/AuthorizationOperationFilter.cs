using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Diploma.Server.Swagger
{
    /// <summary>
    /// Фильтр авторизации
    /// </summary>
    public sealed class AuthorizationOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Применить фильтр
        /// </summary>
        /// <param name="operation">Операция</param>
        /// <param name="context">Контекст операции</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var actionMetadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;
            var isAuthorized = actionMetadata.Any(metadataItem => metadataItem is AuthorizeAttribute);
            var allowAnonymous = actionMetadata.Any(metadataItem => metadataItem is AllowAnonymousAttribute);

            if (!isAuthorized || allowAnonymous)
            {
                return;
            }

            operation.Parameters ??= new List<OpenApiParameter>();
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            } 
                        },
                        Array.Empty<string>()
                    }
                }
            };
        }
    }
}