using AutoMapper;
using Cassino.Application.Contracts;
using Cassino.Application.Dtos.V1.Pagamentos;
using Cassino.Application.Notification;
using Cassino.Domain.Contracts.Repositories;
using Cassino.Domain.Entities;
using Gerencianet.NETCore.SDK;

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
        var assemblyPath = Path.GetDirectoryName(typeof(DependencyInjection).Assembly.Location);
        var pasta = "\\Certificates\\producao-467170-jogosProducao.p12";
        var path = assemblyPath + pasta;
        // X509Certificate2 uidCert = new X509Certificate2(path, "");
        
        
        dynamic endpoints = new Endpoints("Client_Id_e53549ce91fe086e73d44d9238d3770e2ad08035", "Client_Secret_1485ed3cda1cd75dde165c8578b1aa8f0c63f2c1", false, path);
        
                
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