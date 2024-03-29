﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Libro
    {
        public int Id { get; set; }

        [Required]
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength:250)]
        public string Title { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public List<Comentario> Comentarios { get; set; }

        public List<AutorLibro> AutoresLibros { get; set; }
    }
}
