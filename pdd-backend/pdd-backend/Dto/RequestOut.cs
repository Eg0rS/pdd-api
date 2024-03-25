namespace pdd_backend.Dto;

public class RequestOut
{
    public int Id { get; set; }
    public string MacAdress { get; set; }
    public string Longitude { get; set; }
    public string Latitude { get; set; }
    public string Adress { get; set; }
    public string FileId { get; set; }
    public Resolution Resolution { get; set; }
}
