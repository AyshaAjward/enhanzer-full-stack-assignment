
using Enhanzer.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enhanzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly AppDbContext _db;

    public LocationsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetLocations()
    {
        bool authenticated =
            HttpContext.Session.GetString("IsAuthenticated") == "true";

        if (!authenticated)
        {
            return Unauthorized(new
            {
                message = "Please log in first."
            });
        }

        var locations = await _db.Locations
            .Select(location => new
            {
                location.Location_Code,
                location.Location_Name
            })
            .ToListAsync();

        return Ok(locations);
    }
}
