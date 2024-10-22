using System.Text.Json;

namespace SurvivalApi.Services;

public class ResponseType
{
    public String? model { get; set; }
    public String? created_at { get; set; }
    public String? response { get; set; }
    public bool done { get; set; }
    public String? done_reason { get; set; }
    public Int64[]? context { get; set; }
    public int total_duration { get; set; }
    public int load_duration { get; set; }
    public int prompt_eval_count { get; set; }
    public Int64 prompt_eval_duration { get; set; }
    public int eval_count { get; set; }
    public Int64 eval_duration { get; set; }

}

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
    public async Task<String> SendToOllama(String prompt)
    {
        HttpClient client = new HttpClient();
        var toSerialize = new OllamaPayload
        {
            prompt = prompt
        };
        var response = await client.PostAsJsonAsync(
            "http://localhost:11434/api/generate", toSerialize);
        var answer = response.Content.ReadAsStringAsync().Result;
        var answerJson = JsonSerializer.Deserialize<ResponseType>(answer);
        var finalAnswer = answerJson.response;
        return finalAnswer;
    }


    public async Task<String> CreateScenario(String[] playerNames)
    {
        var standardPrompt = "create one dangerous scenario threatening the following people:";
        var playersString = string.Join(" ", playerNames);
        var prompt = standardPrompt + playersString + ". Make it under 400 characters, and use their names";
        var response = await SendToOllama(prompt);
        return response;
    }

    public async Task<String> RunScenario(String scenario, String actions)
    {
        var instructions = "Evaluate the prompt and tell us what the outcome is, make it under 400 characters and decide who lives and dies";
        var prompt = instructions + scenario + actions;
        var response = await SendToOllama(prompt);
        return response;
    }

    public async Task TestSendToOllama()
    {      // /api/test/ollama
        var standardPrompt = "create one dangerous scenario, make it under 200 characters";
        var response = await SendToOllama(standardPrompt);
        Console.WriteLine(response);
    }

    public async Task TestCreateScenario()
    {    // /api/test/create
        string[] playerNames = ["Luke", "Raed", "Tommy"];
        var response = await CreateScenario(playerNames);
        Console.WriteLine(response);
    }

    public async Task TestRunScenario()
    {       // /api/test/run
        var scenario = "A powerful explosion rocks Luke's abandoned warehouse, trapping him beneath rubble. As firefighters struggle to free him, Tommy stumbles upon a smoke-filled Tommy enters, revealing that his brother was caught in the blast just moments earlier. The explosive wreckage seals their uncertain fate, casting darkness over their family's tragic past.";
        var answer = "Put on my gas mask and try to escape";
        var response = await RunScenario(scenario, answer);
        Console.WriteLine(response);
    }
}