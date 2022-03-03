using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.DTOs
{
    public class LibroDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public List<ComentarioDto> comentarios { get; set; }
    }
}
