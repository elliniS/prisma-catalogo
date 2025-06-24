using AutoMapper;
using AutoMapper;
using PrismaCatalogo.Api.DTOs.CorDTO;
using PrismaCatalogo.Api.Models;


namespace PrismaCatalogo.Api.DTOs.Mappings
{
    public class CorDTOMapping : Profile
    {
        public CorDTOMapping()
        {
            CreateMap<Cor, CorRequestDTO>().ReverseMap()
                .ForMember(c => c.Nome, dto => dto.MapFrom(n => n.Nome != null ? n.Nome.Trim() : n.Nome));

            CreateMap<Cor, CorResponseDTO>().ReverseMap();
        }
    }
}
