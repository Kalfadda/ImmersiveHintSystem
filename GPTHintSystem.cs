using UnityEngine;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class GPTHintSystem : MonoBehaviour
{
    [SerializeField] private string apiKey = "YOUR_API_KEY_HERE";
    [SerializeField] private string model = "gpt-3.5-turbo";
    [SerializeField][TextArea(3, 10)] private string aiPersonality = "You are a helpful assistant providing information about the game world.";
    [SerializeField] private StaticLore staticLore;
    [SerializeField] private DynamicLore dynamicLore;

    private HttpClient httpClient;
    private const string API_URL = "https://api.openai.com/v1/chat/completions";

    void Awake()
    {
        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }

    public async Task<string> GetResponse(string userInput)
    {
        string fullContext = GetFullContext();
        string prompt = $"{fullContext}\n\nPlayer input: {userInput}\n\nProvide a response that is consistent with the game's lore and current context. The response should be diegetic (existing within the game world) and should not break the fourth wall or reference real-world concepts outside the game universe:";
        return await SendGPTRequest(prompt);
    }

    private string GetFullContext()
    {
        StringBuilder contextBuilder = new StringBuilder();
        contextBuilder.AppendLine("Static Lore:");
        foreach (var entry in staticLore.entries)
        {
            contextBuilder.AppendLine($"- {entry}");
        }
        contextBuilder.AppendLine("\nDynamic Lore and Game Events:");
        foreach (var entry in dynamicLore.entries)
        {
            contextBuilder.AppendLine($"- {entry}");
        }
        return contextBuilder.ToString();
    }

    private async Task<string> SendGPTRequest(string prompt)
    {
        var requestBody = new
        {
            model = this.model,
            messages = new[]
            {
                new { role = "system", content = aiPersonality },
                new { role = "user", content = prompt }
            }
        };

        var jsonContent = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await httpClient.PostAsync(API_URL, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                JObject jsonResponse = JObject.Parse(responseBody);
                return jsonResponse["choices"][0]["message"]["content"].ToString();
            }
            else
            {
                Debug.LogError($"Error: {response.StatusCode}, {responseBody}");
                return "Sorry, I couldn't generate a response at this time.";
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Exception: {e.Message}");
            return "An error occurred while generating the response.";
        }
    }
    // New methods for managing dynamic lore
    public void AddDynamicLore(string entry)
    {
        dynamicLore.AddEntry(entry);
    }

    public void RemoveDynamicLore(string entry)
    {
        dynamicLore.RemoveEntry(entry);
    }

    public void ClearDynamicLore()
    {
        dynamicLore.ClearEntries();
    }
}