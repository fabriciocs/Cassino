namespace Cassino.Application.Dtos.V1.Pagamentos;

public class authDto
{
    public string access_token { get; set; } = String.Empty;
    public string token_type { get; set; } = String.Empty;
    public int expires_in { get; set; }
    public string scope { get; set; } = String.Empty;
}