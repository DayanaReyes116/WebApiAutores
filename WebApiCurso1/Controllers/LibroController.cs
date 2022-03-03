using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibroController : ControllerBase
    {
        private readonly IMapper mapper;

        public ApplicationDbContext context { get; }
        public LibroController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "ObtenerLibro")]
        public async Task<ActionResult<LibroDTOConAutores>> Get(int id)
        {
            var existeLibro = await context.Libros.AnyAsync(x => x.Id == id);

            if (!existeLibro)
            {
                return NotFound("No existe ese Libro.");
            }

            //return await Context.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);
            var libroResult = await context.Libros.Include(LibroBD => LibroBD.Comentarios)
                .Include(LibroBD => LibroBD.AutoresLibros)
                .ThenInclude(autorLibroDB => autorLibroDB.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            libroResult.AutoresLibros = libroResult.AutoresLibros.OrderBy(x => x.Orden).ToList();
            return mapper.Map<LibroDTOConAutores>(libroResult);

        }

        [HttpPost("crearLibro")]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO)
        {
            if (libroCreacionDTO.AutoresIds == null)
            {
                return BadRequest("No se puede crear un libro sin autores.");
            }

            var autoresIds = await context.Autores
                            .Where(autorBD => libroCreacionDTO.AutoresIds.Contains(autorBD.Id)).Select(x => x.Id).ToListAsync();

            if (autoresIds.Count != libroCreacionDTO.AutoresIds.Count)
            {
                return BadRequest("No existe uno de los autores enviados.");
            }

            #region Comentados
            //var existAutor = await Context.Autores.AnyAsync(x => x.Id == autorId);

            //if (!existAutor)
            //{
            //    return BadRequest($"No existe el autor de Id: {autorId}");
            //}
            #endregion

            var libro = mapper.Map<Libro>(libroCreacionDTO);
            AsignarOrdenAutores(libro);

            context.Add(libro);
            await context.SaveChangesAsync();

            var LibroDto = mapper.Map<LibroDTO>(libro);

            return CreatedAtRoute("ObtenerLibro", new { id = libro.Id }, LibroDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, LibroCreacionDTO libroCreacionDTO)
        {
            var libroDB = await context.Libros
                            .Include(x => x.AutoresLibros)
                            .FirstOrDefaultAsync(t => t.Id == id);

            if (libroDB == null)
            {
                return NotFound();
            }

            libroDB = mapper.Map(libroCreacionDTO, libroDB);
            AsignarOrdenAutores(libroDB);
            await context.SaveChangesAsync();
            return NoContent();
        }
        
        #region Métodos privados
        private void AsignarOrdenAutores(Libro libro)
        {
            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }
            }
        }
        #endregion

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var libroDB = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);

            if (libroDB == null)
            {
                return NotFound();
            }

            var libroDTO = mapper.Map<LibroPatchDTO>(libroDB);
            patchDocument.ApplyTo(libroDTO, ModelState);

            var esValido = TryValidateModel(libroDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(libroDTO, libroDB);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await context.Libros.AnyAsync(t => t.Id == id);

            if (!exists)
            {
                return NotFound();
            }

            context.Remove(new Libro() { Id = id });
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
