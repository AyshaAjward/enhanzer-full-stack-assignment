using System.Net.Http.Json;
using System.Text.Json;

namespace Enhanzer.Api.Services;

public class ExternalAuthService
{
    private readonly HttpClient _httpClient;

    private const string ApiUrl =
        "https://ez-staging-api.azurewebsites.net/api/External_Api/POS_Api/Invoke";

    public ExternalAuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<JsonElement> LoginAsync(
        string email,
        string password)
    {
        var requestBody = new
        {
            API_Action = "GetLoginData",
            Device_Id = "D001",
            Sync_Time = "",
            Company_Code = email,
            API_Body = new
            {
                Username = email,
                Pw = password
            }
        };

        var response = await _httpClient.PostAsJsonAsync(
            ApiUrl,
            requestBody
        );

        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"External login API returned {(int)response.StatusCode}: {responseBody}"
            );
        }

        using var document = JsonDocument.Parse(responseBody);

        return document.RootElement.Clone();
    }
}