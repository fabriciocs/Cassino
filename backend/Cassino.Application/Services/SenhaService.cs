using Cassino.Application.Contracts;
using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using Cassino.Infra.Repositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Cassino.Application.Services
{
    public class SenhaService : ISenhaService
    {
        public readonly IUsuarioRepository _usuarioRepository;
        public SenhaService(IUsuarioRepository usuarioRepository) {
            _usuarioRepository = usuarioRepository;
        }

        //Metodos de RedefinirSenha
        public async Task<Usuario> EmailExiste(string email)
        {
            var usuario = await _usuarioRepository.ObterPorEmail(email);
            if (usuario != null)
                 return usuario;
            return null;
        }


        public string GerarLinkRedefinicaoSenha(Usuario usuario)
        {
            Guid guid = Guid.NewGuid();
            string codigo = guid.ToString();

            //Salva o codigo no banco atrelado a conta do usuario.
            usuario.CodigoRecuperacaoSenha = codigo;
            _usuarioRepository.Alterar(usuario);

            string urlBase = "https://localhost:7161";
            string link = $"{urlBase}/v1/senha/usuario-senha/alterar-senha-deslogado/codigo={codigo}";
            return link;
        }


        public bool EmailRedefinicaoSenha(string email, string link)
        {
            //Configuração de servidor SMTP Gmail
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("", ""); //Email e Password do remetente

            //Configuração email
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("", "CASSINO"); //Endereço e titulo do remetente
                mail.To.Add(email);
                mail.Subject = "Recuperação de Senha - CASSINO";
                mail.Body = "Esse é um e-mail de recuperação de senha. " +
                    "Caso você não tenha solicitado a recuperação, por favor ignore-o. " +
                    $"Link para criar uma nova senha: {link}";

                smtp.Send(mail);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        //Metodos de AlterarSenhaDeslogado
        public async Task<Usuario?> CodigoExiste(string codigo)
        {
            var usuario = await _usuarioRepository.ObterPorCodigoRecuperacaoSenha(codigo);
            if(usuario != null)
                return usuario;
            return null;
        }


    }
}
