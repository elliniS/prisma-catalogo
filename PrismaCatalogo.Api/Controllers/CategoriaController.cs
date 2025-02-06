using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrismaCatalogo.Api.DTOs.CategoriaDTO;
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
    public class CategoriaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriaController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Roles = "Funcionario,Cliente")]
        public async Task<ActionResult<IEnumerable<CategoriaResponseDTO>>> Get()
        {
            var categorias = await _unitOfWork.CategoriaRepository.GetAllAsync();

            var categoriaResponse = _mapper.Map<IEnumerable<CategoriaResponseDTO>>(categorias);
            return Ok(categoriaResponse);
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public async Task<ActionResult<IEnumerable<CategoriaResponseDTO>>> Get(int id)
        {
            var categoria = await _unitOfWork.CategoriaRepository.GetAsync(t => t.Id == id);

            var categoriaResponse = _mapper.Map<CategoriaResponseDTO>(categoria);
            return Ok(categoriaResponse);
        }

        [HttpGet("GetByName/{nome}", Name = "ObterPorNomeCategoria")]
        public async Task<ActionResult<IEnumerable<CategoriaResponseDTO>>> GetByName(string nome)
        {
            var categoria = await _unitOfWork.CategoriaRepository.GetAsync(t => t.Nome == nome);

            var categoriaResponse = _mapper.Map<IEnumerable<CategoriaResponseDTO>>(categoria);
            return Ok(categoriaResponse);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaResponseDTO>> Post(CategoriaRequestDTO categoriaRequest)
        {
            Categoria categoria = _mapper.Map<Categoria>(categoriaRequest);

            validaStruturaDados((await _unitOfWork.CategoriaRepository.GetCategoriasMesmoNivel(categoria.IdPai)), categoria);


            var novoCategoria = _unitOfWork.CategoriaRepository.Create(categoria);
            await _unitOfWork.CommitAsync();

            var categoriaResponse = _mapper.Map<CategoriaResponseDTO>(novoCategoria);

            return Ok(categoriaResponse);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaResponseDTO>> Put(int id, CategoriaRequestDTO categoriaRequest)
        {
            var categoria = _mapper.Map<Categoria>(categoriaRequest);

            if (!CategoriaExists(id))
                throw new APIException("Categoria não encontrada", StatusCodes.Status404NotFound);

            var categorias = (await _unitOfWork.CategoriaRepository.GetCategoriasMesmoNivel(categoria.IdPai)).Where(t => t.Id != id);
            validaStruturaDados(categorias, categoria);

            categoria.Id = id;
            var categoriaUp = _unitOfWork.CategoriaRepository.Update(categoria);
            await _unitOfWork.CommitAsync();

            var categoriaResponse = _mapper.Map<CategoriaResponseDTO>(categoriaUp);


            return Ok(categoriaResponse);

            }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoriaResponseDTO>> Delete(int id)
        {
            if (!CategoriaExists(id))
                throw new APIException("Categoria não encontrada", StatusCodes.Status404NotFound);

            var categoria = await _unitOfWork.CategoriaRepository.GetAsync(t => t.Id == id);

            var validation = new CategoriaValidator();
            var result = validation.Validate(categoria);

            if (!result.IsValid)
            {
                ModelState.Clear();
                result.AddToModelState(ModelState);

                throw new APIException("Dados inconssistentes", StatusCodes.Status422UnprocessableEntity);
            }

            _unitOfWork.CategoriaRepository.Delete(categoria);
            await _unitOfWork.CommitAsync();

            var categoriaResponse = _mapper.Map<CategoriaResponseDTO>(categoria);

            return Ok(categoriaResponse);
        }

        private void validaStruturaDados(IEnumerable<Categoria> categorias, Categoria categoria)
        {
            var validation = new CategoriaValidator(categorias);
            var result = validation.Validate(categoria);

            if (!result.IsValid)
            {
                ModelState.Clear();
                result.AddToModelState(ModelState);

                throw new APIException("Dados inconssistentes", StatusCodes.Status422UnprocessableEntity);
            }
        }

        [HttpDelete("{id:int}")]
        private  bool CategoriaExists(int id)
        {
            return _unitOfWork.CategoriaRepository.GetAllAsync().Result.Where(t => t.Id == id).FirstOrDefault() != null;
        }
            
    }
}
