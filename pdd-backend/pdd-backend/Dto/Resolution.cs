namespace pdd_backend.Dto;

public class Resolution
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public string FileId { get; set; }
    public string IsViolation { get; set; }
    public string Violations { get; set; }
    public string Plate { get; set; }
}
