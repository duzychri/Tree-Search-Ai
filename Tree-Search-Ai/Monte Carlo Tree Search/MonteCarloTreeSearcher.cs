using Ai.Utilities;

namespace MonteCarloTreeSearch
{
    /// <summary>
    /// A class that can be used to find the best move in a game using the Monte Carlo Tree Search algorithm.
    /// </summary>
    /// <typeparam name="GameState">The type that represents a state of the game.</typeparam>
    /// <typeparam name="GameAction">The type that represents an action that can be taken in the game.</typeparam>
    public partial class MonteCarloTreeSearcher<GameState, GameAction>
        where GameAction : IGameAction<GameAction>
        where GameState : IGameState<GameState, GameAction>
    {
        private readonly Random randomGenerator;
        private readonly MonteCarloTreeSearcherSettings<GameState, GameAction> settings;

        private int treeDepth = 0;
        private TreeNode rootNode = null!;

        /// <summary>
        /// Creates a new Monte Carlo Tree Searcher.
        /// </summary>
        /// <param name="settings">The settings for the Monte Carlo Tree Searcher.</param>
        public MonteCarloTreeSearcher(MonteCarloTreeSearcherSettings<GameState, GameAction> settings)
        {
            this.settings = settings;
            randomGenerator = new Random(settings.RandomSeed);
        }

        /// <summary>
        /// Returns the move that the AI thinks is the best.
        /// </summary>
        /// <param name="state">The state that the board is in before the move.</param>
        /// <returns>The move that the AI thinks is the best.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no possible moves could be evaluated.</exception>
        public GameAction? GetBestMove(GameState state)
        {
            treeDepth = 0;
            rootNode = new TreeNode(null, state, default);

            for (int n = 0; n < settings.SimulationCount; n++)
            {
                TreeNode selectedNode = SelectNode();
                TreeNode expandedNode = ExpandNode(selectedNode);
                (TreeNode simulatedNode, GameState outcome) = SimulateOutcome(expandedNode, settings.MaxSimulationDepth);
                PropagateOutcome(simulatedNode, outcome);
            }

            // Return the move that leads to the best child.
            TreeNode? monteCarloNode = rootNode.Children.MaxBy(c => c.Score);
            if (monteCarloNode == null) { throw new InvalidOperationException("No possible moves could be evaluated."); }
            return monteCarloNode.Action;
        }

        /// <summary>
        /// Recursively picks the children which maximizes the value according to the upper confidence bound.
        /// </summary>
        private TreeNode SelectNode()
        {
            TreeNode current = rootNode;

            while (current.HasChildren)
            { current = current.Children.MaxBy(c => c.GetUpperConfidenceBound(settings.ExplorationConfidence))!; }

            return current;
        }

        /// <summary>
        /// Expands the children of the node if the board state is not terminated and no children have been created yet.
        /// Selects a radom child and returns it.
        /// </summary>
        private TreeNode ExpandNode(TreeNode node)
        {
            // If the game is over, the we can't do much but return the node.
            if (node.State.IsTerminated)
            { return node; }

            // Expand missing children.
            if (node.Children.Any() == false)
            {
                foreach ((GameAction action, GameState gameState) in node.State.GetAllValidActionsAndStates())
                {
                    TreeNode newNode = new TreeNode(node, gameState, action);
                    node.Children.Add(newNode);
                    treeDepth = Math.Max(treeDepth, newNode.Depth);
                }
            }

            // Get a random child.
            node = node.Children[randomGenerator.Next(node.Children.Count)];

            // That child will now be simulated.
            return node;
        }

        /// <summary>
        /// Simulates the outcome of the game by randomly picking a valid action until the game is over or the max depth is reached.
        /// </summary>
        private (TreeNode simulatedNode, GameState gameState) SimulateOutcome(TreeNode node, int maxDepth)
        {
            int depth = 0;
            GameState tempState = node.State;

            while (tempState.IsTerminated == false && depth < maxDepth)
            {
                int count = tempState.ValidActionsCount;
                (_, GameState randomState) = tempState.GetAllValidActionsAndStates().GetRandom(randomGenerator, count);
                tempState = randomState;
                depth++;
            }

            return (node, tempState);
        }

        /// <summary>
        /// Propagates the outcome of the simulation back up the tree.
        /// </summary>
        private void PropagateOutcome(TreeNode node, GameState outcome)
        {
            float score = settings.CalculateScore(outcome);

            TreeNode? currentNode = node;
            while (currentNode != null)
            {
                currentNode.Visits++;
                currentNode.Score += score;
                currentNode = currentNode.Parent;
            }
        }
    }
}