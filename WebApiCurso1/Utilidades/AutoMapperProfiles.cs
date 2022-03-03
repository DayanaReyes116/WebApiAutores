using AutoMapper;
using System;
using System.Collections.Generic;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Utilidades
{
    public class AutoMapperProfiles : Profile 
    {
        public AutoMapperProfiles()
        {
            CreateMap<AutorCreacionDTO, Autor>();

            CreateMap<Autor, AutorOutputDto>();
            CreateMap<Autor, AutorDTOConLibros>()
                .ForMember(autorDTO => autorDTO.Libros, opciones => opciones.MapFrom(MapAutorDTOLibros));


            CreateMap<Libro, LibroDTO>();
            CreateMap<Libro, LibroDTOConAutores>()
                .ForMember(libroDto => libroDto.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));


            CreateMap<LibroCreacionDTO, Libro>()
                .ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));

            CreateMap<LibroPatchDTO, Libro>().ReverseMap();
            CreateMap<ComentarioCreacionDto, Comentario>();
            CreateMap<Comentario, ComentarioDto>();
            CreateMap<ComentarioDto, Comentario>();
        }

        private List<LibroDTO> MapAutorDTOLibros(Autor autor, AutorDTOConLibros autorDTOConLibro)
        {
            var resultado = new List<LibroDTO>();

            if (autor.AutoresLibros == null)
            {
                return resultado;
            }

            foreach (var autorLibro in autor.AutoresLibros)
            {
                resultado.Add(new LibroDTO() 
                { 
                    Id = autorLibro.LibroId ,
                    Title = autorLibro.Libro.Title
                });
            }
            return resultado;
        }

        private List<AutorLibro> MapAutoresLibros(LibroCreacionDTO libroCreacionDTO, Libro libro)
        {
            var resultado = new List<AutorLibro>();

            if (libroCreacionDTO.AutoresIds == null )
            {
                return resultado;
            }

            foreach (var autorId in libroCreacionDTO.AutoresIds)
            {
                resultado.Add(new AutorLibro() { AutorId = autorId});
            }
            return resultado;
        }

        private List<AutorOutputDto> MapLibroDTOAutores(Libro libro, LibroDTO libroDTO)
        {
            var resultado = new List<AutorOutputDto>();

            if (libro.AutoresLibros == null)
            {
                return resultado;
            }

            foreach (var autorLibro in libro.AutoresLibros)
            {
                resultado.Add(new AutorOutputDto() 
                                                {
                                                    Id = autorLibro.AutorId,
                                                    Name = autorLibro.Autor.Name
                                                });
            }
            return resultado;
        }
    }
}
