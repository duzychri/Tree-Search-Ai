namespace MonteCarloTreeSearch
{
    /// <summary>
    /// The settings for the Monte Carlo Tree Searcher.
    /// </summary>
    /// <typeparam name="GameState">The type that represents a state of the game.</typeparam>
    /// <typeparam name="GameAction">The type that represents an action that can be taken in the game.</typeparam>
    public class MonteCarloTreeSearcherSettings<GameState, GameAction>
        where GameAction : IGameAction<GameAction>
        where GameState : IGameState<GameState, GameAction>
    {
        /// <summary>
        /// The seed that is used to generate random numbers.
        /// </summary>
        public int RandomSeed { get; set; } = 1234567890;
        /// <summary>
        /// The amount of simulations that will be run.
        /// </summary>
        public int SimulationCount { get; set; } = 1000;
        /// <summary>
        /// The maximum depth that the tree will be expanded to.
        /// </summary>
        public int MaxSimulationDepth { get; set; } = 100;

        /// <summary>
        /// The constant that the exploration term in the UCB formula is multiplied by.
        /// </summary>
        public float ExplorationConfidence { get; init; } = 1;
        /// <summary>
        /// The score that is used to determine the best child node and is propagated up the tree.
        /// </summary>
        public required Func<GameState, float> CalculateScore { get; set; }
    }
}