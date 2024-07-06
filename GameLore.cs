using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameLore", menuName = "GPT Hint System/Game Lore")]
public class GameLore : ScriptableObject
{
    public List<LoreEntry> loreEntries = new List<LoreEntry>();
}
