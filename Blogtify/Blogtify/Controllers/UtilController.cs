using Microsoft.AspNetCore.Mvc;

namespace Blogtify.Controllers;

[ApiController]
[Route("[controller]")]
public class UtilController : ControllerBase
{
    [HttpGet("recommend-content")]
    public IActionResult Index()
    {
        return Ok();
    }

    [HttpHead("/health")]
    public IActionResult Health()
    {
        return Ok();
    }
}
