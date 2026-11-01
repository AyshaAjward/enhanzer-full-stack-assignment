
using System.Text.Json;
using Enhanzer.Api.Data;
using Enhanzer.Api.Models;
using Enhanzer.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enhanzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ExternalAuthService _externalAuth;
    private readonly AppDbContext _db;

    public AuthController(ExternalAuthService externalAuth, AppDbContext db)
    {
        _externalAuth = externalAuth;
        _db = db;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Email and password are required." });
        }

        try
        {
            // The service sends the email as Company_Code and Username.
            // The password is sent as API_Body.Pw.
            JsonElement result = await _externalAuth.LoginAsync(
                request.Email.Trim(),
                request.Password
            );

            // Check the external API response before reading user details.
            if (!result.TryGetProperty("Status_Code", out var status) ||
                status.ValueKind != JsonValueKind.Number ||
                status.GetInt32() != 200)
            {
                return Unauthorized(new { message = "Login failed. Please check your credentials." });
            }

            if (!result.TryGetProperty("Response_Body", out var responseBody) ||
                responseBody.ValueKind != JsonValueKind.Array ||
                responseBody.GetArrayLength() == 0)
            {
                return Unauthorized(new { message = "The login service returned an invalid response." });
            }

            JsonElement userData = responseBody[0];

            // The API can return HTTP 200 but still reject the credentials.
            if (userData.TryGetProperty("Doc_Msg", out var docMessage))
            {
                return Unauthorized(new
                {
                    message = docMessage.GetString() ?? "Invalid login details."
                });
            }

            if (!userData.TryGetProperty("User_Code", out var userCodeElement) ||
                !userData.TryGetProperty("User_Locations", out var locationsElement) ||
                locationsElement.ValueKind != JsonValueKind.Array)
            {
                return Unauthorized(new { message = "The login response is missing required user or location information." });
            }

            string userCode = userCodeElement.GetString() ?? "";

            if (string.IsNullOrWhiteSpace(userCode))
            {
                return Unauthorized(new { message = "Unable to verify the user account." });
            }

            // Convert the returned locations into database records.
            var locations = new List<Location>();

            foreach (JsonElement location in locationsElement.EnumerateArray())
            {
                string code = location.TryGetProperty("Location_Code", out var codeElement)
                    ? codeElement.GetString() ?? ""
                    : "";

                string name = location.TryGetProperty("Location_Name", out var nameElement)
                    ? nameElement.GetString() ?? ""
                    : "";

                if (!string.IsNullOrWhiteSpace(code) &&
                    !string.IsNullOrWhiteSpace(name))
                {
                    locations.Add(new Location
                    {
                        Location_Code = code,
                        Location_Name = name,
                        Created_At = DateTime.UtcNow
                    });
                }
            }

            // Save locations only after successful authentication.
            if (locations.Count > 0)
            {
                var existingLocations = await _db.Locations.ToListAsync();
                _db.Locations.RemoveRange(existingLocations);

                await _db.Locations.AddRangeAsync(locations);
                await _db.SaveChangesAsync();
            }

            // Store authenticated session on the server.
            HttpContext.Session.SetString("UserCode", userCode);
            HttpContext.Session.SetString("IsAuthenticated", "true");

            return Ok(new
            {
                message = "Login successful.",
                userCode,
                locations = locations.Select(x => new
                {
                    x.Location_Code,
                    x.Location_Name
                })
            });
        }
        catch (HttpRequestException)
        {
            return StatusCode(502, new
            {
                message = "Unable to contact the external login service. Please try again."
            });
        }
        catch (JsonException)
        {
            return StatusCode(502, new
            {
                message = "The external login service returned an invalid response."
            });
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "An unexpected error occurred during login."
            });
        }
    }

    [HttpGet("session")]
    public IActionResult CheckSession()
    {
        bool authenticated =
            HttpContext.Session.GetString("IsAuthenticated") == "true";

        return Ok(new { authenticated });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok(new { message = "Logged out successfully." });
    }
}
