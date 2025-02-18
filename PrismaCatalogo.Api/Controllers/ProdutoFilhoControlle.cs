using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrismaCatalogo.Api.DTOs.CorDTO;
using PrismaCatalogo.Api.DTOs.ProdutoFilhoDTO;
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
    public class ProdutoFilhoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProdutoFilhoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoFilhoResponseDTO>>> Get()
        {
            var produtosFilhos = await _unitOfWork.ProdutoFilhoRepository.GetAllAsync();
            
            if (produtosFilhos == null)
            {
                return NotFound();
            }

            var produtoFilhoResponse = _mapper.Map<IEnumerable<ProdutoFilhoResponseDTO>>(produtosFilhos);
            return Ok(produtoFilhoResponse);
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "ObterProdutoFilho")]
        public async Task<ActionResult<IEnumerable<ProdutoFilhoResponseDTO>>> Get(int id)
        {
            var produtoFilho = await _unitOfWork.ProdutoFilhoRepository.GetAsync(t => t.Id == id);

            var produtoFilhoResponse = _mapper.Map<ProdutoFilhoResponseDTO>(produtoFilho);
            return Ok(produtoFilhoResponse);
        }

        [AllowAnonymous]
        [HttpGet("GetByProduto/{id}", Name = "ObterPorProdutoProdutoFilho")]
        public async Task<ActionResult<IEnumerable<ProdutoFilhoResponseDTO>>> GetByProduto(int id)
        {
            var produtosFilhos = await _unitOfWork.ProdutoFilhoRepository.GetListAsync(t => t.ProdutoId == id);

            var produtoFilhoResponse = _mapper.Map<IEnumerable<ProdutoFilhoResponseDTO>>(produtosFilhos);
            return Ok(produtoFilhoResponse);
        }

        [AllowAnonymous]
        [HttpGet("GetByName/{nome}", Name = "ObterPorNomeProdutoFilho")]
        public async Task<ActionResult<IEnumerable<ProdutoFilhoResponseDTO>>> GetByName(string nome)
        {
            var produtoFilho = await _unitOfWork.ProdutoFilhoRepository.GetAsync(t => t.Nome == nome);

            var produtoFilhoResponse = _mapper.Map<IEnumerable<ProdutoFilhoResponseDTO>>(produtoFilho);
            return Ok(produtoFilhoResponse);
        }

        [HttpPost("addTamanhos/{idProduto:int}")]
        public async Task<ActionResult<IEnumerable<ProdutoFilhoResponseDTO>>> AddTamanho(int idProduto, IEnumerable<int> idsTamanhos)
        {
            List<ProdutoFilho> produtosFilhos = new List<ProdutoFilho>();
            Produto produto = await _unitOfWork.ProdutoRepository.GetAsync(pf => pf.Id == idProduto);

            if (produto == null)
                throw new APIException("Produto não encotrado", StatusCodes.Status404NotFound);

            foreach (var idTamnho in idsTamanhos)
            {
                var test = await _unitOfWork.ProdutoFilhoRepository.GetAllAsync();
                var produtosFilhosCor = produto.ProdutosFilhos.Where(pf => pf.CorId != null);

                if(produtosFilhosCor == null || produtosFilhosCor.Count() == 0)
                {
                    ProdutoFilho produtoFilho = new ProdutoFilho()
                    {
                        ProdutoId = idProduto,
                        TamanhoId = idTamnho,
                        QuantEstoque = 0,
                        Ativo = true
                    };

                    produtosFilhos.Add(_unitOfWork.ProdutoFilhoRepository.Create(produtoFilho));
                }
                else
                {
                    if (!produtosFilhosCor.Any(pf => pf.TamanhoId != null))
                    {
                        foreach (var pf in produtosFilhosCor)
                        {
                            pf.TamanhoId = idTamnho;
                            
                            produtosFilhos.Add(_unitOfWork.ProdutoFilhoRepository.Update(pf));
                        }
                    }
                    else
                    {

                        IEnumerable<Cor> cores = (await _unitOfWork.CorRepository.GetAllAsync()).Where(t =>
                            produtosFilhosCor.Select(pf => pf.TamanhoId).Distinct().ToList().Contains(t.Id));

                        foreach(var cor in cores)
                        {
                            ProdutoFilho produtoFilho = new ProdutoFilho() {
                                ProdutoId = idProduto,
                                CorId = cor.Id,
                                TamanhoId = idTamnho,
                                QuantEstoque = 0,
                                Ativo = true
                            };

                            produtosFilhos.Add(_unitOfWork.ProdutoFilhoRepository.Create(produtoFilho));
                            
                        }
                    }
                }
            }

            await _unitOfWork.CommitAsync();
            var produtoFilhoResponse = _mapper.Map<List<ProdutoFilhoResponseDTO>>(produtosFilhos);

            return Ok(produtoFilhoResponse);
        }

        [HttpPost("addCores/{idProduto:int}")]
        public async Task<ActionResult<IEnumerable<ProdutoFilhoResponseDTO>>> AddCor(int idProduto, IEnumerable<int> idsCores)
        {
            List<ProdutoFilho> produtosFilhos = new List<ProdutoFilho>();
            Produto produto = await _unitOfWork.ProdutoRepository.GetAsync(pf => pf.Id == idProduto);

            if (produto == null)
                throw new APIException("Produto não encotrado", StatusCodes.Status404NotFound);

            foreach (var idCor in idsCores)
            {
                var produtosFilhosTamanho = produto.ProdutosFilhos.Where(pf => pf.TamanhoId != null).ToList();

                if (produtosFilhosTamanho == null || produtosFilhosTamanho.Count() == 0)
                {
                    ProdutoFilho produtoFilho = new ProdutoFilho()
                    {
                        Nome = "",
                        ProdutoId = idProduto,
                        CorId = idCor,
                        QuantEstoque = 0,
                        Ativo = true
                    };

                    produtosFilhos.Add(_unitOfWork.ProdutoFilhoRepository.Create(produtoFilho));
                    
                }
                else
                {
                    if (!produtosFilhosTamanho.Any(pf => pf.TamanhoId != null))
                    {
                        foreach (var pf in produtosFilhosTamanho)
                        {
                            pf.CorId = idCor;

                            produtosFilhos.Add(_unitOfWork.ProdutoFilhoRepository.Update(pf));
                            
                        }
                    }
                    else
                    {
                        IEnumerable<Tamanho> Tamanhoes = (await _unitOfWork.TamanhoRepository.GetAllAsync()).Where( t => 
                            produtosFilhosTamanho.Select(pf => pf.TamanhoId).Distinct().ToList().Contains(t.Id)) ;

                        foreach (var Tamanho in Tamanhoes)
                        {
                            ProdutoFilho produtoFilho = new ProdutoFilho()
                            {
                                ProdutoId = idProduto,
                                TamanhoId = Tamanho.Id,
                                CorId = idCor,
                                QuantEstoque = 0,
                                Ativo = true
                            };

                            produtosFilhos.Add(_unitOfWork.ProdutoFilhoRepository.Create(produtoFilho));
                            
                        }
                    }
                }
            }

            await _unitOfWork.CommitAsync();
            var produtosFilhosResponse = _mapper.Map<List<ProdutoFilho>>(produtosFilhos);
            return Ok(produtosFilhosResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoFilhoResponseDTO>> Post(ProdutoFilhoRequestDTO produtoFilhoRequest)
        {
            ProdutoFilho produtoFilho = _mapper.Map<ProdutoFilho>(produtoFilhoRequest);

            validaStruturaDados((await _unitOfWork.ProdutoFilhoRepository.GetListAsync(pf => pf.ProdutoId == produtoFilhoRequest.ProdutoId)), produtoFilho);


            var novoProdutoFilho = _unitOfWork.ProdutoFilhoRepository.Create(produtoFilho);
            await _unitOfWork.CommitAsync();

            foreach (var fotoRequest in produtoFilhoRequest.Fotos)
            {
                var produtoFilhoFoto = _mapper.Map<ProdutoFilhoFoto>(fotoRequest);
                produtoFilhoFoto.ProdutoFilhoId = novoProdutoFilho.Id;
                _unitOfWork.produtoFilhoFotoRepository.Create(produtoFilhoFoto);
            }

            await _unitOfWork.CommitAsync();

            var produtoFilhoResponse = _mapper.Map<ProdutoFilhoResponseDTO>(novoProdutoFilho);

            return Ok(produtoFilhoResponse);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProdutoFilhoResponseDTO>> Put(int id, ProdutoFilhoRequestDTO produtoFilhoRequest)
        {
            var produtoFilho = _mapper.Map<ProdutoFilho>(produtoFilhoRequest);

            if (!ProdutoFilhoExists(id))
                throw new APIException("Tamnho não encontrado", StatusCodes.Status404NotFound);

            var produtosFilhos = await _unitOfWork.ProdutoFilhoRepository.GetListAsync(pf => pf.ProdutoId == produtoFilho.ProdutoId && pf.Id != id);
            validaStruturaDados(produtosFilhos, produtoFilho);

            produtoFilho.Id = id;
            produtoFilho.Fotos = null;
            var produtoFilhoUp = _unitOfWork.ProdutoFilhoRepository.Update(produtoFilho);
            await _unitOfWork.CommitAsync();

            var produtoFilhoResponse = _mapper.Map<ProdutoFilhoResponseDTO>(produtoFilhoUp);

            foreach (var fotoRequest in produtoFilhoRequest.Fotos)
            {

                if (fotoRequest.Id == null || fotoRequest.Id == 0)
                {
                    var produtoFilhoFoto = _mapper.Map<ProdutoFilhoFoto>(fotoRequest);
                    produtoFilhoFoto.ProdutoFilhoId = id;
                    _unitOfWork.produtoFilhoFotoRepository.Create(produtoFilhoFoto);
                }
                else if (fotoRequest.FgExcluir)
                {
                    var produtoFilhoFoto = await _unitOfWork.produtoFilhoFotoRepository.GetAsync(f => f.Id == fotoRequest.Id);
                    _unitOfWork.produtoFilhoFotoRepository.Delete(produtoFilhoFoto);
                }
            }

            await _unitOfWork.CommitAsync();

            return Ok(produtoFilhoResponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoFilhoResponseDTO>> Delete(int id)
        {
            if (!ProdutoFilhoExists(id))
                throw new APIException("Tamnho não encontrado", StatusCodes.Status404NotFound);

            var produtoFilho = await _unitOfWork.ProdutoFilhoRepository.GetAsync(t => t.Id == id);
            _unitOfWork.ProdutoFilhoRepository.Delete(produtoFilho);
            await _unitOfWork.CommitAsync();

            var produtoFilhoResponse = _mapper.Map<ProdutoFilhoResponseDTO>(produtoFilho);

            return Ok(produtoFilhoResponse);
        }

        private void validaStruturaDados(IEnumerable<ProdutoFilho> produtosFilhos, ProdutoFilho produtoFilho)
        {
            var validation = new ProdutoFilhoValidator(produtosFilhos);
            var result = validation.Validate(produtoFilho);

            if (!result.IsValid)
            {
                ModelState.Clear();
                result.AddToModelState(ModelState);

                throw new APIException("Dados inconssistentes", StatusCodes.Status422UnprocessableEntity);
            }
        }

        //private async void GerenciarFotos(IEnumerable<ProdutoFilhoFotoRequestDTO> fotos, int produtoId)
        //{

        //    var produtoFototyyy = await _unitOfWork.ProdutoFotoRepository.GetAsync(f => f.Id == 1);


        //    foreach (var fotoRequest in fotos)
        //    {

        //        if (fotoRequest.Id == null || fotoRequest.Id == 0)
        //        {
        //            var produtoFilhoFoto = _mapper.Map<ProdutoFilhoFoto>(fotoRequest);
        //            produtoFilhoFoto.ProdutoFilhoId = produtoId;
        //            _unitOfWork.produtoFilhoFotoRepository.Create(produtoFilhoFoto);
        //        }
        //        else if (fotoRequest.FgExcluir)
        //        {


        //            var produtoFilhoFoto = (await _unitOfWork.produtoFilhoFotoRepository.GetAllAsync()).Where(f => f.Id == fotoRequest.Id).FirstOrDefault();
        //            _unitOfWork.produtoFilhoFotoRepository.Delete(produtoFilhoFoto);
        //        }
        //    }
        //}

        [HttpDelete("{id:int}")]
        private  bool ProdutoFilhoExists(int id)
        {
            return _unitOfWork.ProdutoFilhoRepository.GetAllAsync().Result.Where(t => t.Id == id).FirstOrDefault() != null;
        }
            
    }
}
