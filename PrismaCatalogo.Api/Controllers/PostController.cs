using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrismaCatalogo.Api.DTOs.CorDTO;
using PrismaCatalogo.Api.Models;
using PrismaCatalogo.Api.Services.Interfaces;

namespace PrismaCatalogo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    { 
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult<PostResponseDTO>> Post(PostRequestDTO postRequestDTO) { 
            Post p = _mapper.Map<Post>(postRequestDTO);

            var aa = await _postService.PublicAsync(p);

            return Ok();
        }
    }

}
