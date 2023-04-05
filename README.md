# Battleships
<ins>**A console application game built in C# .NET 6.**</ins>

## Commands:

### Setup Stage:

The game will have the player place their ships onto the board using a command system. The setup stage's commands are defined in the format below:

```
Action Shipname StartPosition Direction
```

  - `Actions` can be defined as `place` or `move`.

  - `Shipnname` is the name of the ship you want to place or move.
  
  - `StartPosition` is the the position of the first cell of your ship in a letter number format, for example `f10`.

  - `Direction` is the direction to place or move your ship.


### Action Stage:

The game will ask the player for them to guess a position to target. It will ask for a guess:

```
Guess: 
```

Where the player will type their guess in a letter number format, for example `d6`.


### Commands Help:

 - All command terms can be entered as the full string or first character. For example the command `place carrier a1 right`
   can be entered as `p c a1 r`.
 - All command terms are **NOT** case sensitive.
