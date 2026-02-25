using Microsoft.AspNetCore.Mvc;

namespace Oasis.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class TestController() : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(new { message = "Hello, World!" });
    }

    [HttpGet("{id:int}")]
    // [EnableQuery]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(new { message = $"You requested ID: {id}" });
    }
}