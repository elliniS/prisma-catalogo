using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrismaCatalogo.Api.DTOs.CorDTO;
using PrismaCatalogo.Api.DTOs.ProdutoDTO;
using PrismaCatalogo.Api.Exceptions;
using PrismaCatalogo.Api.Filters;
using PrismaCatalogo.Api.Models;
using PrismaCatalogo.Api.Repositories;
using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Services.Interfaces;
using PrismaCatalogo.Api.Validations;

namespace PrismaCatalogo.Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImagemService _imageService;

        public ProdutoController(IUnitOfWork unitOfWork, IMapper mapper, IImagemService imagemService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imagemService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoResponseDTO>>> Get()
        {
            var produtos = await _unitOfWork.ProdutoRepository.GetAllAsync(p => new { p.Id, p.Nome, p.Descricao, p.Ativo, Fotocapa = p.Fotos.FirstOrDefault(), Preco = p.ProdutosFilhos.Min(pf => pf.Preco)});
            
            if (produtos == null)
            {
                return NotFound();
            }

            var produtoResponse = _mapper.Map<IEnumerable<ProdutoResponseDTO>>(produtos);
            return Ok(produtoResponse);
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "ObterProduto")]
        public async Task<ActionResult<IEnumerable<ProdutoResponseDTO>>> Get(int id)
        {
            var produto = await _unitOfWork.ProdutoRepository.GetAsync(t => t.Id == id);

            var produtoResponse = _mapper.Map<ProdutoResponseDTO>(produto);
            return Ok(produtoResponse);
        }

        [AllowAnonymous]
        [HttpGet("GetByName/{nome}", Name = "ObterPorNomeProduto")]
        public async Task<ActionResult<IEnumerable<ProdutoResponseDTO>>> GetByName(string nome)
        {
            var produto = await _unitOfWork.ProdutoRepository.GetAsync(t => t.Nome == nome);

            var produtoResponse = _mapper.Map<IEnumerable<ProdutoResponseDTO>>(produto);
            return Ok(produtoResponse);
        }

        [AllowAnonymous]
        [HttpGet("GetByCategoria/{categoriaId}", Name = "ObterPorCategoria")]
        public async Task<ActionResult<IEnumerable<ProdutoResponseDTO>>> GetByCategoria(int categoriaId)
        {

            List<Produto> produtos = new List<Produto>();

            var cat = await _unitOfWork.CategoriaRepository.GetAsync(c =>  c.Id == categoriaId);

            var aa = await ObtemProdutosCategoriasFilhas(cat, produtos);


             //var produto = await _unitOfWork.ProdutoRepository.GetListAsync(t => t.Categoria != null && t.Categoria.Id == categoriaId, p => new { p.Id, p.Nome, p.Descricao, p.Ativo, Fotocapa = p.Fotos.FirstOrDefault(), Preco = p.ProdutosFilhos.Min(pf => pf.Preco) });

            var produtoResponse = _mapper.Map<IEnumerable<ProdutoResponseDTO>>(produtos);
            return Ok(produtoResponse);
        }

        private async Task<List<Produto>> ObtemProdutosCategoriasFilhas(Categoria categoria, List<Produto> produtos)
        {
            produtos.AddRange(await _unitOfWork.ProdutoRepository.GetListAsync(t => t.Categoria != null && t.Categoria.Id == categoria.Id, p => new { p.Id, p.Nome, p.Descricao, p.Ativo, Fotocapa = p.Fotos.FirstOrDefault(), Preco = p.ProdutosFilhos.Min(pf => pf.Preco), AvaliacaoMedia =  p.Avaliacoes != null && p.Avaliacoes.Count() > 0 ? p.Avaliacoes.Average(a => a.Nota) : (double?)null }));


            foreach(Categoria c in categoria.CategoriasFilhas)
            {
                var cat = await _unitOfWork.CategoriaRepository.GetAsync(i => i.Id == c.Id);

                await ObtemProdutosCategoriasFilhas(cat, produtos);
            }

            return produtos;
        }


        [HttpPost]
        public async Task<ActionResult<ProdutoResponseDTO>> Post(ProdutoRequestDTO produtoRequest)
        {
            Produto produto = _mapper.Map<Produto>(produtoRequest);

            //var aa = await _unitOfWork.ProdutoRepository.GetAllAsync();

            validaStruturaDados((await _unitOfWork.ProdutoRepository.GetAllAsync()), produto);

            var novoProduto = _unitOfWork.ProdutoRepository.Create(produto);
            await _unitOfWork.CommitAsync();

            GerenciarFotos(produtoRequest.Fotos, produto.Id);
            await _unitOfWork.CommitAsync();

            var produtoResponse = _mapper.Map<ProdutoResponseDTO>(novoProduto);

            return Ok(produtoResponse);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProdutoResponseDTO>> Put(int id, ProdutoRequestDTO produtoRequest)
        {


            var produtoFotot = await _unitOfWork.ProdutoFotoRepository.GetAsync(f => f.Id == 1);

            var produto = _mapper.Map<Produto>(produtoRequest);

            if (!ProdutoExists(id))
                throw new APIException("Ptoduto não encontrado", StatusCodes.Status404NotFound);

            var produtos = (await _unitOfWork.ProdutoRepository.GetAllAsync()).Where(t => t.Id != id);
            validaStruturaDados(produtos, produto);

            produto.Id = id;
            produto.Fotos = null;
            var produtoUp = _unitOfWork.ProdutoRepository.Update(produto);
            await _unitOfWork.CommitAsync();

            // GerenciarFotos(produtoRequest.Fotos, produto.Id);
            foreach (var fotoRequest in produtoRequest.Fotos)
            {

                if (fotoRequest.Id == null || fotoRequest.Id == 0)
                {
                    var produtoFoto = _mapper.Map<ProdutoFoto>(fotoRequest);
                    var uploadFoto = _imageService.Upload(produtoFoto.FotoBytes);

                    produtoFoto.ProdutoId = id;
                    produtoFoto.Caminho = uploadFoto.Url.ToString();
                    produtoFoto.Nome = uploadFoto.PublicId;
                    produtoFoto.FotoBytes = null;

                    _unitOfWork.ProdutoFotoRepository.Create(produtoFoto);
                }
                else if (fotoRequest.FgExcluir)
                {
                    var produtoFoto = await _unitOfWork.ProdutoFotoRepository.GetAsync(f => f.Id == fotoRequest.Id);
                    if (produtoFoto.Nome != null)
                    {
                        _imageService.Delete(produtoFoto.Nome);
                    }
                    _unitOfWork.ProdutoFotoRepository.Delete(produtoFoto);
                }
            }

            await _unitOfWork.CommitAsync();

            var produtoResponse = _mapper.Map<ProdutoResponseDTO>(produtoUp);


            return Ok(produtoResponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoResponseDTO>> Delete(int id)
        {
            if (!ProdutoExists(id))
                throw new APIException("Tamnho não encontrado", StatusCodes.Status404NotFound);

            var produto = await _unitOfWork.ProdutoRepository.GetAsync(t => t.Id == id);
            _unitOfWork.ProdutoRepository.Delete(produto);
            await _unitOfWork.CommitAsync();

            var produtoResponse = _mapper.Map<ProdutoResponseDTO>(produto);

            return Ok(produtoResponse);
        }

        private void validaStruturaDados(IEnumerable<Produto> produtos, Produto produto)
        {
            var validation = new ProdutoValidator(produtos);
            var result = validation.Validate(produto);

            if (!result.IsValid)
            {
                ModelState.Clear();
                result.AddToModelState(ModelState);

                throw new APIException("Dados inconssistentes", StatusCodes.Status422UnprocessableEntity);
            }
        }

        private async void GerenciarFotos(IEnumerable<ProdutoFotoRequestDTO> fotos, int produtoId)
        {

            //var produtoFototyyy = await _unitOfWork.ProdutoFotoRepository.GetAsync(f => f.Id == 1);


            foreach (var fotoRequest in fotos)
            {

                if (fotoRequest.Id == null || fotoRequest.Id == 0)
                {
                    var produtoFoto = _mapper.Map<ProdutoFoto>(fotoRequest);
                    var uploadFoto = _imageService.Upload(produtoFoto.FotoBytes);

                    produtoFoto.ProdutoId = produtoId;
                    produtoFoto.Caminho = uploadFoto.Url.ToString();
                    produtoFoto.Nome = uploadFoto.PublicId;
                    produtoFoto.FotoBytes = null;

                    _unitOfWork.ProdutoFotoRepository.Create(produtoFoto);
                }
                else if (fotoRequest.FgExcluir)
                {


                    var produtoFoto = (await _unitOfWork.ProdutoFotoRepository.GetAllAsync()).Where(f => f.Id == fotoRequest.Id).FirstOrDefault();
                    if (produtoFoto.Nome != null)
                    {
                        _imageService.Delete(produtoFoto.Nome);
                    }
                    
                    _unitOfWork.ProdutoFotoRepository.Delete(produtoFoto);
                }
            }
        }

        private  bool ProdutoExists(int id)
        {
            return _unitOfWork.ProdutoRepository.GetAllAsync().Result.Where(t => t.Id == id).FirstOrDefault() != null;
        }
            
    }
}
