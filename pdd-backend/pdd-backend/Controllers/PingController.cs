using Microsoft.AspNetCore.Mvc;

namespace pdd_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class PingController : BaseController
{
    [HttpGet]
    public IActionResult Ping()
    {
        return Ok("Pong");
    }
}
