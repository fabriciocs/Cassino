namespace Cassino.Application.Dtos.V1.Pagamentos;
public class ObterQrCodeDto
{
    public string qrcode { get; set; } = String.Empty;
    public string imagemQrcode { get; set; } = String.Empty;
    public string linkVisualizacao { get; set; } = String.Empty;
}