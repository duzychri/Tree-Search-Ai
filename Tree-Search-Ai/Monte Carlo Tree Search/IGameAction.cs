namespace MonteCarloTreeSearch
{
    /// <summary>
    /// Represents an action that can be taken in a game.
    /// </summary>
    public interface IGameAction<GameAction>
        where GameAction : IGameAction<GameAction>
    { }
}