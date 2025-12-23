# Pokemon Game

A Pokemon-style battle game built with MonoGame.

## Setup Instructions

### Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code with C# extension

### Building the Game

1. Open the solution file `pokemon_game.sln` in Visual Studio
2. Restore NuGet packages (should happen automatically)
3. Build the solution (F6 or Build > Build Solution)
4. Run the game (F5 or Debug > Start Debugging)

Alternatively, from the command line:

```bash
cd pokemon_game
dotnet build
dotnet run
```

## Game Controls

### Movement (Overworld)

- **Arrow Keys** or **WASD**: Move your character around the map
- Walk into NPCs or trainers to trigger battles

### Battle System

#### Positioning Phase

When a battle starts, you'll enter the positioning phase to place your Pokemon on the board:

1. **LEFT/RIGHT Arrow Keys**: Select which Pokemon to position
2. **UP/DOWN Arrow Keys**: Choose which row (1-7) to place the selected Pokemon
3. **ENTER**: Confirm positions and start the battle once all Pokemon are placed
4. Pokemon must be placed in the leftmost column (Column 1) of the 7x7 board

**Tips:**

- Each Pokemon must be placed in a different row
- The positioning counter shows how many Pokemon you've placed
- Selected Pokemon is highlighted in yellow

#### Fighting Phase

- **F9**: Win the battle (placeholder for testing)
- Battle mechanics are currently in development

## Development Notes

### Next Steps

- Add in NPC AI: NPCs will walk directly on the X or Y axis if player is near them
- Implement full battle mechanics
- Add attack animations and damage calculations
