using AutoMapper;
using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Senha;
using Cassino.Application.Notification;
using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using Org.BouncyCastle.Utilities;
using RazorLight;
using System.Security.Claims;

namespace Cassino.Application.Services
{
    public class SenhaService : BaseService, ISenhaService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        public SenhaService(IUsuarioRepository usuarioRepository, IPasswordHasher<Usuario> passwordHasher, INotificator notificator, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration config) : base(mapper, notificator)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }


        //Metodos de SolicitarRedefinicaoSenha
        public async Task<bool> Solicitar(string email)
        {
            var usuario = await EmailExiste(email);
            if (usuario == null)
                return false;
            var usuarioPreenchido = await GerarCodigoRedefinicaoSenha(usuario);
            if (usuarioPreenchido == null)
                return false;
            var EmailFoiEnviado = await EmailRedefinicaoSenha(usuarioPreenchido);
            if (EmailFoiEnviado)
                return true;
            return false;
        }


        public async Task<Usuario> EmailExiste(string email)
        {
            var usuario = await _usuarioRepository.ObterPorEmail(email);
            if (usuario != null)
                 return usuario;
            Notificator.HandleNotFoundResource();
            return null;
        }


        public async Task<Usuario?> GerarCodigoRedefinicaoSenha(Usuario usuario)
        {
            Guid guid = Guid.NewGuid();
            string codigo = guid.ToString();
            DateTime tempoExpiracaoCodigo = DateTime.UtcNow.AddHours(3);
            
            //Salva o codigo e o timeStamp no banco atrelado a conta do usuario. 
            usuario.CodigoRecuperacaoSenha = codigo;
            usuario.TempoExpiracaoDoCodigo = tempoExpiracaoCodigo;
            _usuarioRepository.Alterar(usuario);
            if(!await _usuarioRepository.UnitOfWork.Commit())
            {
                Notificator.Handle("Ocorreu um problema ao salvar Codigo de Recuperação no banco de dados.");
                return null;
            }

            return usuario;
        }


        public async Task<bool> EmailRedefinicaoSenha(Usuario usuarioPreenchido)
        {
            #region Configuração do Template Email
            var assemblyPath = Path.GetDirectoryName(typeof(DependencyInjection).Assembly.Location);
            var pasta = "\\EmailTemplate";
            var path = assemblyPath + pasta;

            //string baseDirectoryPath = Directory.GetCurrentDirectory();
            //string cut = "Cassino.Api";
            //int cutIndex = baseDirectoryPath.IndexOf(cut, StringComparison.Ordinal);
            //string pathBase = baseDirectoryPath.Substring(0, cutIndex);
            //string path = pathBase + "Cassino.Core\\EmailTemplate";

            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(path)
                .UseMemoryCachingProvider()
                .Build();

            var modeloEmail = new ModeloEmailDto
            {
                Nome = usuarioPreenchido.Nome,
                Codigo = usuarioPreenchido.CodigoRecuperacaoSenha,
                Url = _config.GetSection("RedefinirPageUrl").Value,
                ExpiracaoEmHoras = 3
            };
           
            string template = await engine.CompileRenderAsync("TemplateEmailResetarSenha.cshtml", modeloEmail);
            #endregion


            #region Configuração E-mail
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailRemetenteUsername").Value));
            email.To.Add(MailboxAddress.Parse(usuarioPreenchido.Email));
            email.Subject = "BigBet - Redefinição de Senha";
            email.Body = new TextPart(TextFormat.Html) { Text = template };
            #endregion

            #region Configuração de servidor SMTP Gmail
            using var smtp = new SmtpClient();
            try
            {
                smtp.Connect(_config.GetSection("EmailProvedor").Value, int.Parse(_config.GetSection("EmailPort").Value), SecureSocketOptions.StartTls);
                byte[] emailSenhaBytes = Convert.FromBase64String(_config.GetSection("EmailRemetenteSenha").Value);
                string emailSenha = System.Text.Encoding.UTF8.GetString(emailSenhaBytes);
                smtp.Authenticate(_config.GetSection("EmailRemetenteUsername").Value, emailSenha);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                Notificator.Handle("Ocorreu um problema ao tentar enviar o e-mail de redefinição de senha. " + ex.Message);
                return false;
            }
            return true;
            #endregion
        }


        //Metodos de RedefinirSenha
        public async Task<bool> Redefinir(string codigo, AlterarSenhaDto novaSenha)
        {
            var usuario = await CodigoExiste(codigo);
            if (usuario == null)
                return false;
            if (!VerificarSenha(novaSenha))
                return false;
            if (await SalvarNovaSenha(usuario, novaSenha))
                return true;
            return false;
        }

        public async Task<Usuario?> CodigoExiste(string codigo)
        {
            var usuario = await _usuarioRepository.ObterPorCodigoRecuperacaoSenha(codigo);
            if(usuario != null)
            {
                if(DateTime.UtcNow > usuario.TempoExpiracaoDoCodigo)
                {
                    //Apagando codigo e timeStamp do usuario.
                    Notificator.Handle("O tempo do código de redefinição expirou.");

                    usuario.CodigoRecuperacaoSenha = null;
                    usuario.TempoExpiracaoDoCodigo = null;

                    _usuarioRepository.Alterar(usuario);
                    if (!await _usuarioRepository.UnitOfWork.Commit())
                    {
                        Notificator.Handle("Ocorreu um problema ao atualizar os dados do usuario no banco de dados.");
                        return null;
                    }
                    return null;
                }

                return usuario;
            }
            
            Notificator.HandleNotFoundResource();
            return null;
        }


        public bool VerificarSenha(AlterarSenhaDto novaSenha)
        {
            if (novaSenha.NovaSenha == novaSenha.ConfirmarNovaSenha)
                return true;
            Notificator.Handle("Senha e confirmação de senha não são iguais.");
            return false;
        }


        public async Task<bool> SalvarNovaSenha(Usuario usuario, AlterarSenhaDto alterarSenha)
        {
            usuario.CodigoRecuperacaoSenha = null;
            usuario.TempoExpiracaoDoCodigo = null;

            usuario.Senha = alterarSenha.NovaSenha;
            usuario.Senha = _passwordHasher.HashPassword(usuario, usuario.Senha);
            _usuarioRepository.Alterar(usuario);
            if (await _usuarioRepository.UnitOfWork.Commit())
            {
                return true;
            }
            Notificator.Handle("Ocorreu um problema ao salvar nova senha no banco.");
            return false;
        }


        public async Task<bool> AlterarSenhaLogin(string senhaAntiga, AlterarSenhaDto alterarSenhaDto)
        {
            //Buscando usuario por meio do HTTPContext
            var usuarioId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if(usuarioId == null)
                return false;
            var usuario = await _usuarioRepository.ObterPorId(Int32.Parse(usuarioId));

            //Comparando senha antiga com a passada.
            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Senha, senhaAntiga);
            if (resultado == PasswordVerificationResult.Failed)
            {
                Notificator.Handle("Senha incorreta.");
                return false;
            }

            //Comparando as duas novas senhas (senha e confirmar senha)
            if (alterarSenhaDto.NovaSenha != alterarSenhaDto.ConfirmarNovaSenha)
            {
                Notificator.Handle("Senha e confirmação de senha não são iguais.");
                return false;
            }
            
            usuario.Senha = _passwordHasher.HashPassword(usuario, alterarSenhaDto.NovaSenha);
            _usuarioRepository.Alterar(usuario);
            if (!await _usuarioRepository.UnitOfWork.Commit())
            {
                Notificator.Handle("Ocorreu um problema ao salvar nova senha no banco.");
                return false;
            }
            return true;
        }
    }
}
