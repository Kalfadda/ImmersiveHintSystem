# Immersive Hint System [![CodeFactor](https://www.codefactor.io/repository/github/kalfadda/immersivehintsystem/badge)](https://www.codefactor.io/repository/github/kalfadda/immersivehintsystem)
![Logo](https://i.imgur.com/WDxRJMz.png)
## Overview

The Immersive Hint System is a flexible and easy-to-use framework for integrating AI-powered hints and responses into your Unity game. It maintains consistency with your game's lore and current context, ensuring immersive and relevant AI interactions.

## Features

- Easy integration with OpenAI's GPT models
- Separate management of static and dynamic lore
- Simple game event system for updating context
- Customizable AI personality
- Ensures AI responses stay within the bounds of your game world

## Components

1. **GPTHintSystem**: The main script that handles API requests and manages lore.
2. **GameEventManager**: Manages game events and updates dynamic lore.
3. **StaticLore**: ScriptableObject for storing unchanging game lore.
4. **DynamicLore**: ScriptableObject for storing changing game context and events.

## Installation

1. Clone this repository or download the scripts.
2. Place the scripts in your Unity project's `Assets` folder.
3. Ensure you have the Newtonsoft.Json package installed in your Unity project.

## Setup

1. Create ScriptableObjects:
   - Right-click in Project window > Create > GPT Hint System > Static Lore
   - Right-click in Project window > Create > GPT Hint System > Dynamic Lore

2. Set up Static Lore:
   - Open the StaticLore asset
   - Add entries like "The player resides in the land of enchantment" or "The player is 6 feet tall and has dark hair"

3. Set up the scene:
   - Create an empty GameObject and name it "GPTHintSystem"
   - Add the `GPTHintSystem` component to it
   - Add the `GameEventManager` component to it
   - In the `GPTHintSystem` component:
     - Assign your OpenAI API key
     - Choose the model (default is "gpt-3.5-turbo")
     - Write the AI personality
     - Assign the StaticLore and DynamicLore assets
   - The `GameEventManager` component doesn't need any assignments

## Usage

### Adding Game Events

```csharp
GameEventManager eventManager = FindObjectOfType<GameEventManager>();
eventManager.AddGameEvent("The user found only 3 out of 4 special gems");
```

### Getting AI Responses

```csharp
GPTHintSystem hintSystem = FindObjectOfType<GPTHintSystem>();
string response = await hintSystem.GetResponse("What should I do next?");
Debug.Log(response);
```

## Best Practices

1. Handle the asynchronous nature of `GetResponse` method in your game logic.
2. Keep your API key secure. Consider using Unity's built-in encryption for sensitive data in a production environment.
3. Monitor your API usage to manage costs, especially during testing phases.
4. Consider implementing a caching system for frequently asked questions to reduce API calls.
---
