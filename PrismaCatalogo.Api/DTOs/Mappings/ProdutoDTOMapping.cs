using AutoMapper;
using PrismaCatalogo.Api.DTOs.ProdutoDTO;
using PrismaCatalogo.Api.Models;


namespace PrismaCatalogo.Api.DTOs.Mappings
{
    public class ProdutoDTOMapping : Profile
    {
        public ProdutoDTOMapping()
        {
            CreateMap<Produto, ProdutoRequestDTO>().ReverseMap()
                .ForMember(t => t.Nome, dto => dto.MapFrom(n => n.Nome != null ? n.Nome.Trim() : n.Nome))
                .ForMember(dest => dest.Fotos, opt => opt.MapFrom(src => src.Fotos)); 
                

            CreateMap<Produto, ProdutoResponseDTO>().ReverseMap()
                .ForMember(dest => dest.ProdutosFilhos, opt => opt.MapFrom(src => src.ProdutosFilhos))
                .ForMember(dest => dest.Fotos, opt => opt.MapFrom(src => src.Fotos));
        }
    }
}
