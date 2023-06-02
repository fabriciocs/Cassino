using AutoMapper;
using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Aposta;
using Cassino.Application.Notification;
using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;

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

        public void RegistrarAposta(AdicionarApostaDto apostaDto)
        {
            var aposta = Mapper.Map<Aposta>(apostaDto);
            _apostaRepository.Adicionar(aposta);
        }

        public async Task<List<Aposta>> ObterApostasDeUsuario(int id)
        {
            var usuario = await _usuarioRepository.ObterPorId(id);
            if(usuario != null)
            {
                var listaApostasDeUsuario = await _apostaRepository.ObterPorUsuario(usuario);
                if (listaApostasDeUsuario.Count != 0)
                    return listaApostasDeUsuario;
                Notificator.Handle("Lista vazia: Ainda não existem apostas registradas.");
                return listaApostasDeUsuario;
            }
            Notificator.HandleNotFoundResource();
            return null;
        }

        public async Task<List<Aposta>> ObterTodasApostas()
        {
            var listaApostas = await _apostaRepository.ObterTodas();
            if (listaApostas.Count != 0)
                return listaApostas;
            Notificator.Handle("Lista vazia: Esse usuario ainda não tem apostas registradas.");
            return listaApostas;
        }
    }
}
