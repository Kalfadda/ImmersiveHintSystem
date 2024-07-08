using UnityEngine;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ChatCompletionRequest
{
    public string model;
    public List<Message> messages;
}

[Serializable]
public class Message
{
    public string role;
    public string content;
}

[Serializable]
public class ChatCompletionResponse
{
    public Choice[] choices;
}

[Serializable]
public class Choice
{
    public Message message;
}

public class GPTHintSystem : MonoBehaviour
{
    [SerializeField] private string apiKey = "YOUR_API_KEY_HERE";
    [SerializeField] private string model = "gpt-3.5-turbo";
    [SerializeField] private AIPersonality currentPersonality;
    [SerializeField] private StaticLore staticLore;
    [SerializeField] private DynamicLore dynamicLore;
    [SerializeField] private PromptTemplates promptTemplates;
    [SerializeField] private OfflineResponseDatabase offlineResponseDatabase;

    private HttpClient httpClient;
    private const string API_URL = "https://api.openai.com/v1/chat/completions";

    [System.Serializable]
    private class PrioritizedContext
    {
        public string context;
        public int priority;
    }

    private List<PrioritizedContext> prioritizedContexts = new List<PrioritizedContext>();

    void Awake()
    {
        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }

    public async Task<string> GetResponse(string userInput, string emotion = "neutral", string promptTemplate = "default")
    {
        string fullContext = GetFullContext();
        string personalityPrompt = currentPersonality.GetPersonalityPrompt(emotion);
        string prompt = promptTemplates.GetPromptTemplate(promptTemplate)
            .Replace("{PERSONALITY}", personalityPrompt)
            .Replace("{CONTEXT}", fullContext)
            .Replace("{USER_INPUT}", userInput);

        return await SendGPTRequest(prompt);
    }

    private string GetFullContext()
    {
        StringBuilder contextBuilder = new StringBuilder();

        // Add prioritized contexts first
        foreach (var prioritizedContext in prioritizedContexts.OrderByDescending(pc => pc.priority))
        {
            contextBuilder.AppendLine($"[Priority {prioritizedContext.priority}] {prioritizedContext.context}");
        }

        contextBuilder.AppendLine("\nStatic Lore:");
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

    public void AddPrioritizedContext(string context, int priority)
    {
        prioritizedContexts.Add(new PrioritizedContext { context = context, priority = priority });
        // Optional: Sort the list after adding a new context
        prioritizedContexts = prioritizedContexts.OrderByDescending(pc => pc.priority).ToList();
    }

    public void RemovePrioritizedContext(string context)
    {
        prioritizedContexts.RemoveAll(pc => pc.context == context);
    }

    public void ClearPrioritizedContexts()
    {
        prioritizedContexts.Clear();
    }

    private async Task<string> SendGPTRequest(string prompt)
    {
        if (!await CheckInternetConnectivity())
        {
            return GetOfflineResponse(prompt);
        }

        var requestBody = new ChatCompletionRequest
        {
            model = this.model,
            messages = new List<Message>
            {
                new Message { role = "system", content = currentPersonality.basePersonality },
                new Message { role = "user", content = prompt }
            }
        };

        var jsonContent = JsonUtility.ToJson(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await httpClient.PostAsync(API_URL, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                ChatCompletionResponse parsedResponse = JsonUtility.FromJson<ChatCompletionResponse>(responseBody);
                return parsedResponse.choices[0].message.content;
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

    private async Task<bool> CheckInternetConnectivity()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(5);
                var response = await client.GetAsync("https://www.google.com");
                return response.IsSuccessStatusCode;
            }
        }
        catch
        {
            return false;
        }
    }

    private string GetOfflineResponse(string prompt)
    {
        if (offlineResponseDatabase != null)
        {
            return offlineResponseDatabase.GetResponse(prompt);
        }
        return "I'm sorry, I don't have enough information to respond to that right now.";
    }

    // Methods for managing dynamic lore
    public void AddDynamicLore(string entry)
    {
        if (dynamicLore != null)
        {
            dynamicLore.AddEntry(entry);
        }
    }

    public void RemoveDynamicLore(string entry)
    {
        if (dynamicLore != null)
        {
            dynamicLore.RemoveEntry(entry);
        }
    }

    public void ClearDynamicLore()
    {
        if (dynamicLore != null)
        {
            dynamicLore.ClearEntries();
        }
    }
}