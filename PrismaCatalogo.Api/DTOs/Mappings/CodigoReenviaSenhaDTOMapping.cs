using AutoMapper;
using PrismaCatalogo.Api.DTOs.CodigoReenviaSenhaDTO;
using PrismaCatalogo.Api.DTOs.CorDTO;
using PrismaCatalogo.Api.Models;


namespace PrismaCatalogo.Api.DTOs.Mappings
{
    public class CodigoReenviaSenhaDTOMapping : Profile
    {
        public CodigoReenviaSenhaDTOMapping()
        {
            CreateMap<CodigoReenviaSenha, CodigoReenviaSenhaRequestDTO>().ReverseMap();

            CreateMap<CodigoReenviaSenha, CodigoReenviaSenhaResponseDTO>().ReverseMap();
        }
    }
}
