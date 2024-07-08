using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Prompt Templates", menuName = "GPT Hint System/Prompt Templates")]
public class PromptTemplates : ScriptableObject
{
    public List<PromptTemplate> templates = new List<PromptTemplate>();

    [System.Serializable]
    public class PromptTemplate
    {
        public string name;
        [TextArea(3, 10)]
        public string template;
    }

    public string GetPromptTemplate(string templateName)
    {
        var template = templates.Find(t => t.name.ToLower() == templateName.ToLower());
        if (template != null)
        {
            return template.template;
        }
        return templates.Find(t => t.name.ToLower() == "default").template;
    }
}
