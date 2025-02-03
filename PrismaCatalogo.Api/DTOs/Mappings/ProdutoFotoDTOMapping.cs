using AutoMapper;
using PrismaCatalogo.Api.DTOs.CorDTO;
using PrismaCatalogo.Api.Models;


namespace PrismaCatalogo.Api.DTOs.Mappings
{
    public class ProdutoFotoDTOMapping : Profile
    {
        public ProdutoFotoDTOMapping()
        {
            CreateMap<ProdutoFoto, ProdutoFotoRequestDTO>().ReverseMap();

            CreateMap<ProdutoFoto, ProdutoFotoResponseDTO>().ReverseMap();
        }
    }
}
