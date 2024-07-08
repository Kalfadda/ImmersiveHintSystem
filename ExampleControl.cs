using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class ExampleControl : MonoBehaviour
{
    private GPTHintSystem hintSystem;
    public TextMeshProUGUI hintDisplay;
    public TMP_InputField inputField;
    void Start()
    {
        hintSystem = FindObjectOfType<GPTHintSystem>();
    }

    public async void GenerateNewHintButton()
    {
        string hint = await hintSystem.GetResponse("What do I do next?");
        hintDisplay.text = hint;
    }

    public void SubmitNewLore()
    {
        string input = inputField.text;
        hintSystem.AddDynamicLore(input);
        hintDisplay.text = "New lore added!";
        inputField.text = "";
    }
}
