using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Autor_Copia : IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo { 0 } es requerido.")]
        [StringLength(maximumLength: 100, ErrorMessage = "El campo  { 0 } no debe tener más de  { 1 } carácteres")]
        //********Esta Clase Primera Letra es una validación de atributo y siempre van hacer estas las primeras a consideerar
        [PrimeraLetraMayuscula]
        public string Name { get; set; }
        //public int Menor { get; set; }
        //public int Mayor { get; set; }
        public List<Libro> Libros { get; set; }
        //***Estas son validaciones de Entidades y son consideradas despues de las dde los atributos
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                var primeraLetra = Name[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayúscula",
                                                      new string[] { nameof(Name) });
                }
            }

            //if (Menor > Mayor )
            //{
            //    yield return new ValidationResult("El número menor no puede ser mayor",
            //                                          new string[] { nameof(Menor) });
            //}
        }
    }
}
