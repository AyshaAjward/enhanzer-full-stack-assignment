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
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    
    public async Task<JsonElement> LoginAsync(string email, string password)
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

        string json = JsonSerializer.Serialize(requestBody);

        using var content = new StringContent(
            json,
            System.Text.Encoding.UTF8,
            "application/json"
        );

        using var response = await _httpClient.PostAsync(ApiUrl, content);

        string responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"HTTP STATUS: {(int)response.StatusCode}");
        Console.WriteLine($"RESPONSE BODY: {responseBody}");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"External API returned HTTP {(int)response.StatusCode}"
            );
        }

        using var document = JsonDocument.Parse(responseBody);
        return document.RootElement.Clone();
    }


}