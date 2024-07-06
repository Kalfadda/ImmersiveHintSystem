using UnityEngine;
using System.Text;

public class ContextManager : MonoBehaviour
{
    public GameContext gameContext;

    public void AddContextEntry(string key, string value, float progressPercentage = 0)
    {
        gameContext.contextEntries.Add(new GameContext.ContextEntry
        {
            key = key,
            value = value,
            progressPercentage = progressPercentage
        });
    }

    public void RemoveContextEntry(string key)
    {
        gameContext.contextEntries.RemoveAll(entry => entry.key == key);
    }

    public void UpdateContextEntry(string key, string newValue, float newProgressPercentage)
    {
        var entry = gameContext.contextEntries.Find(e => e.key == key);
        if (entry != null)
        {
            entry.value = newValue;
            entry.progressPercentage = newProgressPercentage;
        }
    }

    public string GetFullContext()
    {
        StringBuilder contextBuilder = new StringBuilder();
        contextBuilder.AppendLine("Game Lore and Rules:");
        foreach (var loreEntry in gameContext.gameLore.loreEntries)
        {
            contextBuilder.AppendLine($"{loreEntry.key}: {loreEntry.description}");
        }
        contextBuilder.AppendLine("\nCurrent Game Context:");
        foreach (var entry in gameContext.contextEntries)
        {
            contextBuilder.AppendLine($"{entry.key}: {entry.value} (Progress: {entry.progressPercentage}%)");
        }
        return contextBuilder.ToString();
    }
}
