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
        {
            return "API Key is missing.";
        }

        // Clean Prompt
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = $@"You are Dolphin-AI assistant.
Give short and clear answers.
If the user asks for code, return only the code.

User Question: {question}"
                        }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}"
        );

        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);

        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return $"Gemini Error: {responseString}";
        }

        using var doc = JsonDocument.Parse(responseString);

        var answer = doc.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

        return answer ?? "No response from AI.";
    }
}