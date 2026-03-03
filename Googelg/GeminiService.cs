using System.Text;
using System.Text.Json;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> AskAI(string question)
    {
        var apiKey = _configuration["GEMINI_API_KEY"];

        if (string.IsNullOrEmpty(apiKey))
            return "API Key is missing.";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = question }
                    }
                }
            }
        };

        var jsonString = JsonSerializer.Serialize(requestBody);

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent"
        );

        request.Headers.Add("x-goog-api-key", apiKey);
        request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        var result = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return $"Gemini Error: {result}";

        using var doc = JsonDocument.Parse(result);

        if (!doc.RootElement.TryGetProperty("candidates", out var candidates))
            return $"Unexpected Response: {result}";

        var answer = candidates[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        return answer ?? "No response";
    }
}