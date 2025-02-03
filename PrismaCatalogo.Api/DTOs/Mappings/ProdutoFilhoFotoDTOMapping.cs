using AutoMapper;
using PrismaCatalogo.Api.DTOs.CorDTO;
using PrismaCatalogo.Api.Models;


namespace PrismaCatalogo.Api.DTOs.Mappings
{
    public class ProdutoFilhoFotoDTOMapping : Profile
    {
        public ProdutoFilhoFotoDTOMapping()
        {
            CreateMap<ProdutoFilhoFoto, ProdutoFilhoFotoRequestDTO>().ReverseMap();

            CreateMap<ProdutoFilhoFoto, ProdutoFilhoFotoResponseDTO>().ReverseMap();
        }
    }
}
