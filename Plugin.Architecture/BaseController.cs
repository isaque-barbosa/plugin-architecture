using Microsoft.AspNetCore.Mvc;

namespace Plugin.Architecture;
[Route("api/[controller]/[action]")]
public class BaseController : Controller
{
    public BaseController()
    {
        
    }

    [HttpGet]
    public async Task<IActionResult> Home()
    {
        await Task.Delay(100);
        return Ok("Home");
    }
}
