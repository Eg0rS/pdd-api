using Database;
using Database.Interfaces;
using Kafka.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using pdd_backend.Dto;
using pdd_backend.Models;

namespace pdd_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class RequestsController : BaseController
{
    private readonly IConnection connection;
    private readonly IKafkaProducesService kafkaProducesService;

    public RequestsController(IConnection connection, IKafkaProducesService kafkaProducesService)
    {
        this.connection = connection;
        this.kafkaProducesService = kafkaProducesService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRequest([FromBody] RequestIn requestIn)
    {
        if (requestIn == null)
        {
            return BadRequest("RequestOut is empty");
        }

        requestIn.Address ??= await GetAddress(requestIn);

        var queryObject = new QueryObject(
            "INSERT INTO requests (latitude, longitude, address, created_at, file_id, mac_address) VALUES (@latitude, @longitude, @address, @created_at, @file_id, @mac_addres) RETURNING id",
            new
            {
                latitude = requestIn.Latitude,
                longitude = requestIn.Longitude,
                address = requestIn.Address,
                created_at = DateTime.Now,
                file_id = requestIn.FileId,
                mac_addres = requestIn.MacAddress
            });
        var requestId = await connection.CommandWithResponse<int>(queryObject);
        await kafkaProducesService.WriteTraceLogAsync(new { id = requestId, fileId = requestIn.FileId });
        return Ok();
    }

    [HttpGet("{macAddress}")]
    public async Task<IActionResult> GetRequests(string macAddress)
    {
        var queryObject = new QueryObject(
            "SELECT id as Id, latitude as Latitude, longitude as Longitude, address as Address, created_at as CreatedAt, file_id as FileId , mac_address as MacAddress  FROM requests WHERE mac_address = @mac_address",
            new { mac_address = macAddress });
        var requests = await connection.ListOrEmpty<RequestOut>(queryObject);
        if (requests.Count == 0)
        {
            return Ok(requests);
        }
        var queryObject2 = new QueryObject(
            $"SELECT id as Id, request_id as RequestId, file_id as FileId, is_violation as IsViolation, violations as Violations, plate as Plate  FROM resolutions WHERE request_id in ({string.Join(", ", requests.Select(x => x.Id).ToList())})");

        var resolutions = await connection.ListOrEmpty<Resolution>(queryObject2);
        foreach (var request in requests)
        {
            request.Resolution = resolutions.FirstOrDefault(x => x.RequestId == request.Id);
        }

        return Ok(requests);
    }


    private async Task<string> GetAddress(RequestIn requestOut)
    {
        var url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={requestOut.Latitude}&lon={requestOut.Longitude}";
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Referer", "https://nominatim.openstreetmap.org/");
        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var address = JsonConvert.DeserializeObject<ApiAddress>(content);
        return address.display_name;
    }
}
