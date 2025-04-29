using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrismaCatalogo.Api.DTOs.AvaliacaoDTO;
using PrismaCatalogo.Api.Exceptions;
using PrismaCatalogo.Api.Filters;
using PrismaCatalogo.Api.Models;
using PrismaCatalogo.Api.Repositories;
using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Validations;
using PrismaCatalogo.Validations;

namespace PrismaCatalogo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AvaliacaoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<AvaliacaoResponseDTO>>> Get()
        //{
        //    var avaliacoes = await _unitOfWork.AvaliacaoRepository.GetAllAsync();

        //    var avaliacaoDto = _mapper.Map<IEnumerable<AvaliacaoResponseDTO>>(avaliacoes);
        //    return Ok(avaliacaoDto);
        //}

        //[HttpGet("{id}", Name = "ObterAvaliacao")]
        //public async Task<ActionResult<AvaliacaoResponseDTO>> Get(int id)
        //{
        //    var avaliacao = await _unitOfWork.AvaliacaoRepository.GetAsync(t => t.Id == id);

        //    var avaliacaoResponse = _mapper.Map<AvaliacaoResponseDTO>(avaliacao);
        //    return Ok(avaliacaoResponse);
        //}

        [HttpGet("GetByProduto/{id}", Name = "ObterAvaliacaoPorProduto")]
        public async Task<ActionResult<IEnumerable<AvaliacaoResponseDTO>>> GetByProduto(int id)
        {
            var avaliacao = await _unitOfWork.AvaliacaoRepository.GetListAsync(t => t.ProdutoId == id);

            var avaliacaoResponse = _mapper.Map<IEnumerable<AvaliacaoResponseDTO>>(avaliacao);
            return Ok(avaliacaoResponse);
        }

        [HttpGet("GetByProduto/{produtoId}/Usuario/{usuarioId}", Name = "ObterAvaliacaoPorProdutoUsuario")]
        public async Task<ActionResult<AvaliacaoResponseDTO>> GetByUsuario(int produtoId, int usuarioId)
        {
            var avaliacao = await _unitOfWork.AvaliacaoRepository.GetAsync(t => t.ProdutoId == produtoId && t.UsuarioId == usuarioId);
            var avaliacaoResponse = _mapper.Map<AvaliacaoResponseDTO>(avaliacao);

            return Ok(avaliacaoResponse);
        }

        [HttpPost]
        public async Task<ActionResult<AvaliacaoResponseDTO>> Post(AvaliacaoRequestDTO avaliacaoRequest)
        {
            Avaliacao avaliacao = _mapper.Map<Avaliacao>(avaliacaoRequest);

            /// validaStruturaDados(( await _unitOfWork.AvaliacaoRepository.GetAllAsync()), avaliacao);


            var novaAvaliacao = _unitOfWork.AvaliacaoRepository.Create(avaliacao); 
            await _unitOfWork.CommitAsync();

            var avaliacaoResponse = _mapper.Map<AvaliacaoResponseDTO>(novaAvaliacao);

            return Ok(avaliacaoResponse);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<AvaliacaoResponseDTO>> Put(int id, AvaliacaoRequestDTO avaliacaoRequest)
        {
            var avaliacao = _mapper.Map<Avaliacao>(avaliacaoRequest);

            if (!AvaliacaoExists(id))
                throw new APIException("Tamnho não encontrado", StatusCodes.Status404NotFound);

            //var avaliacoes = (await _unitOfWork.AvaliacaoRepository.GetAllAsync()).Where(t => t.Id != id);
            //validaStruturaDados(avaliacoes, avaliacao);

            avaliacao.Id = id;
            var avaliacaoUp = _unitOfWork.AvaliacaoRepository.Update(avaliacao);
            await _unitOfWork.CommitAsync();

            var avaliacaoResponse = _mapper.Map<AvaliacaoResponseDTO>(avaliacaoUp);


            return Ok(avaliacaoResponse);
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AvaliacaoResponseDTO>> Delete(int id)
        {
            if (!AvaliacaoExists(id))
                throw new APIException("Tamnho não encontrado", StatusCodes.Status404NotFound);

            var avaliacao = await _unitOfWork.AvaliacaoRepository.GetAsync(t => t.Id == id);
            _unitOfWork.AvaliacaoRepository.Delete(avaliacao);
            await _unitOfWork.CommitAsync();

            var avaliacaoResponse = _mapper.Map<AvaliacaoResponseDTO>(avaliacao);

            return Ok(avaliacaoResponse);
        }

        //private void validaStruturaDados(IEnumerable<Avaliacao> avaliacoes, Avaliacao avaliacao)
        //{
        //    var validation = new AvaliacaoValidator(avaliacoes);
        //    var result = validation.Validate(avaliacao);

        //    if (!result.IsValid)
        //    {
        //        ModelState.Clear();
        //        result.AddToModelState(ModelState);

        //        throw new APIException("Dados inconssistentes", StatusCodes.Status422UnprocessableEntity);
        //    }
        //}

        [HttpDelete("{id:int}")]
        private  bool AvaliacaoExists(int id)
        {
            return _unitOfWork.AvaliacaoRepository.GetAllAsync().Result.Where(t => t.Id == id).FirstOrDefault() != null;
        }
            
    }
}
