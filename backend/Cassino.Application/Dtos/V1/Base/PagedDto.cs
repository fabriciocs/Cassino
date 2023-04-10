using Cassino.Domain.Contracts;
using Cassino.Domain.Contracts.Paginacao;

namespace Cassino.Application.Dtos.V1.Base;

public class PagedDto<T> : IResultadoPaginado<T>, IViewModel
{
    public IList<T> Itens { get; set; }
    public IPaginacao Paginacao { get; set; }

    public PagedDto()
    {
        Itens = new List<T>();
        Paginacao = new PaginacaoDto();
    }
}