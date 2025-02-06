using AutoMapper;
using PrismaCatalogo.Api.DTOs.UsuarioDTO;
using PrismaCatalogo.Api.Models;


namespace PrismaCatalogo.Api.DTOs.Mappings
{
    public class UsuarioDTOMapping : Profile
    {
        public UsuarioDTOMapping()
        {
            CreateMap<Usuario, UsuarioRequestDTO>().ReverseMap()
                .ForMember(c => c.Nome, dto => dto.MapFrom(n => n.Nome != null ? n.Nome.Trim() : n.Nome));

            CreateMap<Usuario, UsuarioLoginRequestDTO>().ReverseMap();

            CreateMap<Usuario, UsuarioResponseDTO>().ReverseMap();

            CreateMap<Usuario, UsuarioTokenResponseDTO>().ReverseMap();
        }
    }
}
