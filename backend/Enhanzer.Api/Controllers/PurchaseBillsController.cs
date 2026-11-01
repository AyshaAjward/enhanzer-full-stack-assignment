
using Microsoft.AspNetCore.Mvc;

namespace Enhanzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseBillsController : ControllerBase
{
    [HttpGet("access-check")]
    public IActionResult AccessCheck()
    {
        bool authenticated =
            HttpContext.Session.GetString("IsAuthenticated") == "true";

        if (!authenticated)
        {
            return Unauthorized(new { message = "Please log in first." });
        }

        return Ok(new { message = "You are authenticated." });
    }
}
