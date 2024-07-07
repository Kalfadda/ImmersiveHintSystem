using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    private GPTHintSystem hintSystem;

    void Awake()
    {
        hintSystem = GetComponent<GPTHintSystem>();
        if (hintSystem == null)
        {
            Debug.LogError("GPTHintSystem not found on the same GameObject as GameEventManager.");
        }
    }

    public void AddGameEvent(string eventDescription)
    {
        hintSystem.AddDynamicLore(eventDescription);
    }

    public void RemoveGameEvent(string eventDescription)
    {
        hintSystem.RemoveDynamicLore(eventDescription);
    }

    public void ClearGameEvents()
    {
        hintSystem.ClearDynamicLore();
    }
}