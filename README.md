# Immersive Hint System [![CodeFactor](https://www.codefactor.io/repository/github/kalfadda/immersivehintsystem/badge)](https://www.codefactor.io/repository/github/kalfadda/immersivehintsystem)
![Logo](https://i.imgur.com/WDxRJMz.png)

# Documentation

## Table of Contents

1. [Introduction](#introduction)
2. [System Components](#system-components)
3. [Setup and Configuration](#setup-and-configuration)
4. [Usage](#usage)
5. [Examples](#examples)
6. [Use Cases](#use-cases)
7. [Offline Mode](#offline-mode)
8. [Best Practices](#best-practices)

## Introduction

The Immersive Hint System is a flexible and powerful tool designed for Unity games, leveraging OpenAI's GPT models to provide dynamic, context-aware hints and responses to players. This system allows for personalized AI interactions, adapting to the game's current state, player actions, and predefined lore.

Key features include:
- Dynamic personality adjustment
- Context-aware responses
- Integration of static and dynamic lore
- Offline mode support
- Customizable prompt templates

## System Components

The Immersive Hint System consists of several interconnected components:

1. **GPTHintSystem**: The core component that manages API requests, context building, and response generation.

2. **AIPersonality (ScriptableObject)**:
   - Purpose: Defines the AI's base personality and emotional modifiers.
   - Functionality: 
     - Stores a base personality description.
     - Contains a list of emotion modifiers that can alter the AI's responses based on different emotional states.
     - Provides a method to get a personality prompt based on a given emotion.

3. **StaticLore (ScriptableObject)**:
   - Purpose: Contains unchanging background information about the game world.
   - Functionality:
     - Stores a list of lore entries that remain constant throughout the game.
     - Provides a foundation for the AI's knowledge about the game world, characters, and history.

4. **DynamicLore (ScriptableObject)**:
   - Purpose: Stores and manages evolving game events and player actions.
   - Functionality:
     - Maintains a list of entries that can be added, removed, or cleared during gameplay.
     - Allows the system to adapt its responses based on the current game state and player progress.

5. **PromptTemplates (ScriptableObject)**:
   - Purpose: Provides customizable templates for structuring requests to the GPT model.
   - Functionality:
     - Stores a list of named prompt templates.
     - Allows developers to create different templates for various types of interactions (e.g., quest hints, NPC dialogues).
     - Provides a method to retrieve a specific template by name.

6. **OfflineResponseDatabase (ScriptableObject)**:
   - Purpose: Stores pre-defined responses for offline use.
   - Functionality:
     - Contains a list of keyword-response pairs.
     - Provides fallback responses when the system can't connect to the OpenAI API.
     - Allows for basic functionality even without an internet connection.

7. **GameEventManager**: Facilitates the addition and removal of game events to the dynamic lore.

## Setup and Configuration

1. Create the necessary ScriptableObjects:
   - AIPersonality
   - StaticLore
   - DynamicLore
   - PromptTemplates
   - OfflineResponseDatabase

2. Attach the `GPTHintSystem` script to a GameObject in your scene.

3. Configure the `GPTHintSystem` in the Inspector:
   - Set your OpenAI API key
   - Choose the desired GPT model
   - Assign the created ScriptableObjects to their respective fields

4. Set up the `GameEventManager`:
   - Attach it to a GameObject (can be the same as `GPTHintSystem`)
   - Assign the `GPTHintSystem` and `DynamicLore` references in the Inspector

## Usage

### Basic Usage

To get a response from the Immersive Hint System:

```csharp
public class GameManager : MonoBehaviour
{
    public GPTHintSystem hintSystem;

    public async void GetHint(string playerQuestion)
    {
        string response = await hintSystem.GetResponse(playerQuestion);
        DisplayHintToPlayer(response);
    }

    private void DisplayHintToPlayer(string hint)
    {
        // Implement your UI logic here
    }
}
```

### Adding Dynamic Lore

To update the game state and influence AI responses:

```csharp
public class QuestManager : MonoBehaviour
{
    public GameEventManager eventManager;

    public void CompleteQuest(string questName)
    {
        string eventDescription = $"Player completed the quest: {questName}";
        eventManager.AddGameEvent(eventDescription);
    }
}
```

### Changing AI Personality

To adjust the AI's emotional state:

```csharp
public class DialogueManager : MonoBehaviour
{
    public GPTHintSystem hintSystem;

    public async void GetNPCResponse(string playerDialogue, string npcEmotion)
    {
        string response = await hintSystem.GetResponse(playerDialogue, npcEmotion);
        DisplayNPCResponse(response);
    }
}
```

## Examples

### Example 1: Quest Hint

```csharp
string playerQuestion = "Where should I look for the lost artifact?";
string hint = await hintSystem.GetResponse(playerQuestion, "helpful", "quest_hint");
// hint might be: "Based on the ancient texts in the library, the lost artifact is rumored to be hidden in the Whispering Caves to the north of the village."
```

### Example 2: NPC Interaction

```csharp
string playerDialogue = "Can you tell me about the war that happened 100 years ago?";
string npcResponse = await hintSystem.GetResponse(playerDialogue, "somber", "npc_dialogue");
// npcResponse might be: "Ah, the Great Conflict. *sighs* It's not a tale I enjoy recounting. So many lives lost, so much destruction. What specifically would you like to know about those dark times?"
```

## Use Cases

1. **Dynamic Hint System**: Provide players with contextual hints that adapt to their progress and game state.
2. **Intelligent NPCs**: Create more engaging and responsive non-player characters that react to the player's actions and game events.
3. **Adaptive Storytelling**: Generate dynamic narrative elements that respond to player choices and game progression.
4. **Tutorial Assistant**: Offer a smart tutorial system that can answer player questions and provide guidance based on their current situation.
5. **Puzzle Solver Aid**: Give subtle, context-aware clues for complex puzzles without explicitly solving them for the player.

## Offline Mode

The system includes an offline mode for situations where internet connectivity is unavailable:

1. Populate the `OfflineResponseDatabase` with keyword-response pairs.
2. The system automatically switches to offline mode when it can't connect to the OpenAI API.
3. Responses are selected based on keyword matching from the player's input.

Example of populating the OfflineResponseDatabase:

```csharp
offlineResponseDatabase.responses.Add(new OfflineResponseDatabase.OfflineResponse
{
    keyword = "lost artifact",
    response = "The lost artifact is an object of great power. Many have searched for it in the ancient ruins to the east."
});
```

## Best Practices

1. **Maintain Consistency**: Keep your StaticLore, DynamicLore, and AIPersonality consistent to ensure coherent responses.
2. **Optimize API Usage**: Use the prioritized context feature to ensure the most relevant information is included in each request.
3. **Balance Information**: Provide enough context for informative responses, but avoid overloading the system with unnecessary details.
4. **Test Offline Responses**: Ensure your offline database covers common player questions and critical game information.
5. **Update Dynamic Lore**: Regularly update the dynamic lore as the game state changes to keep responses relevant and accurate.
6. **Emotion Variety**: Utilize different emotions in the AIPersonality to create more engaging and varied interactions.
7. **Prompt Engineering**: Refine your prompt templates to guide the AI towards producing the desired style and content of responses.
