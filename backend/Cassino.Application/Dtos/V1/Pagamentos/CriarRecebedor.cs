namespace Cassino.Application.Dtos.V1.Pagamentos;

public class AutomaticAnticipationSettings
{
    public bool enabled { get; set; }
    public string type { get; set; }
    public int volume_percentage { get; set; }
    public int delay { get; set; }
}

public class DefaultBankAccount
{
    public string id { get; set; }
    public string holder_name { get; set; }
    public string holder_type { get; set; }
    public string holder_document { get; set; }
    public string bank { get; set; }
    public string branch_number { get; set; }
    public string branch_check_digit { get; set; }
    public string account_number { get; set; }
    public string account_check_digit { get; set; }
    public string type { get; set; }
    public string status { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public Metadata metadata { get; set; }
}

public class GatewayRecipient
{
    public string gateway { get; set; }
    public string status { get; set; }
    public string pgid { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
}

public class Metadata
{
    public string key { get; set; }
}

public class CriarRecebedor
{
    public string id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string document { get; set; }
    public string description { get; set; }
    public string type { get; set; }
    public string status { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public TransferSettings transfer_settings { get; set; }
    public DefaultBankAccount default_bank_account { get; set; }
    public List<GatewayRecipient> gateway_recipients { get; set; }
    public AutomaticAnticipationSettings automatic_anticipation_settings { get; set; }
    public Metadata metadata { get; set; }
}

public class TransferSettings
{
    public bool transfer_enabled { get; set; }
    public string transfer_interval { get; set; }
    public int transfer_day { get; set; }
}