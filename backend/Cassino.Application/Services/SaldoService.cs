using AutoMapper;
using Cassino.Application.Contracts;
using Cassino.Application.Contracts.temp;
using Cassino.Application.Dtos.V1.Aposta;
using Cassino.Application.Dtos.V1.Saldo;
using Cassino.Application.Notification;
using Cassino.Domain.Contracts.Repositories;

namespace Cassino.Application.Services;

public class SaldoService : BaseService, ISaldoService
{
    private readonly IUsuarioRepository _clienteRepository;
    private readonly IApostaService _apostaService;
    private readonly IRendaCasaService _rendaCasaService;

    public SaldoService(IMapper mapper, INotificator notificator, IUsuarioRepository clienteRepository, IApostaService apostaService, IRendaCasaService rendaCasaService) : base(mapper, notificator)
    {
        _clienteRepository = clienteRepository;
        _apostaService = apostaService;
        _rendaCasaService = rendaCasaService;
    }
    
    public async Task<SaldoUsuarioDto?> BuscarSaldo(int id)
    {
        var usuario = await _clienteRepository.ObterPorId(id);
        if (usuario != null)
        {
            return Mapper.Map<SaldoUsuarioDto?>(usuario);
        }

        Notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<SaldoUsuarioDto?> AtualizarSaldo(AdicionarApostaDto apostaDto)
    {
        var usuario = await _clienteRepository.ObterPorId(apostaDto.IdUsuario);
        if (usuario == null)
        {
            Notificator.Handle("Não foi possível encontrar o usuário no banco de dados.");
            return null;
        }

        var casa = await _rendaCasaService.ObterCasa();
        if (!await _rendaCasaService.MovimentacaoRenda(apostaDto, casa))
            return null;

        usuario.Saldo += apostaDto.Valor;
        _clienteRepository.Alterar(usuario);
        if (!await _clienteRepository.UnitOfWork.Commit())
        {
            Notificator.Handle("Não foi possível atualizar o saldo do usuário no banco de dados.");
            return null;
        }

        if (!await _apostaService.RegistrarAposta(apostaDto))
        {
            Notificator.Handle("Não foi possível registrar a aposta do usuário no banco de dados.");
            return null;
        }

        return Mapper.Map<SaldoUsuarioDto?>(usuario);
    }
    
    public async Task AtualizarSaldo(decimal valor, int usuarioId)
    {
        var usuario = await _clienteRepository.ObterPorId(usuarioId);
        if (usuario == null)
        {
            Notificator.Handle("Não foi possível encontrar o usuário no banco de dados.");
            return;
        }

        usuario.Saldo += valor;
        _clienteRepository.Alterar(usuario);
        if (!await _clienteRepository.UnitOfWork.Commit())
        {
            Notificator.Handle("Não foi possível atualizar o saldo do usuário no banco de dados.");
        }
    }
}