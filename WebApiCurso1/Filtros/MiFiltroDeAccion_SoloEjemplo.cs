using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace WebApiAutores.Filtros
{
    public class MiFiltroDeAccion_SoloEjemplo : IActionFilter
    {
        private readonly ILogger<MiFiltroDeAccion_SoloEjemplo> logger;

        public  MiFiltroDeAccion_SoloEjemplo(ILogger<MiFiltroDeAccion_SoloEjemplo> logger)
        {
            this.logger = logger;
        }
        //este se ejecuta antes de la accion
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Antes de ejecutar la acción.");
        }

        //Se ejecuta despues que la accion se ha ejecutado
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("Después de ejecutar la acción.");
        }
        //Este se debe registar en el sistema de inyeccion de dependencia en el startup
    }
}
