namespace Ai.Utilities
{
    /// <summary>
    /// Defines a tree node.
    /// </summary>
    public interface ITreeNode<Node> where Node : ITreeNode<Node>
    {
        /// <summary>
        /// The parent of this node.
        /// </summary>
        Node? Parent { get; }

        /// <summary>
        /// The children of this node.
        /// </summary>
        List<Node> Children { get; }

        /// <summary>
        /// <see langword="true" /> if this node has children, otherwise <see langword="false" />.
        /// </summary>
        bool HasChildren { get; }
    }
}