﻿using System;
using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.DTOs
{
    public class LibroPatchDTO
    {
        [Required(ErrorMessage = "El campo { 0 } es requerido.")]
        [StringLength(maximumLength: 250, ErrorMessage = "El campo  { 0 } no debe tener más de  { 1 } carácteres")]
        [PrimeraLetraMayuscula]
        public string Title { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
