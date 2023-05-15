using AutoMapper;
using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Aposta;
using Cassino.Application.Notification;
using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassino.Application.Services
{
    public class ApostaService : BaseService, IApostaService
    {
        private readonly IApostaRepository _apostaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        public ApostaService(IApostaRepository apostaRepository, IUsuarioRepository usuarioRepository, IMapper mapper, INotificator notificator) : base(mapper, notificator)
        {
            _apostaRepository = apostaRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task RegistrarAposta(AdicionarApostaDto apostaDto)
        {
            var aposta = Mapper.Map<Aposta>(apostaDto);
            await _apostaRepository.Adicionar(aposta);
        }

        public async Task<List<Aposta>> ObterApostasDeUsuario(int id)
        {
            var usuario = await _usuarioRepository.ObterPorId(id);
            if(usuario != null)
            {
                var listaApostasDeUsuario = await _apostaRepository.ObterPorUsuario(usuario);
                return listaApostasDeUsuario;
            }
            return null;
        }

        public async Task<List<Aposta>> ObterTodasApostas()
        {
            return await _apostaRepository.ObterTodas();
        }
    }
}
