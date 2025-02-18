using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrismaCatalogo.Api.DTOs.TamanhoDTO;
using PrismaCatalogo.Api.Exceptions;
using PrismaCatalogo.Api.Filters;
using PrismaCatalogo.Api.Models;
using PrismaCatalogo.Api.Repositories;
using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Validations;

namespace PrismaCatalogo.Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TamanhoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TamanhoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TamanhoResponseDTO>>> Get()
        {
            var tamanhos = await _unitOfWork.TamanhoRepository.GetAllAsync();
            
            if (tamanhos == null)
            {
                return NotFound();
            }

            var tamanhoResponse = _mapper.Map<IEnumerable<TamanhoResponseDTO>>(tamanhos);
            return Ok(tamanhoResponse);
        }

        [HttpGet("{id}", Name = "ObterTamanho")]
        public async Task<ActionResult<IEnumerable<TamanhoResponseDTO>>> Get(int id)
        {
            var tamanho = await _unitOfWork.TamanhoRepository.GetAsync(t => t.Id == id);

            var tamanhoResponse = _mapper.Map<TamanhoResponseDTO>(tamanho);
            return Ok(tamanhoResponse);
        }

        [HttpGet("GetByName/{nome}", Name = "ObterPorNomeTamanho")]
        public async Task<ActionResult<IEnumerable<TamanhoResponseDTO>>> GetByName(string nome)
        {
            var tamanho = await _unitOfWork.TamanhoRepository.GetAsync(t => t.Nome == nome);

            var tamanhoResponse = _mapper.Map<IEnumerable<TamanhoResponseDTO>>(tamanho);
            return Ok(tamanhoResponse);
        }

        [HttpPost]
        public async Task<ActionResult<TamanhoResponseDTO>> Post(TamanhoRequestDTO tamanhoRequest)
        {
            Tamanho tamanho = _mapper.Map<Tamanho>(tamanhoRequest);

            validaStruturaDados((await _unitOfWork.TamanhoRepository.GetAllAsync()), tamanho);


            var novoTamanho = _unitOfWork.TamanhoRepository.Create(tamanho);
            await _unitOfWork.CommitAsync();

            var tamanhoResponse = _mapper.Map<TamanhoResponseDTO>(novoTamanho);

            return Ok(tamanhoResponse);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TamanhoResponseDTO>> Put(int id, TamanhoRequestDTO tamanhoRequest)
        {
            var tamanho = _mapper.Map<Tamanho>(tamanhoRequest);

            if (!TamanhoExists(id))
                throw new APIException("Tamnho não encontrado", StatusCodes.Status404NotFound);

            var tamanhos = (await _unitOfWork.TamanhoRepository.GetAllAsync()).Where(t => t.Id != id);
            validaStruturaDados(tamanhos, tamanho);

            tamanho.Id = id;
            var tamanhoUp = _unitOfWork.TamanhoRepository.Update(tamanho);
            await _unitOfWork.CommitAsync();

            var tamanhoResponse = _mapper.Map<TamanhoResponseDTO>(tamanhoUp);


            return Ok(tamanhoResponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TamanhoResponseDTO>> Delete(int id)
        {
            if (!TamanhoExists(id))
                throw new APIException("Tamnho não encontrado", StatusCodes.Status404NotFound);

            var tamanho = await _unitOfWork.TamanhoRepository.GetAsync(t => t.Id == id);
            _unitOfWork.TamanhoRepository.Delete(tamanho);
            await _unitOfWork.CommitAsync();

            var tamanhoResponse = _mapper.Map<TamanhoResponseDTO>(tamanho);

            return Ok(tamanhoResponse);
        }

        private void validaStruturaDados(IEnumerable<Tamanho> tamanhos, Tamanho tamanho)
        {
            var validation = new TamanhoValidator(tamanhos);
            var result = validation.Validate(tamanho);

            if (!result.IsValid)
            {
                ModelState.Clear();
                result.AddToModelState(ModelState);

                throw new APIException("Dados inconssistentes", StatusCodes.Status422UnprocessableEntity);
            }
        }

        [HttpDelete("{id:int}")]
        private  bool TamanhoExists(int id)
        {
            return _unitOfWork.TamanhoRepository.GetAllAsync().Result.Where(t => t.Id == id).FirstOrDefault() != null;
        }
            
    }
}
