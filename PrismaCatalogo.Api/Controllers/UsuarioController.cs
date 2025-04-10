using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using PrismaCatalogo.Api.DTOs.UsuarioDTO;
using PrismaCatalogo.Api.Enuns;
using PrismaCatalogo.Api.Exceptions;
using PrismaCatalogo.Api.Filters;
using PrismaCatalogo.Api.Models;
using PrismaCatalogo.Api.Repositories;
using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Services.Interfaces;
using PrismaCatalogo.Api.Validations;

namespace PrismaCatalogo.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UsuarioController(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDTO>>> Get()
        {
            var usuarios = await _unitOfWork.UsuarioRepository.GetAllAsync();
            
            if (usuarios == null)
            {
                return NotFound();
            }

            var usuarioResponse = _mapper.Map<IEnumerable<UsuarioResponseDTO>>(usuarios);
            return Ok(usuarioResponse);
        }

        [HttpGet("{id}", Name = "ObterUsuario")]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDTO>>> Get(int id)
        {
            var usuario = await _unitOfWork.UsuarioRepository.GetAsync(t => t.Id == id);

            var usuarioResponse = _mapper.Map<UsuarioResponseDTO>(usuario);
            return Ok(usuarioResponse);
        }

        [HttpGet("GetByName/{nome}", Name = "ObterPorNomeUsuario")]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDTO>>> GetByName(string nome)
        {
            var usuario = await _unitOfWork.UsuarioRepository.GetAsync(t => t.Nome == nome);

            var usuarioResponse = _mapper.Map<IEnumerable<UsuarioResponseDTO>>(usuario);
            return Ok(usuarioResponse);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UsuarioResponseDTO>> Post(UsuarioRequestDTO usuarioRequest)
        {
            Usuario usuario = _mapper.Map<Usuario>(usuarioRequest);

            validaStruturaDados((await _unitOfWork.UsuarioRepository.GetAllAsync()), usuario);


            var novoUsuario = _unitOfWork.UsuarioRepository.Create(usuario);
            await _unitOfWork.CommitAsync();

            var usuarioResponse = _mapper.Map<UsuarioResponseDTO>(novoUsuario);

            return Ok(usuarioResponse);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<UsuarioResponseDTO>> Put(int id, UsuarioRequestDTO usuarioRequest)
        {
            var usuario = _mapper.Map<Usuario>(usuarioRequest);

            if (!UsuarioExists(id))
                throw new APIException("Tamnho não encontrado", StatusCodes.Status404NotFound);

            var usuarios = (await _unitOfWork.UsuarioRepository.GetAllAsync()).Where(t => t.Id != id);
            validaStruturaDados(usuarios, usuario);

            usuario.Id = id;
            var usuarioUp = _unitOfWork.UsuarioRepository.Update(usuario);
            await _unitOfWork.CommitAsync();

            var usuarioResponse = _mapper.Map<UsuarioResponseDTO>(usuarioUp);


            return Ok(usuarioResponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UsuarioResponseDTO>> Delete(int id)
        {
            if (!UsuarioExists(id))
                throw new APIException("Tamnho não encontrado", StatusCodes.Status404NotFound);

            var usuario = await _unitOfWork.UsuarioRepository.GetAsync(t => t.Id == id);
            _unitOfWork.UsuarioRepository.Delete(usuario);
            await _unitOfWork.CommitAsync();

            var usuarioResponse = _mapper.Map<UsuarioResponseDTO>(usuario);

            return Ok(usuarioResponse);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<UsuarioTokenResponseDTO> Login(UsuarioLoginRequestDTO usuarioRequest)
        {
            var usuario = await _unitOfWork.UsuarioRepository.GetAsync(u => u.NomeUsuario == usuarioRequest.NomeUsuario && u.Senha == usuarioRequest.Senha);

            if (usuario == null)
                throw new APIException("Usuario ou senha invalidos", StatusCodes.Status404NotFound);

            var nRefheshToken = _tokenService.GenereteRefreshToken();
            var usuarioResponse = _mapper.Map<UsuarioTokenResponseDTO>(usuario);
            usuarioResponse.Token = _tokenService.GenereteToken(usuario);
            usuarioResponse.RefreshToken = nRefheshToken;
            await _tokenService.DeleteRefreshToken(usuario.Id);
            await _tokenService.SaveRefreshToken(usuario.Id, nRefheshToken);

            return usuarioResponse;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Refresh")]
        public async Task<UsuarioTokenResponseDTO> Refresh(string token, string refreToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            var nome = principal.Identity.Name;
            var usua = await _unitOfWork.UsuarioRepository.GetAsync(u => u.NomeUsuario == nome);

            var refreshToken = await _tokenService.GetRefreshToken(usua.Id);

            if (refreshToken != refreToken) {
                throw new APIException("RefreshToken invalido!", StatusCodes.Status403Forbidden);
            }

            var nToken = _tokenService.GenereteToken(principal.Claims);
            var nRefreshToken = _tokenService.GenereteRefreshToken();

            await _tokenService.DeleteRefreshToken(usua.Id);
            await _tokenService.SaveRefreshToken(usua.Id, nRefreshToken);

            var usuarioResponse = _mapper.Map<UsuarioTokenResponseDTO>(usua);
            usuarioResponse.Token = nToken;
            usuarioResponse.RefreshToken = nRefreshToken;

            return usuarioResponse;

        }

        private void validaStruturaDados(IEnumerable<Usuario> usuarios, Usuario usuario)
        {
            var validation = new UsuarioValidator(usuarios);
            var result = validation.Validate(usuario);

            if (!result.IsValid)
            {
                ModelState.Clear();
                result.AddToModelState(ModelState);

                throw new APIException("Dados inconssistentes", StatusCodes.Status422UnprocessableEntity);
            }
        }

        private  bool UsuarioExists(int id)
        {
            return _unitOfWork.UsuarioRepository.GetAllAsync().Result.Where(t => t.Id == id).FirstOrDefault() != null;
        }
            
    }
}
