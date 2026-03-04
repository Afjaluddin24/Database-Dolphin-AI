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

        // Current Date & Time
        var currentDate = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm");

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
                            text = $"You are Dolphin-AI assistant. Current date and time is {currentDate}. Answer the user question clearly.\nUser Question: {question}"
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
            return $"Gemini Error: {responseString}";

        using var doc = JsonDocument.Parse(responseString);

        var answer = doc.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

        return $"🤖 Dolphin-AI\n📅 {currentDate}\n\n{answer}";
    }
}