using MonteCarloTreeSearch;

namespace TicTacToe.GameLogic
{
    public record struct GameAction(Player Player, BoardPosition Position) : IGameAction<GameAction>;
}