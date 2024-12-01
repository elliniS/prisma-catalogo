using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrismaCatalogo.Api.DTOs.CorDTO;
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
    public class CorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CorController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CorResponseDTO>>> Get()
        {
            var Cores = await _unitOfWork.CorRepository.GetAllAsync();

            var CorDto = _mapper.Map<IEnumerable<CorResponseDTO>>(Cores);
            return Ok(CorDto);
        }

        [HttpGet("{id}", Name = "ObterCor")]
        public async Task<ActionResult<IEnumerable<CorResponseDTO>>> Get(int id)
        {
            var cor = await _unitOfWork.CorRepository.GetAsync(t => t.Id == id);

            var corResponse = _mapper.Map<CorResponseDTO>(cor);
            return Ok(corResponse);
        }

        [HttpGet("GetByName/{nome}", Name = "ObterPorNomeCor")]
        public async Task<ActionResult<IEnumerable<CorResponseDTO>>> GetByName(string nome)
        {
            var cor = await _unitOfWork.CorRepository.GetAsync(t => t.Nome == nome);

            var corResponse = _mapper.Map<IEnumerable<CorResponseDTO>>(cor);
            return Ok(corResponse);
        }

        [HttpPost]
        public async Task<ActionResult<CorResponseDTO>> Post(CorRequestDTO corRequest)
        {
            Cor cor = _mapper.Map<Cor>(corRequest);

            validaStruturaDados(( await _unitOfWork.CorRepository.GetAllAsync()), cor);


            var novaCor = _unitOfWork.CorRepository.Create(cor); 
            await _unitOfWork.CommitAsync();

            var corResponse = _mapper.Map<CorResponseDTO>(novaCor);

            return Ok(corResponse);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CorResponseDTO>> Put(int id, CorRequestDTO corRequest)
        {
            var cor = _mapper.Map<Cor>(corRequest);

            if (!CorExists(id))
                throw new APIException("Tamnho não encontrado", StatusCodes.Status404NotFound);

            var cores = (await _unitOfWork.CorRepository.GetAllAsync()).Where(t => t.Id != id);
            validaStruturaDados(cores, cor);

            cor.Id = id;
            var corUp = _unitOfWork.CorRepository.Update(cor);
            await _unitOfWork.CommitAsync();

            var corResponse = _mapper.Map<CorResponseDTO>(corUp);


            return Ok(corResponse);
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CorResponseDTO>> Delete(int id)
        {
            if (!CorExists(id))
                throw new APIException("Tamnho não encontrado", StatusCodes.Status404NotFound);

            var cor = await _unitOfWork.CorRepository.GetAsync(t => t.Id == id);
            _unitOfWork.CorRepository.Delete(cor);
            await _unitOfWork.CommitAsync();

            var corResponse = _mapper.Map<CorResponseDTO>(cor);

            return Ok(corResponse);
        }

        private void validaStruturaDados(IEnumerable<Cor> cores, Cor cor)
        {
            var validation = new CorValidator(cores);
            var result = validation.Validate(cor);

            if (!result.IsValid)
            {
                ModelState.Clear();
                result.AddToModelState(ModelState);

                throw new APIException("Dados inconssistentes", StatusCodes.Status422UnprocessableEntity);
            }
        }

        [HttpDelete("{id:int}")]
        private  bool CorExists(int id)
        {
            return _unitOfWork.CorRepository.GetAllAsync().Result.Where(t => t.Id == id).FirstOrDefault() != null;
        }
            
    }
}
