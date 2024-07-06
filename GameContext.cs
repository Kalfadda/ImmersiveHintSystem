using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameContext", menuName = "GPT Hint System/Game Context")]
public class GameContext : ScriptableObject
{
    [System.Serializable]
    public class ContextEntry
    {
        public string key;
        [TextArea(3, 10)]
        public string value;
        [Range(0, 100)]
        public float progressPercentage;
    }

    public GameLore gameLore;
    public List<ContextEntry> contextEntries = new List<ContextEntry>();
}
