using AutoMapper;
using PrismaCatalogo.Api.DTOs.CorDTO;
using PrismaCatalogo.Api.Models;


namespace PrismaCatalogo.Api.DTOs.Mappings
{
    public class PostDTOMapping : Profile
    {
        public PostDTOMapping()
        {
            CreateMap<Post, PostRequestDTO>().ReverseMap();

            CreateMap<Post, PostResponseDTO>().ReverseMap();
        }
    }
}
