using Api.Extesions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Text.Json.Serialization;
using WebApiAutores;
using WebApiAutores.Filtros;
using WebApiAutores.Middlewares;
using WebApiAutores.Servicios;

namespace WebApiCurso1
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
            //services.AddControllers(opciones =>
            //{
            //    opciones.Filters.Add(typeof(FiltroDeExcepcion));
            //}).AddJsonOptions(x =>
            //                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            services.AddControllers();

            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddNewtonsoftJson();


            #region Capitulo manejo de filtros globales en este caso registro de exceptiones de mi api 
            ////La comfiguraciòn del filtro Global se hace agregando al addController la opcion de MiFiltroException
            ///este es un ejemplo comentado del no registro del filtro 


            //services.AddControllers().AddJsonOptions(x =>
            //                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            #endregion

            services.AddDbContext<ApplicationDbContext>(options =>
                                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"))
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiCurso1", Version = "v1" });
            });

            ////inyección del filtro personalizado
            //services.AddTransient<MiFiltroDeAccion_SoloEjemplo>();
            //services.AddHostedService<EscribirEnArchivo>();

            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {

            #region Manejo de Middleware personalizados y centralizados
            // este middleware va a retorrar un string y no va ha correr los demas middlerware " Detiene LOS DEMAS"
            //app.Run(async contexto =>
            //{
            //    await contexto.Response.WriteAsync("Estoy interceptando la tuberia.");
            //});

            //para rutas especificas, es decir se especifica que la el middeware se ejecuta segun la ruta especificada y no la otra que es lineas
            ////** con el run se intersectan los procesos
            //app.Map("/ruta1", app =>
            //{
            //    app.Run(async contexto =>
            //    {
            //        await contexto.Response.WriteAsync("Estoy interceptando la tuberia.");
            //    });
            //});

            ///en otro ejemplo podemos  atrapar las respuestas **sirve para guardar todo lo que se envia en el cuerpo de la respuesta a nuestro cliente para todos los controladores
            /////Este es un Middleware personalizado 
            //app.Use(async (contexto, siguiente) =>
            //    {
            //        using (var ms = new MemoryStream())
            //        {
            //            var cuerpoOriginalRespuesta = contexto.Response.Body;
            //            contexto.Response.Body = ms;

            //            await siguiente.Invoke();
            //            ms.Seek(0, SeekOrigin.Begin);
            //            string respuesta = new StreamReader(ms).ReadToEnd();
            //            ms.Seek(0, SeekOrigin.Begin);

            //            await ms.CopyToAsync(cuerpoOriginalRespuesta);
            //            contexto.Response.Body = cuerpoOriginalRespuesta;

            //            logger.LogInformation(respuesta);
            //        }
            //    });

            ///ahora con Una clase creada  se hace de una mejor manera creando middleware

            ///fin de ejemplo clase 41 de Middleware
            ///
            //De esta manera se expone la clase utilizada, ejemplo la siguiente linea, para no exponer y hacerlo mas bonito se hace lo que viene despues de app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
            //app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();

            //con la clase estatica creada desde la clase Middlerware no se expone, así centralizamos operaciones en nuestro WebAppi
            //app.UseLoguearRespuestaHTTP();
            #endregion

            #region Manejo de FILTROS personalizados y globales
            //nos permiten reutilizar codigo que queremos aplicar en distintas etapas
            //controlador de accion personalizada

            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCustomSwagger();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
