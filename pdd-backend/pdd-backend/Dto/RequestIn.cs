namespace pdd_backend.Dto;

public class RequestIn
{
    public string MacAddress { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public string? Address { get; set; }
    public string FileId { get; set; }
}
