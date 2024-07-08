using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DynamicLore", menuName = "GPT Hint System/Dynamic Lore")]
public class DynamicLore : ScriptableObject
{
    [TextArea(3,3)]
    public List<string> entries = new List<string>();

    public void AddEntry(string entry)
    {
        if (!entries.Contains(entry))
        {
            entries.Add(entry);
        }
    }

    public void RemoveEntry(string entry)
    {
        entries.Remove(entry);
    }

    public void ClearEntries()
    {
        entries.Clear();
    }
}