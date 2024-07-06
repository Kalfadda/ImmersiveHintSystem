# LoreContextGPT

Immersive Hint System is a Unity-based system that integrates game context and lore with an AI-powered hint system. This project leverages the power of GPT-3.5-turbo to provide immersive, diegetic hints that enhance the player's experience without breaking the fourth wall.

## Features

- **Context Management**: Manage and update game context entries dynamically.
- **Lore Integration**: Seamlessly integrate detailed game lore to maintain consistency.
- **AI-Powered Hints**: Use GPT-3.5-turbo to generate in-game hints based on the current game context and lore.
- **Customizable Personality**: Define the AI's personality to ensure hints match the tone and style of your game.

## Components

### ContextManager

Handles the addition, removal, and updating of context entries, and compiles the full game context for hint generation.

### GameContext

Stores the current game context, including progress tracking and contextual values.

### GameLore

Maintains a list of lore entries, ensuring that all hints and interactions remain consistent with the game's story and setting.

### LoreEntry

Defines individual lore entries with a key and a detailed description.

### GPTHintSystem

Communicates with the OpenAI API to generate hints based on the current game context and lore. Ensures hints are diegetic and relevant to the game world.

## Usage

1. **Setup**: Add the `ContextManager`, `GameContext`, and `GameLore` components to your game objects.
2. **Configuration**: Customize the AI personality and API settings in the `GPTHintSystem`.
3. **Integration**: Use the `GetHint` method to retrieve AI-generated hints based on player queries and the current game state.

## Example

```csharp
// Example of adding a context entry
contextManager.AddContextEntry("Objective", "Find the hidden key in the forest", 25.0f);

// Example of getting a hint
string hint = await gptHintSystem.GetHint("Where should I look for the key?");
Debug.Log(hint);
