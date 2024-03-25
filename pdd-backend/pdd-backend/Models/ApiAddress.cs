using System.Text.Json.Serialization;

namespace pdd_backend.Models;

public class ApiAddress
{
    [JsonPropertyName("display_name")]
    public string display_name { get; set; }
}
