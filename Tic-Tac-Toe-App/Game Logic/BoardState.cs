using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TicTacToe.Utilities;
using MonteCarloTreeSearch;

namespace TicTacToe.GameLogic
{
    /// <summary>
    /// Represents a state of a Tic-Tac-Toe board.
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public struct BoardState : IGameState<BoardState, GameAction>, IEquatable<BoardState>
    {
        private int stateBits;

        private const int activePlayerIndex = 9 * 2;
        private const int isTerminatedIndex = 10 * 2;
        private const int terminatedPlayerIndex = 11 * 2;
        private const int validActionsCountIndex = 12 * 2;

        #region Constructors

        /// <summary>
        /// Creates a new empty board state.
        /// </summary>
        public BoardState()
        {
            stateBits = 0;
            ValidActionsCount = 9;
            ActivePlayer = Player.X;
        }

        #endregion Constructors

        #region IBoardState

        /// <summary>
        /// Returns the amount of valid actions that can be taken from this state.
        /// </summary>
        public int ValidActionsCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => BitHelper.GetBits4(ref stateBits, validActionsCountIndex);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set => BitHelper.SetBits4(ref stateBits, validActionsCountIndex, value);
        }

        /// <summary>
        /// Returns true if the game is over, otherwise false.
        /// </summary>
        public bool IsTerminated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => BitHelper.GetBits2(ref stateBits, isTerminatedIndex) != 0;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set => BitHelper.SetBits2(ref stateBits, isTerminatedIndex, value ? 1 : 0);
        }

        /// <summary>
        /// Returns the player that won the game, or Player.None if the game is not over.
        /// </summary>
        public Player TerminatingPlayer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (Player)BitHelper.GetBits2(ref stateBits, terminatedPlayerIndex);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set => BitHelper.SetBits2(ref stateBits, terminatedPlayerIndex, (int)value);
        }

        /// <summary>
        /// The player who will make the next move.
        /// </summary>
        public Player ActivePlayer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetActivePlayer();
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => SetActivePlayer(value);
        }

        /// <summary>
        /// Gets or sets the specified tile to the specified player.
        /// </summary>
        public Player this[BoardPosition position]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetTile(position.GetIndex());
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => SetTile(position.GetIndex(), value);
        }

        /// <summary>
        /// Returns the valid actions and their resulting states that can be taken from this state.
        /// </summary>
        public IEnumerable<(GameAction action, BoardState state)> GetAllValidActionsAndStates()
        {
            for (int index = 0; index < 9; index++)
            {
                if (GetTile(index) != Player.None)
                { continue; }

                BoardState newState = this;
                newState.SetTile(index, ActivePlayer);
                newState.ActivePlayer = ActivePlayer.Flipped();

                BoardPosition position = BoardPosition.IndexToPosition(index);
                yield return (new GameAction(ActivePlayer, position), newState);
            }
        }

        #endregion IBoardState

        #region Active Player Methods

        private Player GetActivePlayer()
        {
            return (Player)BitHelper.GetBits2(ref stateBits, activePlayerIndex);
        }

        private void SetActivePlayer(Player player)
        {
            if (player.IsValid() == false)
            { throw new ArgumentException("Invalid player value.", nameof(player)); }

            BitHelper.SetBits2(ref stateBits, activePlayerIndex, (int)player);
        }

        #endregion Active Player Methods

        #region Tile Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Player GetTile(int tileIndex)
        {
            return (Player)BitHelper.GetBits2(ref stateBits, tileIndex * 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetTile(int tileIndex, Player value)
        {
            if (value.IsValid() == false)
            { throw new ArgumentException("Invalid player value.", nameof(value)); }

            BitHelper.SetBits2(ref stateBits, tileIndex * 2, (int)value);
            UpdateTermination();
        }

        private void UpdateTermination()
        {
            // Calculate the number of valid actions.
            int validActionsCount = 0;
            for (int index = 0; index < 9; index++)
            {
                if (GetTile(index) == Player.None)
                { validActionsCount++; }
            }
            ValidActionsCount = validActionsCount;

            // Check if a player has won.
            Player terminatingPlayer = GetTerminatingPlayer();
            TerminatingPlayer = terminatingPlayer;

            // Check if the game is over.
            bool isTerminated = validActionsCount == 0 || terminatingPlayer != Player.None;
            IsTerminated = isTerminated;
        }

        private Player GetTerminatingPlayer()
        {
            // Check for a horizontal win.
            for (int row = 0; row < 3; row++)
            {
                if ((this[(0, row)] & this[(1, row)] & this[(2, row)]) != Player.None)
                { return this[(0, row)]; }
            }

            // Check for a vertical win.
            for (int column = 0; column < 3; column++)
            {
                if ((this[(column, 0)] & this[(column, 1)] & this[(column, 2)]) != Player.None)
                { return this[(column, 0)]; }
            }

            // Check for a diagonal win.
            if ((this[(0, 0)] & this[(1, 1)] & this[(2, 2)]) != Player.None)
            { return this[(1, 1)]; }

            if ((this[(2, 0)] & this[(1, 1)] & this[(0, 2)]) != Player.None)
            { return this[(1, 1)]; }

            // No winner.
            return Player.None;
        }

        #endregion Tile Methods

        #region ToString

        /// <summary>
        /// Returns a string representation of the board state.
        /// </summary>
        public override string ToString()
        {
            return BitHelper.IntToBitString(stateBits);
        }

        #endregion

        #region IEquatable

        public override int GetHashCode() => HashCode.Combine(stateBits);

        public bool Equals(BoardState other) => stateBits == other.stateBits;
        public override bool Equals(object? obj) => obj is BoardState state && Equals(state);

        public static bool operator ==(BoardState left, BoardState right) => left.stateBits == right.stateBits;
        public static bool operator !=(BoardState left, BoardState right) => left.stateBits != right.stateBits;

        #endregion IEquatable
    }
}