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

        public async Task<bool> RegistrarAposta(AdicionarApostaDto apostaDto)
        {
            var aposta = Mapper.Map<Aposta>(apostaDto);
            _apostaRepository.Adicionar(aposta);

            if (!await _apostaRepository.UnitOfWork.Commit())
            {
                Notificator.Handle("Não foi possível registrar a aposta do usuário no banco de dados.");
                return false;
            }

            return true;
        }

        public async Task<List<VMApostaDto>?> ObterApostasDeUsuario(int id)
        {
            var usuario = await _usuarioRepository.ObterPorId(id);
            if(usuario != null)
            {
                var lista = await _apostaRepository.ObterPorUsuario(usuario);
                if (lista.Any())
                {
                    var apostasUsuario = Mapper.Map<List<VMApostaDto>?>(lista);
                    return apostasUsuario;
                }
                Notificator.Handle("Lista vazia: Ainda não existem apostas registradas.");
                var listaVazia = Mapper.Map<List<VMApostaDto>?>(lista);
                return listaVazia;
            }
            Notificator.HandleNotFoundResource();
            return null;
        }

        public async Task<List<VMApostaDto>?> ObterTodasApostas()
        {
            var lista = await _apostaRepository.ObterTodas();
            if (lista.Any())
            {
                var apostas = Mapper.Map<List<VMApostaDto>?>(lista);
                return apostas;
            }
                
            Notificator.Handle("Lista vazia: Esse usuario ainda não tem apostas registradas.");
            var listaApostasVazia = Mapper.Map<List<VMApostaDto>?>(lista);
            return listaApostasVazia;
        }
    }
}
