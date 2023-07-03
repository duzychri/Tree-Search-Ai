using System;

namespace TicTacToe.GameLogic
{
    public enum Player
    {
        None = 0,
        X = 1,
        O = 2
    }

    public static class PlayerExtensions
    {
        public static bool IsValid(this Player value)
        {
            return (int)value >= 0 && (int)value <= 2;
        }

        public static Player Flipped(this Player value)
        {
            if (value == Player.None)
            { throw new InvalidOperationException("Cannot flip Player.None."); }

            return value == Player.X ? Player.O : Player.X;
        }
    }
}