using AutoMapper;
using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Pagamentos;
using Cassino.Application.Notification;
using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;

namespace Cassino.Application.Services;

public class SaqueService : BaseService, ISaqueService
{
    private readonly ISaqueRepository _repository;
    public SaqueService(IMapper mapper, INotificator notificator, ISaqueRepository repository) : base(mapper, notificator)
    {
        _repository = repository;
    }
    
    public async Task<Saque?> Adicionar(SaqueDto saque)
    {

        var extract = Mapper.Map<Saque>(saque);

        extract.Aprovado = false;
        extract.DataSaque = DateTime.Now;
        _repository.Adicionar(extract);

        if (!await _repository.UnitOfWork.Commit())
        {
            Notificator.Handle("Não foi possivel salvar saque no banco de dados");
            return null;
        }

        return extract;
    }

    public Task<Saque?> Alterar(Saque saque)
    {
        throw new NotImplementedException();
    }

    public async Task Confirmar(int id)
    {
        var saque = await _repository.FistOrDefault(c => c.Id == id);

        if (saque is null)
        {
            return;
        }

        saque.Aprovado = true;
        
        // request de saque
        
        // configurações do banco para transferencia pix
    }

    
}