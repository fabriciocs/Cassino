namespace Cassino.Application.Dtos.V1.Pagamentos;

using System;
using System.Collections.Generic;

public class Item
{
    public string id { get; set; }
    public string type { get; set; }
    public string description { get; set; }
    public decimal amount { get; set; }
    public int quantity { get; set; }
    public string status { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}

public class MobilePhone
{
    public string country_code { get; set; }
    public string number { get; set; }
    public string area_code { get; set; }
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
    public MobilePhone phones { get; set; }
}

public class LastTransaction
{
    public string pix_provider_tid { get; set; }
    public string qr_code { get; set; }
    public string qr_code_url { get; set; }
    public DateTime expires_at { get; set; }
    public string id { get; set; }
    public string transaction_type { get; set; }
    public string gateway_id { get; set; }
    public decimal amount { get; set; }
    public string status { get; set; }
    public bool success { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public Dictionary<string, object> gateway_response { get; set; }
    public Dictionary<string, object> antifraud_response { get; set; }
    public Dictionary<string, object> metadata { get; set; }
}

public class Charge
{
    public string id { get; set; }
    public string code { get; set; }
    public string gateway_id { get; set; }
    public decimal amount { get; set; }
    public string status { get; set; }
    public string currency { get; set; }
    public string payment_method { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public Customer customer { get; set; }
    public LastTransaction last_transaction { get; set; }
}

public class PixResponseDto
{
    public string id { get; set; }
    public string code { get; set; }
    public decimal amount { get; set; }
    public string currency { get; set; }
    public bool closed { get; set; }
    public List<Item> items { get; set; }
    public Customer customer { get; set; }
    public string status { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public List<Charge> charges { get; set; }
}

public class PixDto
{
    public string qr_code { get; set; }
    public string qr_code_url { get; set; }
    public string status { get; set; }
    public string id { get; set; }
    public DateTime expires_at { get; set; }
    public decimal amount { get; set; }
}