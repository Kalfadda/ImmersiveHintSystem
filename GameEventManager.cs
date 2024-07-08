using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public GPTHintSystem hintSystem;
    public DynamicLore dynamicLore;

    private void Awake()
    {
        if (hintSystem == null)
        {
            hintSystem = GetComponent<GPTHintSystem>();
        }
        if (hintSystem == null)
        {
            Debug.LogError("GPTHintSystem not found. Please assign it in the inspector or ensure it's on the same GameObject.");
        }
    }

    public void AddGameEvent(string eventDescription)
    {
        dynamicLore.AddEntry(eventDescription);
        hintSystem.AddPrioritizedContext(eventDescription, 1); // You can adjust the priority as needed
    }

    public void RemoveGameEvent(string eventDescription)
    {
        dynamicLore.RemoveEntry(eventDescription);
        // Note: There's no direct way to remove a specific prioritized context in the current setup
        // You might want to add a method in GPTHintSystem to remove a specific prioritized context if needed
    }

    public void ClearGameEvents()
    {
        dynamicLore.ClearEntries();
        hintSystem.ClearPrioritizedContexts();
    }
}