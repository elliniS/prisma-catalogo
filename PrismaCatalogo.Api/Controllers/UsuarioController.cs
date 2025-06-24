using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using PrismaCatalogo.Api.DTOs.CodigoReenviaSenhaDTO;
using PrismaCatalogo.Api.DTOs.UsuarioDTO;
using PrismaCatalogo.Api.Enuns;
using PrismaCatalogo.Api.Exceptions;
using PrismaCatalogo.Api.Filters;
using PrismaCatalogo.Api.Models;
using PrismaCatalogo.Api.Repositories;
using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Services;
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
        private readonly IEmailService _emailService;

        public UsuarioController(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _emailService = emailService;
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
                throw new APIException("Usuario ou senha invalido!", StatusCodes.Status404NotFound);

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

        [AllowAnonymous]
        [HttpPost]
        [Route("EnviaCodigoRedefiniSenha")]
        public async Task<CodigoReenviaSenhaResponseDTO> EnviaCodigoRedefiniSenha(UsuarioLoginRequestDTO usuarioRequestDTO)
        {
            var usua = await _unitOfWork.UsuarioRepository.GetAsync(u => u.NomeUsuario.Equals(usuarioRequestDTO.NomeUsuario));

            if (usua == null)
            {
                throw new APIException("Usuario não encantrado", StatusCodes.Status403Forbidden);
            }
            
            string codigoNumero = new Random().Next(100000, 999999).ToString();

            var codigo = new CodigoReenviaSenha() { 
                Codigo = codigoNumero,
                UsuarioId = usua.Id,
                DtCriado = DateTime.UtcNow
            };

            Email email = new Email()
            {
                Assunto = "Código para redefinir senha",
                Corpo = $"Seu código é: {codigo.Codigo}",
                Destinatario = usua.Email
            };

            var retorno =  await _emailService.SendEmail(email);
            _unitOfWork.CodigoReenviaSenhaRepository.Create(codigo);
             await _unitOfWork.CommitAsync();

            var codigoDTO = _mapper.Map<CodigoReenviaSenhaResponseDTO>(codigo);

            return codigoDTO; 
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("VerificaCodigo")]
        public async Task<CodigoReenviaSenhaResponseDTO> VerificaCodigo(CodigoReenviaSenhaRequestDTO codigoReenviaSenhaRequest)
        {
            var codi = await _unitOfWork.CodigoReenviaSenhaRepository.GetAsync(c => c.UsuarioId == codigoReenviaSenhaRequest.UsuarioId && c.Codigo == codigoReenviaSenhaRequest.Codigo);

            VerificaCodigo(codi);

            var codigoDTO = _mapper.Map<CodigoReenviaSenhaResponseDTO>(codi);

            return codigoDTO;

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AlteraSenha")]
        public async Task<CodigoReenviaSenhaResponseDTO> AlteraSenha(CodigoReenviaSenhaRequestDTO codigoReenviaSenhaRequest)
        {
            var codi = await _unitOfWork.CodigoReenviaSenhaRepository.GetAsync(c => c.UsuarioId == codigoReenviaSenhaRequest.UsuarioId && c.Codigo == codigoReenviaSenhaRequest.Codigo);

            VerificaCodigo(codi);

            var usu = await _unitOfWork.UsuarioRepository.GetAsync(u => u.Id == codigoReenviaSenhaRequest.UsuarioId);

            codi.usado = true;
            usu.Senha = codigoReenviaSenhaRequest.Senha;

            _unitOfWork.UsuarioRepository.Update(usu);
            _unitOfWork.CodigoReenviaSenhaRepository.Update(codi);
            await _unitOfWork.CommitAsync();

            var codigoDTO = _mapper.Map<CodigoReenviaSenhaResponseDTO>(codi);

            return codigoDTO;

        }

        private void VerificaCodigo(CodigoReenviaSenha codigo)
        {
            if (codigo == null)
            {
                throw new APIException("Codigo incorreto", StatusCodes.Status403Forbidden);
            }
            else if (codigo.DtCriado < DateTime.Now.AddMinutes(-10))
            {
                throw new APIException("Codigo expirado", StatusCodes.Status403Forbidden);
            }
            else if (codigo.usado)
            {
                throw new APIException("Codigo Já usado", StatusCodes.Status403Forbidden);
            }
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
