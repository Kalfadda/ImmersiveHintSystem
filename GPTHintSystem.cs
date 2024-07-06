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
    [SerializeField] private string apiUrl = "https://api.openai.com/v1/chat/completions";
    [SerializeField] private string model = "gpt-3.5-turbo"; // Add this line
    [SerializeField] [TextArea(3,5)] private string personality;

    private HttpClient httpClient;
    private ContextManager contextManager;

    void Awake()
    {
        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        contextManager = GetComponent<ContextManager>();
        if (contextManager == null)
        {
            contextManager = gameObject.AddComponent<ContextManager>();
        }
    }

    public async Task<string> GetHint(string userQuery)
    {
        string gameContext = contextManager.GetFullContext();
        string prompt = $"{gameContext}\n\nPlayer question: {userQuery}\n\nProvide a hint that is consistent with the game's lore and current context. The hint should be diegetic (existing within the game world) and should not break the fourth wall or reference real-world concepts outside the game universe:";
        return await SendGPTRequest(prompt);
    }

    private async Task<string> SendGPTRequest(string prompt)
    {
        var requestBody = new
        {
            model = this.model, // Use the model field here
            messages = new[]
            {
                new { role = "system", content = personality +  " and Never ever break the fourth wall or reference concepts outside the game world. You only provide information that you know and exists in the Game Context or Game Lore" },
                new { role = "user", content = prompt }
            }
        };

        var jsonContent = JsonConvert.SerializeObject(requestBody); // Use JsonConvert instead of JsonUtility
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                JObject jsonResponse = JObject.Parse(responseBody);
                return jsonResponse["choices"][0]["message"]["content"].ToString();
            }
            else
            {
                Debug.LogError($"Error: {response.StatusCode}, {responseBody}");
                return "Sorry, I couldn't generate a hint at this time.";
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Exception: {e.Message}");
            return "An error occurred while generating the hint.";
        }
    }
}