using AutoMapper;
using PrismaCatalogo.Api.DTOs.TamanhoDTO;
using PrismaCatalogo.Api.Models;


namespace PrismaCatalogo.Api.DTOs.Mappings
{
    public class TamanhoDTOMapping : Profile
    {
        public TamanhoDTOMapping()
        {
            CreateMap<Tamanho, TamanhoRequestDTO>().ReverseMap()
                .ForMember(t => t.Nome, dto => dto.MapFrom(n => n.Nome != null ? n.Nome.Trim() : n.Nome));

            CreateMap<Tamanho, TamanhoResponseDTO>().ReverseMap();
        }
    }
}
