//using TicTacToe.GameLogic;

//namespace TicTacToe.Tests
//{
//    [TestClass]
//    public class BoardStateTests
//    {
//        #region Read & Write Tiles

//        [TestMethod]
//        public void BoardState_OnlyWritesValidTileValues()
//        {
//            // Arrange
//            BoardState boardState = new BoardState();
//            BoardPosition boardPosition = new BoardPosition(0, 0);
//            Player tooLarge = (Player)4;
//            Player tooSmall = (Player)(-1);

//            // Act & Assert
//            Assert.ThrowsException<ArgumentException>(() => boardState[boardPosition] = tooLarge);
//            Assert.ThrowsException<ArgumentException>(() => boardState[boardPosition] = tooSmall);
//        }

//        [TestMethod]
//        public void BoardState_CanReadAnWriteTiles()
//        {
//            // Arrange
//            BoardState boardState = new BoardState();
//            Player[,] states = GetPossibleBoardConstellation();

//            // Act
//            for (int x = 0; x < 3; x++)
//            {
//                for (int y = 0; y < 3; y++)
//                {
//                    BoardPosition boardPosition = new BoardPosition(x, y);
//                    boardState[boardPosition] = states[x, y];
//                }
//            }

//            // Assert
//            for (int x = 0; x < 3; x++)
//            {
//                for (int y = 0; y < 3; y++)
//                {
//                    BoardPosition boardPosition = new BoardPosition(x, y);
//                    Assert.AreEqual(states[x, y], boardState[boardPosition]);
//                }
//            }
//        }

//        #endregion Read & Write Tiles

//        #region Read & Write Active Player

//        [TestMethod]
//        public void BoardState_OnlyWritesValidActivePlayer()
//        {
//            // Arrange
//            BoardState boardState = new BoardState();
//            Player tooLarge = (Player)4;
//            Player tooSmall = (Player)(-1);

//            // Act & Assert
//            Assert.ThrowsException<ArgumentException>(() => boardState.ActivePlayer = tooLarge);
//            Assert.ThrowsException<ArgumentException>(() => boardState.ActivePlayer = tooSmall);
//        }

//        [TestMethod]
//        public void BoardState_CanReadAndWriteActivePlayer()
//        {
//            // Arrange
//            BoardState boardState = new BoardState();

//            // Act & Assert 
//            boardState.ActivePlayer = Player.O;
//            Assert.AreEqual(Player.O, boardState.ActivePlayer);

//            boardState.ActivePlayer = Player.X;
//            Assert.AreEqual(Player.X, boardState.ActivePlayer);
//        }

//        #endregion Read & Write Active Player

//        #region Valid Actions

//        [TestMethod]
//        public void BoardState_ValidActionsCountReturnsTheCorrectNumber()
//        {
//            // Arrange
//            BoardState boardState = new BoardState();
//            Player[,] states = GetPossibleBoardConstellation();

//            // Act & Assert
//            int count = 9;
//            for (int x = 0; x < 3; x++)
//            {
//                for (int y = 0; y < 3; y++)
//                {
//                    Assert.AreEqual(count, boardState.ValidActionsCount);

//                    BoardPosition boardPosition = new BoardPosition(x, y);
//                    boardState[boardPosition] = states[x, y];
//                    if (states[x, y] != Player.None) { count--; }

//                    Assert.AreEqual(count, boardState.ValidActionsCount);
//                }
//            }
//        }

//        [TestMethod]
//        public void BoardState_ValidActionsCountAndGetAllValidActionsAndStatesReturnsTheSameAmountOfActions()
//        {
//            // Arrange
//            BoardState boardState = new BoardState();
//            Player[,] states = GetPossibleBoardConstellation();

//            // Act & Assert
//            for (int x = 0; x < 3; x++)
//            {
//                for (int y = 0; y < 3; y++)
//                {
//                    BoardPosition boardPosition = new BoardPosition(x, y);
//                    boardState[boardPosition] = states[x, y];
//                    Assert.AreEqual(boardState.ValidActionsCount, boardState.GetAllValidActionsAndStates().Count());
//                }
//            }
//        }

//        #endregion Valid Actions

//        #region Calculate Terminated

//        [TestMethod]
//        public void BoardState_EmptyBoardDoesntTerminate()
//        {
//            // Arrange
//            BoardState boardState = new BoardState();

//            // Act & Assert
//            Assert.AreEqual(false, boardState.IsTerminated);
//        }

//        [TestMethod]
//        public void BoardState_EmptyBoardReturnsNoVictor()
//        {
//            // Arrange
//            BoardState boardState = new BoardState();

//            // Act & Assert
//            Assert.AreEqual(Player.None, boardState.TerminatingPlayer);
//        }

//        [TestMethod]
//        public void BoardState_FullBoardTerminatesWithoutVictor()
//        {
//            // Arrange
//            BoardState boardState = new BoardState();
//            boardState[(0, 0)] = Player.X;
//            boardState[(0, 1)] = Player.O;
//            boardState[(0, 2)] = Player.X;
//            boardState[(1, 0)] = Player.O;
//            boardState[(1, 1)] = Player.X;
//            boardState[(1, 2)] = Player.O;
//            boardState[(2, 0)] = Player.O;
//            boardState[(2, 1)] = Player.X;
//            boardState[(2, 2)] = Player.O;

//            // Act & Assert
//            Assert.AreEqual(true, boardState.IsTerminated);
//            Assert.AreEqual(Player.None, boardState.TerminatingPlayer);
//        }

//        [TestMethod]
//        public void BoardState_HorizontallyWonBoardTerminates()
//        {
//            // Arrange
//            BoardState boardState;

//            // Act & Assert
//            boardState = new BoardState();
//            for (int x = 0; x < 3; x++)
//            { boardState[(x, 0)] = Player.X; }
//            Assert.AreEqual(true, boardState.IsTerminated);

//            boardState = new BoardState();
//            for (int x = 0; x < 3; x++)
//            { boardState[(x, 1)] = Player.X; }
//            Assert.AreEqual(true, boardState.IsTerminated);

//            boardState = new BoardState();
//            for (int x = 0; x < 3; x++)
//            { boardState[(x, 2)] = Player.X; }
//            Assert.AreEqual(true, boardState.IsTerminated);
//        }

//        [TestMethod]
//        public void BoardState_VerticallyWonBoardTerminates()
//        {
//            // Arrange
//            BoardState boardState;

//            // Act & Assert
//            boardState = new BoardState();
//            for (int y = 0; y < 3; y++)
//            { boardState[(0, y)] = Player.X; }
//            Assert.AreEqual(true, boardState.IsTerminated);

//            boardState = new BoardState();
//            for (int y = 0; y < 3; y++)
//            { boardState[(1, y)] = Player.X; }
//            Assert.AreEqual(true, boardState.IsTerminated);

//            boardState = new BoardState();
//            for (int y = 0; y < 3; y++)
//            { boardState[(2, y)] = Player.X; }
//            Assert.AreEqual(true, boardState.IsTerminated);
//        }

//        [TestMethod]
//        public void BoardState_DiagonallyWonBoardTerminates()
//        {
//            // Arrange
//            BoardState boardState;

//            // Act & Assert
//            boardState = new BoardState();
//            for (int i = 0; i < 3; i++)
//            { boardState[(i, i)] = Player.X; }
//            Assert.AreEqual(true, boardState.IsTerminated);

//            boardState = new BoardState();
//            for (int i = 0; i < 3; i++)
//            { boardState[(2 - i, i)] = Player.X; }
//            Assert.AreEqual(true, boardState.IsTerminated);
//        }

//        [TestMethod]
//        public void BoardState_WonBoardTerminatesForBothPlayers()
//        {
//            // Arrange
//            BoardState boardState;

//            // Act & Assert
//            boardState = new BoardState();
//            for (int i = 0; i < 3; i++)
//            { boardState[(i, i)] = Player.X; }
//            Assert.AreEqual(true, boardState.IsTerminated);

//            boardState = new BoardState();
//            for (int i = 0; i < 3; i++)
//            { boardState[(i, i)] = Player.O; }
//            Assert.AreEqual(true, boardState.IsTerminated);
//        }

//        [TestMethod]
//        public void BoardState_TerminatedBoardReturnsTheCorrectVictor()
//        {
//            // Arrange
//            BoardState boardState;

//            // Act & Assert
//            boardState = new BoardState();
//            for (int i = 0; i < 3; i++)
//            { boardState[(i, i)] = Player.X; }
//            Assert.AreEqual(Player.X, boardState.TerminatingPlayer);

//            boardState = new BoardState();
//            for (int i = 0; i < 3; i++)
//            { boardState[(i, i)] = Player.O; }
//            Assert.AreEqual(Player.O, boardState.TerminatingPlayer);
//        }

//        #endregion Calculate Terminated

//        #region Utility Methods

//        private static Player[,] GetPossibleBoardConstellation()
//        {
//            Player[,] states = new Player[3, 3];
//            for (int x = 0; x < 3; x++)
//            {
//                for (int y = 0; y < 3; y++)
//                {
//                    Player value = (Player)((x + y) % 3);
//                    states[x, y] = value;
//                    if ((int)value > 2) { throw new Exception("Invalid value"); }
//                }
//            }

//            return states;
//        }

//        #endregion Utility Methods
//    }
//}