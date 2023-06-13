// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

public class Account
{
    public string id { get; set; }
    public string name { get; set; }
}

public class AntifraudResponse
{
}

public class BankAccount
{
    public string bank_name { get; set; }
    public string ispb { get; set; }
}

public class Charge
{
    public string id { get; set; }
    public string code { get; set; }
    public string gateway_id { get; set; }
    public int amount { get; set; }
    public int paid_amount { get; set; }
    public string status { get; set; }
    public string currency { get; set; }
    public string payment_method { get; set; }
    public DateTime paid_at { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public bool pending_cancellation { get; set; }
    public Customer customer { get; set; }
    public LastTransaction last_transaction { get; set; }
    public Metadata metadata { get; set; }
}

public class Customer
{
    public string id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string document { get; set; }
    public string document_type { get; set; }
    public string type { get; set; }
    public bool delinquent { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public Phones phones { get; set; }
    public Metadata metadata { get; set; }
}

public class Data
{
    public string id { get; set; }
    public string code { get; set; }
    public int amount { get; set; }
    public string currency { get; set; }
    public List<Item> items { get; set; }
    public Customer customer { get; set; }
    public string status { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public List<Charge> charges { get; set; }
    public Metadata metadata { get; set; }
}

public class GatewayResponse
{
}

public class Item
{
    public string id { get; set; }
    public int amount { get; set; }
    public DateTime created_at { get; set; }
    public string description { get; set; }
    public int quantity { get; set; }
    public string status { get; set; }
    public DateTime updated_at { get; set; }
}

public class LastTransaction
{
    public string transaction_type { get; set; }
    public string pix_provider_tid { get; set; }
    public string qr_code { get; set; }
    public string qr_code_url { get; set; }
    public string end_to_end_id { get; set; }
    public Payer payer { get; set; }
    public DateTime expires_at { get; set; }
    public string id { get; set; }
    public string gateway_id { get; set; }
    public int amount { get; set; }
    public string status { get; set; }
    public bool success { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public GatewayResponse gateway_response { get; set; }
    public AntifraudResponse antifraud_response { get; set; }
    public Metadata metadata { get; set; }
}

public class Metadata
{
}

public class MobilePhone
{
    public string country_code { get; set; }
    public string number { get; set; }
    public string area_code { get; set; }
}

public class Payer
{
    public string name { get; set; }
    public string document { get; set; }
    public string document_type { get; set; }
    public BankAccount bank_account { get; set; }
}

public class Phones
{
    public MobilePhone mobile_phone { get; set; }
}

public class Root
{
    public string id { get; set; }
    public Account account { get; set; }
    public string type { get; set; }
    public DateTime created_at { get; set; }
    public Data data { get; set; }
}

