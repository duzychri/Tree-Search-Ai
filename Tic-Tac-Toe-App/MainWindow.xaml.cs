using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using TicTacToe.GameLogic;
using MonteCarloTreeSearch;

namespace TicTacToe.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TicTacToeEngine gameEngine = null!;
        private readonly MonteCarloTreeSearcher<BoardState, GameAction> gameAi;

        public MainWindow()
        {
            // Initialize game engine and ai.
            var aiSettings = new MonteCarloTreeSearcherSettings<BoardState, GameAction>()
            {
                CalculateScore = (outcome) =>
                {
                    float score = 0f;
                    if (outcome.TerminatingPlayer == Player.O)
                    { score = 1f; }
                    else if (outcome.IsTerminated && outcome.TerminatingPlayer == Player.None)
                    { score = 1f; }
                    return score;
                }
            };
            gameAi = new MonteCarloTreeSearcher<BoardState, GameAction>(aiSettings);

            // Initialize window.
            InitializeComponent();

            // Initialize game.
            Reset();
        }

        private void Reset()
        {
            // Reset game engine and ai.
            gameEngine = new TicTacToeEngine();

            // Clear all buttons.
            foreach (Button button in GetAllButtons())
            {
                button.Content = "";
            }
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)e.Source;
            (int column, int row) = GetButtonCoordinates(button);

            if (gameEngine.GameIsOver)
            { return; }

            if (string.IsNullOrWhiteSpace(button.Content as string) == false)
            { return; }

            HandleTileClick((column, row));

            if (gameEngine.GameIsOver == false)
            {
                BoardState gameState = gameEngine.GetBoardState();
                GameAction nextMove = gameAi.GetBestMove(gameState);
                HandleTileClick(nextMove.Position);

                if (gameEngine.GameIsOver)
                { HandleGameOver(); }
            }
            else
            {
                HandleGameOver();
            }
        }

        private void HandleGameOver()
        {
            LabelGameOver.Content = "Game Over: Yes";
            switch (gameEngine.VictoriousPlayer)
            {
                case Player.X:
                    MessageBox.Show("X has won!");
                    break;
                case Player.O:
                    MessageBox.Show("O has won!");
                    break;
                case Player.None:
                    MessageBox.Show("Draw!");
                    break;
            }
        }

        #region Game Logic Methods

        private void SetButtonContent(BoardPosition position, Player player)
        {
            Button button = GetButton(position);
            button.Content = player == Player.X ? "X" : "O";
        }

        private void HandleTileClick(BoardPosition position)
        {
            gameEngine.MakeMove(position);

            Player player = gameEngine.GetTilePlayer(position);
            SetButtonContent(position, player);
        }

        #endregion Game Logic Methods

        #region Utility Methods

        private IEnumerable<Button> GetAllButtons()
        {
            yield return Button00;
            yield return Button01;
            yield return Button02;
            yield return Button10;
            yield return Button11;
            yield return Button12;
            yield return Button20;
            yield return Button21;
            yield return Button22;
        }

        private BoardPosition GetButtonCoordinates(Button button)
        {
            string columnAndRow = button.Name.Replace("Button", "");
            int column = int.Parse(columnAndRow[0].ToString());
            int row = int.Parse(columnAndRow[1].ToString());
            return (column, row);
        }

        private Button GetButton(BoardPosition position)
        {
            return (position.Column, position.Row) switch
            {
                (0, 0) => Button00,
                (0, 1) => Button01,
                (0, 2) => Button02,
                (1, 0) => Button10,
                (1, 1) => Button11,
                (1, 2) => Button12,
                (2, 0) => Button20,
                (2, 1) => Button21,
                (2, 2) => Button22,
                _ => throw new Exception("Invalid column and row"),
            };
        }

        #endregion Utility Methods
    }
}
