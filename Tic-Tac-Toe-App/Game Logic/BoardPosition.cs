using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TicTacToe.GameLogic
{
    [DebuggerDisplay("{ToString()}")]
    public readonly struct BoardPosition : IEquatable<BoardPosition>
    {
        /// <summary>
        /// The position on the x-axis.
        /// </summary>
        public readonly int Column => index % 3;
        /// <summary>
        /// The position on the y-axis.
        /// </summary>
        public readonly int Row => index / 3;

        private readonly int index;

        public BoardPosition(int column, int row)
        {
            // Check if the column is valid.
            if (column < 0 || column > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "Column must be between 0 and 2.");
            }

            // Check if the row is valid.
            if (row < 0 || row > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Row  must be between 0 and 2.");
            }

            // Set the index.
            index = PositionToIndex(column, row);
        }

        #region Internal Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int GetIndex()
        {
            return index;
        }

        /// <summary>
        /// Calculates the index of a board position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int PositionToIndex(int column, int row)
        {
            return column + row * 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static BoardPosition IndexToPosition(int index)
        {
            return new BoardPosition(index % 3, index / 3);
        }

        #endregion Internal Methods

        #region Operators

        public void Deconstruct(out int column, out int row) { column = Column; row = Row; }
        public static implicit operator (int x, int y)(BoardPosition b) => (b.Column, b.Row);
        public static implicit operator BoardPosition((int x, int y) value) => new BoardPosition(value.x, value.y);

        #endregion Operators

        #region ToString

        override public string ToString() => $"({Column}, {Row})";

        #endregion ToString

        #region IEquatable

        public override int GetHashCode() => HashCode.Combine(index);

        public bool Equals(BoardPosition other) => index == other.index;
        public override bool Equals(object? obj) => obj is BoardPosition position && Equals(position);

        public static bool operator ==(BoardPosition left, BoardPosition right) => left.index == right.index;
        public static bool operator !=(BoardPosition left, BoardPosition right) => left.index != right.index;

        #endregion IEquatable
    }
}