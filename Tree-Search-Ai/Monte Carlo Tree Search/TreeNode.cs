using Ai.Utilities;
using System.Diagnostics;

namespace MonteCarloTreeSearch
{
    public partial class MonteCarloTreeSearcher<GameState, GameAction>
        where GameAction : IGameAction<GameAction>
        where GameState : IGameState<GameState, GameAction>
    {
        /// <summary>
        /// A node for a Monte Carlo Tree Search.
        /// </summary>
        [DebuggerDisplay("Visits = {Visits}, Score = {Score}, Action = {Action}")]
        private class TreeNode : ITreeNode<TreeNode>
        {
            #region ITreeNode Properties

            /// <summary>
            /// The parent of this node.
            /// </summary>
            public TreeNode? Parent { get; }

            /// <summary>
            /// The children of this node.
            /// </summary>
            public List<TreeNode> Children { get; }

            /// <summary>
            /// <see langword="true" /> if this node has children, otherwise <see langword="false" />.
            /// </summary>
            public bool HasChildren => Children.Count > 0;

            #endregion ITreeNode Properties

            #region MCTS Properties

            /// <summary>
            /// The sum of the times that choosing this node has resulted in a 'win'.
            /// </summary>
            public float Score { get; set; } = 0;

            /// <summary>
            /// The amount of times this node has been visited.
            /// </summary>
            public float Visits { get; set; } = 0;

            /// <summary>
            /// The game action that lead from the last state to this one.
            /// </summary>
            public GameAction? Action { get; }

            /// <summary>
            /// The state of the game at this node.
            /// </summary>
            public GameState State { get; }

            #endregion MCTS Properties

            #region Debug Properties & Methods

            /// <summary>
            /// The depth of this node in the tree.
            /// </summary>
            public int Depth => Parent?.Depth + 1 ?? 0;

            /// <summary>
            /// Returns all parents of this node.
            /// </summary>
            public IEnumerable<TreeNode> GetParents()
            {
                TreeNode? currentNode = Parent;
                while (currentNode != null)
                {
                    yield return currentNode;
                    currentNode = currentNode.Parent;
                }
            }

            #endregion Debug Properties & Methods

            /// <summary>
            /// Creates a new Monte Carlo Tree Search node.
            /// </summary>
            public TreeNode(TreeNode? parent, GameState state, GameAction? action)
            {
                Parent = parent;
                Children = new List<TreeNode>();

                State = state;
                Action = action;
            }

            /// <summary>
            /// Returns the upper confidence bound of the node.
            /// </summary>
            /// <param name="explorationConfidence">A confidence value that controls the level of exploration.</param>
            public float GetUpperConfidenceBound(float explorationConfidence)
            {
                if (Parent == null)
                { throw new InvalidOperationException("The upper confidence bound can only be calculated for nodes that have a parent."); }

                // Unexplored nodes have maximum values so we favour exploration.
                if (Visits == 0)
                { return float.PositiveInfinity; }

                // The current estimated value of the node.
                // This makes it so we prefer to visit nodes that seem to have a higher win rate.
                float exploitationTerm = Score / Visits;

                // A term that is inversely proportional to the number of times the node has been visited, with respect to the number of visits of its parent node.
                // This shoud make it so we prefer to visit nodes that have not been visited as much.
                float explorationTerm = MathF.Sqrt(MathF.Log(Parent.Visits) / Visits);

                // The upper confidence bound of the node.
                return exploitationTerm + explorationConfidence * explorationTerm;
            }
        }
    }
}