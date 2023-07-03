# Simple-Neural-Network

A library for cool tree search algorithms like monte carlo tree search or minimax.

You can try it out in your browser [here](https://duzychri.github.io/projects/tree-search-ai).

## How to use

First initialize a MCTS ai with the appropriate settings.

This requires an appropriate ```CalculateScore``` function. This function should return higher values for better board states or outcomes. For example: 1 for a win, 0.5 for a draw, 0 for a loss.

```csharp
var settings = new MonteCarloTreeSearcherSettings<GameState, GameAction>()
{
    ExplorationConfidence = 1.4,
    CalculateScore = (boardState) => { ... },
    ...
};
var mcts = new MonteCarloTreeSearcher<GameState, GameAction>(settings);
```

Then you can use it to find the best action for a given game state.

```csharp
GameState gameState = ...;
GameAction nextMove = mcts.GetBestMove(gameState);
```

Then you apply that action to the game state and repeat.

## Examples

The included Tic Tac Toe WPF app allows you to test the ai in action.
