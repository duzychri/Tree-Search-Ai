namespace MonteCarloTreeSearch
{
    /// <summary>
    /// Represents a state that a game can be in.
    /// </summary>
    public interface IGameState<GameState, GameAction> : IEquatable<GameState>
        where GameState : IGameState<GameState, GameAction>
    {
        /// <summary>
        /// Returns true if the game is over, otherwise false.
        /// </summary>
        bool IsTerminated { get; }

        /// <summary>
        /// Returns the amount of valid actions that can be taken from this state.
        /// </summary>
        int ValidActionsCount { get; }

        /// <summary>
        /// Returns the valid actions and their resulting states that can be taken from this state.
        /// </summary>
        IEnumerable<(GameAction action, GameState state)> GetAllValidActionsAndStates();
    }
}