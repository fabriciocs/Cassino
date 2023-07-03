namespace Cassino.Application.Dtos.V1.Pagamentos;

public class Calendario
{
    public DateTime criacao { get; set; }
    public int expiracao { get; set; }
}

public class Devedor
{
    public string cnpj { get; set; }
    public string nome { get; set; }
}

public class Loc
{
    public int id { get; set; }
    public string location { get; set; }
    public string tipoCob { get; set; }
}

public class CriarDepositoDto
{
    public Calendario calendario { get; set; }
    public string txid { get; set; }
    public int revisao { get; set; }
    public Loc loc { get; set; }
    public string location { get; set; }
    public string status { get; set; }
    public Devedor devedor { get; set; }
    public Valor valor { get; set; }
    public string chave { get; set; }
    public string solicitacaoPagador { get; set; }
}

public class Valor
{
    public string original { get; set; }
}