using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StaticLore", menuName = "GPT Hint System/Static Lore")]
public class StaticLore : ScriptableObject
{
    [TextArea(5, 3)]
    public List<string> entries = new List<string>();
}
