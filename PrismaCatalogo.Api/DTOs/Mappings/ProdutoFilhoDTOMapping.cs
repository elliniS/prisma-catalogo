using AutoMapper;
using PrismaCatalogo.Api.DTOs.ProdutoFilhoDTO;
using PrismaCatalogo.Api.Models;


namespace PrismaCatalogo.Api.DTOs.Mappings
{
    public class ProdutoFilhoDTOMapping : Profile
    {
        public ProdutoFilhoDTOMapping()
        {
            CreateMap<ProdutoFilho, ProdutoFilhoRequestDTO>().ReverseMap()
                .ForMember(t => t.Nome, dto => dto.MapFrom(n => n.Nome != null ? n.Nome.Trim() : n.Nome))
                .ForMember(dest => dest.Fotos, opt => opt.MapFrom(src => src.Fotos));

            CreateMap<ProdutoFilho, ProdutoFilhoResponseDTO>().ReverseMap()
                //.ForMember(t => t.Nome, dto => dto.MapFrom(n => n.Nome != null && n.Nome.Trim() != "" ? n.Nome.Trim() : String.Format("{0}_{1}", n.Cor.Nome, n.Tamanho.Nome)))
                .ForMember(dest => dest.Cor, opt => opt.MapFrom(src => src.Cor))
                .ForMember(dest => dest.Tamanho, opt => opt.MapFrom(src => src.Tamanho))
                .ForMember(dest => dest.Fotos, opt => opt.MapFrom(src => src.Fotos));
        }
    }
}
