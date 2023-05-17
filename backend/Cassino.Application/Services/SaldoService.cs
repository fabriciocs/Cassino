using AutoMapper;
using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Aposta;
using Cassino.Application.Dtos.V1.Saldo;
using Cassino.Application.Notification;
using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassino.Application.Services
{
    public class SaldoService : BaseService, ISaldoService
    {
        private readonly IUsuarioRepository _clienteRepository;
        private readonly IApostaRepository _apostaRepository;
        private readonly IApostaService _apostaService;

        public SaldoService(IMapper mapper, INotificator notificator, IUsuarioRepository clienteRepository, IApostaService apostaService, IApostaRepository apostaRepository) : base(mapper, notificator)
        {
            _clienteRepository = clienteRepository;
            _apostaRepository = apostaRepository;
            _apostaService = apostaService;
        }
        

        public async Task<SaldoUsuarioDto> BuscarSaldo(int id)
        {
            var usuario = await _clienteRepository.ObterPorId(id);
            if (usuario != null)
            {
                return Mapper.Map<SaldoUsuarioDto>(usuario);
            }

            Notificator.HandleNotFoundResource();
            return null;
        }

        public async Task<Boolean> AtualizarSaldo(AdicionarApostaDto apostaDto)
        {
            var usuario = await _clienteRepository.ObterPorId(apostaDto.IdUsuario);
            if (usuario == null)
            {
                Notificator.Handle("Não foi possível encontrar o usuário no banco de dados.");
                return false;
            }

            usuario.Saldo += apostaDto.Valor;
            _clienteRepository.Alterar(usuario);

            if (!await _clienteRepository.UnitOfWork.Commit())
            {
                Notificator.Handle("Não foi possível atualizar o saldo do usuário no banco de dados.");
                return false;
            }

            _apostaService.RegistrarAposta(apostaDto);
            if (!await _apostaRepository.UnitOfWork.Commit())
            {
                Notificator.Handle("Não foi possível registrar a aposta do usuário no banco de dados.");
                return false;
            }
            return true;
        }
    }
}
