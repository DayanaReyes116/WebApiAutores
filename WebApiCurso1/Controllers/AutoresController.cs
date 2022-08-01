using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AutoresController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        //[HttpGet("configuraciones")]
        //public ActionResult<string>ObtenerConfiguracion()
        //{
        //    return configuration["testing"];
        //}

        [HttpGet("{id:int}", Name = "ObtenerAutor")]
        public async Task<ActionResult<AutorDTOConLibros>> Get(int id)
        {
            var autor = await context.Autores
                .Include(autorDB => autorDB.AutoresLibros)
                .ThenInclude(autorLibroDB => autorLibroDB.Libro)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            return mapper.Map<AutorDTOConLibros>(autor);
        }

        [HttpGet("{nombre}")]//**"{nombre} estos son parametros de rutas
        public async Task<ActionResult<List<AutorOutputDto>>> Get([FromRoute] string nombre)
        {
            var autores = await context.Autores.Where(autorDB => autorDB.Name.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorOutputDto>>(autores);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[ServiceFilter(typeof(MiFiltroDeAccion_SoloEjemplo))]
        public async Task<ActionResult<List<AutorOutputDto>>> Get()
        {
            //throw new NotImplementedException();
            
            //return await context.Autores.Include(x => x.Libros).ToListAsync();
            var autores = await context.Autores.ToListAsync();

            return mapper.Map<List<AutorOutputDto>>(autores);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
        {
            //Válidaciones en controlador
            var existeAutor = await context.Autores.AnyAsync(x => x.Name == autorCreacionDTO.Name);

            if (existeAutor)
            {
                return BadRequest($"Ya existe un Autor con el nombre  { autorCreacionDTO.Name }");
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);
            context.Add(autor);
            await context.SaveChangesAsync();//los cambios van hacer persistido en la bd, cuando decimos persistir es insertar en la BD

            var autorDTO = mapper.Map<AutorOutputDto>(autor);
            return CreatedAtRoute("ObtenerAutor", new { Id = autor.Id}, autorDTO);
        }

        [HttpPut("{id:int}")] //api/autores/1
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id) 
        {
            var existeAutor = await context.Autores.AnyAsync(t => t.Id == id);

            if (!existeAutor)
            {
                NotFound();
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(Autor autor, int id) 
        {
            var exists = await context.Autores.AnyAsync(t=> t.Id == id);
            
            if (!exists)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id});
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
