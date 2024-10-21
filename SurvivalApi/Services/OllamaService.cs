
using System.Text.Json;

namespace SurvivalApi.Services;


public class OllamaPayload
{
    public string model { get; set; } = "llama3.2";
    public Options options { get; set; } = new Options();
    public bool stream { get; set; } = false;
    public string? prompt { get; set; }
}

public class Options
{
    public float temperature { get; set; } = 4.0F;
}

public class OllamaService
{
    
    public async Task<int> SendToOllama(String prompt) {
        HttpClient client = new HttpClient();
        var toSerialize = new OllamaPayload{
            prompt = prompt
        };
        var payload = JsonSerializer.Serialize(toSerialize);
        Console.WriteLine(payload);
        var res = await client.PostAsJsonAsync(
            "http://localhost:11434/api/generate", toSerialize); 
        var answer = res.Content.ReadAsStringAsync().Result;
        Console.WriteLine(answer);
        return 1;
    }

    public String CleanOllamaResponse(String res) {
        return res;
    }

    public async Task CreateScenario(String[] playerNames) {
        var standardPrompt = "create a dangerous scenario involving the following people:";
        var playersString = string.Join(" ", playerNames);
        var prompt = standardPrompt + playersString;
        await SendToOllama(prompt);
    }

    public async Task RunScenario(String scenario) {
        var instructions = "Evaluate the prompt and tell us what the outcome is";
        var prompt = instructions + scenario;
        await SendToOllama(prompt);
    }

    public async Task TestSendToOllama() {
        var standardPrompt = "create a dangerous scenario involving the following people:";
        await SendToOllama(standardPrompt);
    }
}