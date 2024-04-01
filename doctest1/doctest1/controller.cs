using Microsoft.AspNetCore.Mvc;

namespace doctest1;

[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{
    public ApiController()
    {
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await test.test1();
        return Ok("1231");
    }
}