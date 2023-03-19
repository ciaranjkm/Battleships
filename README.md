# Battleships 
### A console application game built in C# .NET 7 *seperate paragraph*.

## Commands for the setup stage.

The game will have the player place their ships onto the board using a command system. The setup stage's commands are defined in the format below:

```
Action Shipname StartPosition Direction
```

  - `Actions` can be defined as `place` or `move`.

  - `Shipnname` is the name of the ship you want to place or move, entered as the full name `carrief` or starting letter `c`.
  
  - `StartPosition` is the the position of the first cell of your ship in a letter number format, for example `f10`.

  - `Direction` is the direction to place or move your ship.


## Commands for action stage.

The game will ask the player for them to guess a position to target. It will ask for a guess:

```
Guess: 
```

Where the player will type their guess in a letter number format, for example `d6`.
