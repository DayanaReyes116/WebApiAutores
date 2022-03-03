using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Autor
    { 
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo { 0 } es requerido.")]
        [StringLength(maximumLength: 100, ErrorMessage = "El campo  { 0 } no debe tener más de  { 1 } carácteres")]
        [PrimeraLetraMayuscula]
        public string Name { get; set; }
        public List<AutorLibro> AutoresLibros { get; set; }
    }
}
