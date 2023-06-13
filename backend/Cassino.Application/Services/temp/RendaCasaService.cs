using AutoMapper;
using Cassino.Application.Contracts.temp;
using Cassino.Application.Dtos.V1.Aposta;
using Cassino.Application.Notification;
using Cassino.Domain.Contracts.Repositories.temp;
using Cassino.Domain.Entities.temp;

namespace Cassino.Application.Services.temp;

public class RendaCasaService : BaseService, IRendaCasaService
{
    private readonly IRendaCasaRepository _rendaCasaRepository;
    public RendaCasaService(IMapper mapper, INotificator notificator, IRendaCasaRepository rendaCasaRepository) : base(mapper, notificator)
    {
        _rendaCasaRepository = rendaCasaRepository;
    }

    public async Task Adicionar(decimal valor)
    {
        var renda = new Renda();
        renda.Valor = valor;
        _rendaCasaRepository.Adicionar(renda);
        if (!await _rendaCasaRepository.UnitOfWork.Commit())
        {
            Notificator.Handle("Deu errado amigo!");
        }
    }

    public async Task<bool> MovimentacaoRenda(AdicionarApostaDto apostaDto, Renda renda)
    {
        if (apostaDto.EhApostaInicial)
        {
            renda.Valor += apostaDto.Valor;
            _rendaCasaRepository.AtualizarSaldoCasa(renda);
            if(!await _rendaCasaRepository.UnitOfWork.Commit())
            {
                Notificator.Handle("Houver um problema ao atualizar o saldo da casa.");
                return false;
            }
            return true;
        }

        if (!apostaDto.EhApostaInicial)
        {
            renda.Valor -= apostaDto.Valor;
            if (renda.Valor <= 0)
            {
                Notificator.Handle("A casa quebrou! FUDEU");
                return false;
            }
            
            _rendaCasaRepository.AtualizarSaldoCasa(renda);
            if (!await _rendaCasaRepository.UnitOfWork.Commit())
            {
                Notificator.Handle("Houver um problema ao atualizar o saldo da casa.");
                return false;
            }
            return true;
        }
        return false;
    }

    public Renda ObterCasa()
    {
        return _rendaCasaRepository.ObterCasa();
    }

    public decimal ObterRendaCasa()
    {
        var casa = ObterCasa();
        return casa.Valor;
    }
}