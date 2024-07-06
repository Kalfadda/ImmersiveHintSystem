using UnityEngine;

[CreateAssetMenu(fileName = "LoreEntry", menuName = "GPT Hint System/Lore Entry")]
public class LoreEntry : ScriptableObject
{
    public string key;
    [TextArea(3, 10)]
    public string description;
}
