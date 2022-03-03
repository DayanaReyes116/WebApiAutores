using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Reflection;

namespace Api.Extesions
{
    /// <summary>
    /// Configuración de Swagger
    /// </summary>
    public static class SwaggerApp
    {
        /// <summary>
        /// Parametrizar la inicialización del Swagger de manera personalizada.
        /// </summary>
        /// <param name="services">Colección de Servicios de la aplicación.</param>
        /// <returns>Colección de servicios con el swagger adicionado.</returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Autores",
                    Version = "v1",
                    Description = "Servicios Web API para el backend.",
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
            });

            return services;
        }

        /// <summary>
        /// Parametrización del uso del swagger de la aplicación
        /// </summary>
        /// <param name="app">Aplicación</param>
        /// <returns>Aplicación con swagger definido.</returns>
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger().UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1 WebApiAutores");
                options.DocumentTitle = "API v1";
                options.DocExpansion(DocExpansion.None);
            });
            return app;
        }
    }
}
