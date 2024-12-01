using AutoMapper;
using PrismaCatalogo.Api.DTOs.CategoriaDTO;
using PrismaCatalogo.Api.DTOs.CorDTO;
using PrismaCatalogo.Api.Models;
using System.Linq;


namespace PrismaCatalogo.Api.DTOs.Mappings
{
    public class CategoriaDTOMapping : Profile
    {
        public CategoriaDTOMapping()
        {
            CreateMap<Categoria, CategoriaRequestDTO>().ReverseMap()
                .ForMember(c => c.Nome, dto => dto.MapFrom(n => n.Nome != null ? n.Nome.Trim() : n.Nome));

            CreateMap<CategoriaResponseDTO, Categoria>().ReverseMap()
                //.ForPath(dest => dest.CategoriasFilhas, opt => opt.MapFrom(src => src.CategoriasFilhas))
                .ForMember(dest => dest.CategoriasFilhas, opt => opt.MapFrom(src => src.CategoriasFilhas));

            //CreateMap<CategoriaResponseDTO, Categoria>().ReverseMap()
            //    .ForPath(dest => dest.CategoriasFilhas, opt => opt.MapFrom(src => src.CategoriasFilhas != null ? src.CategoriasFilhas.Select(i => i.Id) : null));
        }
    }
}
