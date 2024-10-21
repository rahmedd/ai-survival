
using System.Text.Json;

namespace SurvivalApi.Services;

public class ResponseType
{
    public String? model { get; set; }
    public String? created_at { get; set; }
    public String? response { get; set; }
    public bool done { get; set; }
    /*public String? done_reason { get; set; }
    public Int64[]? context { get; set; }
    public int total_duration { get; set; }
    public int load_duration { get; set; }
    public int prompt_eval_count { get; set; }
    public Int64 prompt_eval_duration { get; set; }
    public int eval_count { get; set; }
    public Int64 eval_duration { get; set; }
    */
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
    
    public async Task<String> SendToOllama(String prompt) {
        HttpClient client = new HttpClient();
        var toSerialize = new OllamaPayload{
            prompt = prompt
        };
        var payload = JsonSerializer.Serialize(toSerialize);
        var res = await client.PostAsJsonAsync(
            "http://localhost:11434/api/generate", toSerialize); 
        var answer = res.Content.ReadAsStringAsync().Result;
        var split = "done_reason";
        if (answer.IndexOf(split) != -1){
            string substring = answer.Substring(0, answer.IndexOf(split) - 2) + "}";
            var answerJson = JsonSerializer.Deserialize<ResponseType>(substring);
            var finalAnswer = answerJson.response;
            return finalAnswer;
        }
        else {
            Console.WriteLine("There was an error with the model");
            return "There is a fire in the building";
        }
    }

    public String CleanOllamaResponse(String res) {
        return res;
    }

    public async Task CreateScenario(String[] playerNames) {
        var standardPrompt = "create one dangerous scenario involving the following people:";
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
        var standardPrompt = "create one dangerous scenario, make it under 200 characters";
        var prompt = await SendToOllama(standardPrompt);
        Console.WriteLine(prompt);
    }
}