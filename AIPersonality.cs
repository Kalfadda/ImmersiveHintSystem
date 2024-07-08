using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New AI Personality", menuName = "GPT Hint System/AI Personality")]
public class AIPersonality : ScriptableObject
{
    [TextArea(5,3)]
    public string basePersonality;
    public List<EmotionModifier> emotionModifiers = new List<EmotionModifier>();

    [System.Serializable]
    public class EmotionModifier
    {
        public string emotion;
        [TextArea(5,3)]
        public string modifier;
    }

    public string GetPersonalityPrompt(string emotion)
    {
        var modifier = emotionModifiers.Find(em => em.emotion.ToLower() == emotion.ToLower());
        if (modifier != null)
        {
            return $"{basePersonality} {modifier.modifier}";
        }
        return basePersonality;
    }
}
