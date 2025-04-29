using AutoMapper;
using PrismaCatalogo.Api.DTOs.AvaliacaoDTO;
using PrismaCatalogo.Api.Models;


namespace PrismaCatalogo.Api.DTOs.Mappings
{
    public class AvaliacaoDTOMapping : Profile
    {
        public AvaliacaoDTOMapping()
        {
            CreateMap<Avaliacao, AvaliacaoRequestDTO>().ReverseMap();

            CreateMap<Avaliacao, AvaliacaoResponseDTO>()
                .ForMember(dto => dto.NomeUsuario, opt => opt.MapFrom(a => a.Usuario != null ? a.Usuario.NomeUsuario : ""));
        }
    }
}
