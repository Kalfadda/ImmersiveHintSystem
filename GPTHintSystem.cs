using UnityEngine;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

    [Serializable]
    private class Message
    {
        public string role;
        public string content;
    }

    [Serializable]
    private class ChatRequest
    {
        public string model;
        public Message[] messages;
    }

    [Serializable]
    private class ChatResponse
    {
        public Choice[] choices;
    }

    [Serializable]
    private class Choice
    {
        public Message message;
    }

    private async Task<string> SendGPTRequest(string prompt)
    {
        var chatRequest = new ChatRequest
        {
            model = this.model,
            messages = new Message[]
            {
                new Message { role = "system", content = aiPersonality },
                new Message { role = "user", content = prompt }
            }
        };

        var jsonContent = JsonUtility.ToJson(chatRequest);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await httpClient.PostAsync(API_URL, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                ChatResponse chatResponse = JsonUtility.FromJson<ChatResponse>(responseBody);
                return chatResponse.choices[0].message.content;
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

    // Methods for managing dynamic lore
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
