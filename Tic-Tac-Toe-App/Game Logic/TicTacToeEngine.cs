using System;
using System.Collections.Generic;

namespace TicTacToe.GameLogic
{
    public class TicTacToeEngine
    {
        public bool GameIsOver => board.IsTerminated;
        public Player CurrentPlayer => board.ActivePlayer;
        public Player VictoriousPlayer => board.TerminatingPlayer;

        private BoardState board = new();
        private readonly Stack<GameAction> moves = new();

        #region Api Methods

        public BoardState GetBoardState()
        {
            return board;
        }

        public Player GetTilePlayer(BoardPosition position)
        {
            return board[position];
        }

        public void MakeMove(BoardPosition position)
        {
            // Check if the game is over.
            if (GameIsOver)
            {
                throw new InvalidOperationException("The game is already over.");
            }

            // Check if the position is already taken.
            if (GetTilePlayer(position) == Player.X || GetTilePlayer(position) == Player.O)
            {
                throw new InvalidOperationException("Position is already taken.");
            }

            // Add the move to the stack.
            moves.Push(new GameAction(CurrentPlayer, position));

            // Set the position to the current player.
            board[position] = CurrentPlayer;

            // Switch the current player.
            board.ActivePlayer = CurrentPlayer.Flipped();
        }

        public void UndoMove()
        {
            // Check if there are any moves to undo.
            if (moves.Count == 0)
            {
                throw new InvalidOperationException("There are no moves to undo.");
            }

            // Get the last move.
            GameAction lastMove = moves.Pop();

            // Remove the player from the board.
            board[lastMove.Position] = Player.None;

            // Switch the current player.
            board.ActivePlayer = lastMove.Player;
        }

        #endregion Api Methods
    }
}