using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Offline Response Database", menuName = "GPT Hint System/Offline Response Database")]
public class OfflineResponseDatabase : ScriptableObject
{
    public List<OfflineResponse> responses = new List<OfflineResponse>();

    [System.Serializable]
    public class OfflineResponse
    {
        public string keyword;
        [TextArea(3, 10)]
        public string response;
    }

    public string GetResponse(string prompt)
    {
        foreach (var response in responses)
        {
            if (prompt.ToLower().Contains(response.keyword.ToLower()))
            {
                return response.response;
            }
        }
        return "I'm sorry, I don't have enough information to respond to that right now.";
    }
}
