using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reversi;
using Reversi.Controller;
using Reversi.Data;
using Reversi.View;
using System.Collections.Generic;
using System.Numerics;

namespace ReversiTests
{
    [TestClass]
    public class ReversiBoardTests
    {
        ReversiBoardController defaultReversiBoardController;

        private ReversiBoardSpace GetBoardSpace(ReversiBoard board, Vector2 space)
        {
            return board.Spaces[(int)space.X, (int)space.Y];
        }

        [TestCleanup]
        public void TestClean()
        {
            defaultReversiBoardController = null;
        }

        [TestInitialize]
        public void TestInit()
        {
            defaultReversiBoardController = new ReversiBoardController(new ReversiBoardView());
        }
        //Tests Copy Constructor of ReversiBoard constructs a new ReversiBoard
        [TestMethod]
        public void ReversiBoardConstructer_CreatesNewBoardFromReversiBoard()
        {
            ReversiBoard reversiBoardCopy = new ReversiBoard(defaultReversiBoardController.Board);
            Assert.AreEqual(defaultReversiBoardController.Board.CurrentPlayer, reversiBoardCopy.CurrentPlayer);
            Vector2 space = new Vector2(3, 2);
            defaultReversiBoardController.MakeMove(new Move(ReversiBoardSpace.BLACK, space));
            Assert.AreNotEqual(GetBoardSpace(reversiBoardCopy, space), GetBoardSpace(defaultReversiBoardController.Board, space));
        }

        [TestMethod]
        public void ReversiBoardConstructer_CreatesDefaultBoard()
        {
            Vector2 black1 = new Vector2(4, 3);
            Vector2 black2 = new Vector2(3, 4);
            Vector2 white1 = new Vector2(4, 4);
            Vector2 white2 = new Vector2(3, 3);
            Assert.AreEqual(GetBoardSpace(defaultReversiBoardController.Board, black1), ReversiBoardSpace.BLACK);
            Assert.AreEqual(GetBoardSpace(defaultReversiBoardController.Board, black2), ReversiBoardSpace.BLACK);
            Assert.AreEqual(GetBoardSpace(defaultReversiBoardController.Board, white1), ReversiBoardSpace.WHITE);
            Assert.AreEqual(GetBoardSpace(defaultReversiBoardController.Board, white2), ReversiBoardSpace.WHITE);
            Assert.AreEqual(defaultReversiBoardController.Board.CurrentPlayer, Player.PlayerBlack);
        }

        [TestMethod]
        public void ReversiBoardConstructer_CreatesBoardFromFile()
        {
            //Todo
        }

        [TestMethod]
        public void ReversiBoardConstructer_CreatesBoardFromLines()
        {
            //Todo
        }

        [TestMethod]
        public void ReversiBoard_ValidMove1_IsValidMove()
        {
            Vector2 space = new Vector2(3, 2);
            Move move = new Move(ReversiBoardSpace.BLACK, space);
            Assert.AreEqual(defaultReversiBoardController.IsValidMove(move), true);
        }
        [TestMethod]
        public void ReversiBoard_InvalidMove1_IsValidMove()
        {
            Vector2 space = new Vector2(0, 0);
            Move move = new Move(ReversiBoardSpace.BLACK, space);
            Assert.AreEqual(defaultReversiBoardController.IsValidMove(move), false);
        }
        [TestMethod]
        public void ReversiBoard_InvalidMove2_IsValidMove()
        {
            Vector2 space = new Vector2(3, 5);
            Move move = new Move(ReversiBoardSpace.BLACK, space);
            Assert.AreEqual(defaultReversiBoardController.IsValidMove(defaultReversiBoardController.Board, move), false);
        }

        [TestMethod]
        public void ReversiBoard_ValidMove1_MakeMove()
        {
            Vector2 space = new Vector2(2, 3);
            bool result = defaultReversiBoardController.MakeMove(new Move(ReversiBoardSpace.BLACK, space));
            Assert.AreEqual(result, true);
            Assert.AreEqual(GetBoardSpace(defaultReversiBoardController.Board, space), ReversiBoardSpace.BLACK);
        }
        [TestMethod]
        public void ReversiBoard_ValidMove2_MakeMove()
        {
            Vector2 space = new Vector2(3, 2);
            bool result = defaultReversiBoardController.MakeMove(new Move(ReversiBoardSpace.BLACK, space));
            Assert.AreEqual(result, true);
            Vector2 flippedToBlackSpace = new Vector2(3, 3);
            Assert.AreEqual(GetBoardSpace(defaultReversiBoardController.Board, flippedToBlackSpace), ReversiBoardSpace.BLACK);
        }
        [TestMethod]
        public void ReversiBoard_ValidMove3_MakeMove()
        {
            Vector2 space = new Vector2(3, 2);
            bool result = defaultReversiBoardController.MakeMove(new Move(ReversiBoardSpace.BLACK, space));
            Assert.AreEqual(result, true);
            Vector2 flippedToBlackSpace = new Vector2(3, 3);
            Vector2 whiteSpace = new Vector2(2, 2);
            result = defaultReversiBoardController.MakeMove(new Move(ReversiBoardSpace.WHITE, whiteSpace));
            Assert.AreEqual(result, true);
            Vector2 flippedToWhiteSpace = new Vector2(3, 3);
            Assert.AreEqual(GetBoardSpace(defaultReversiBoardController.Board, flippedToWhiteSpace), ReversiBoardSpace.WHITE);
        }

        [TestMethod]
        public void ReversiBoard_InvalidMove1_MakeMove()
        {
            Vector2 space = new Vector2(0, 0);
            bool result = defaultReversiBoardController.MakeMove(new Move(ReversiBoardSpace.BLACK, space));
            Assert.AreEqual(result, false);
            Assert.AreEqual(GetBoardSpace(defaultReversiBoardController.Board, space), ReversiBoardSpace.EMPTY);
        }
        [TestMethod]
        public void ReversiBoard_InvalidMove2_MakeMove()
        {
            Vector2 space = new Vector2(3, 3);//White Space
            bool result = defaultReversiBoardController.MakeMove(new Move(ReversiBoardSpace.BLACK, space));
            Assert.AreEqual(result, false);
            Assert.AreEqual(GetBoardSpace(defaultReversiBoardController.Board, space), ReversiBoardSpace.WHITE);
        }

        [TestMethod]
        public void ReversiBoard_Board1_GetMoves()
        {
            List<Vector2> validMovesBlack = 
                new List<Vector2> { 
                    new Vector2(2,3),
                    new Vector2(3,2),
                    new Vector2(4,5),
                    new Vector2(5,4),
                };
            List<Move> moves = defaultReversiBoardController.GetMoves(Player.PlayerBlack, false);
            Assert.AreEqual(moves.Count, validMovesBlack.Count);
            foreach (Move move in moves)
            {
                Assert.AreEqual(validMovesBlack.Remove(move._spacePos), true);
            }
            Assert.AreEqual(validMovesBlack.Count, 0);
        }

        [TestMethod]
        public void ReversiBoard_InvalidMove_SetSpace()
        {
            Vector2 space = new Vector2(0, 0);
            bool result = defaultReversiBoardController.SetSpace(ReversiBoardSpace.BLACK, space);
            Assert.AreEqual(result, true);
            Assert.AreEqual(GetBoardSpace(defaultReversiBoardController.Board, space), ReversiBoardSpace.BLACK);
        }
        [TestMethod]
        public void ReversiBoard_OffBoard_SetSpace()
        {
            Vector2 space = new Vector2(-1, 0);
            bool result = defaultReversiBoardController.SetSpace(ReversiBoardSpace.BLACK, space);
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void ReversiBoard_Tie_Evaluate()
        {
            int resultBlack = defaultReversiBoardController.Evaluate(Player.PlayerBlack, BoardWeights.EvenWeighting);
            int resultWhite = defaultReversiBoardController.Evaluate(Player.PlayerWhite, BoardWeights.EvenWeighting);
            Assert.AreEqual(resultBlack, 0);
            Assert.AreEqual(resultWhite, 0);
        }

        [TestMethod]
        public void ReversiBoard_Default_IsTerminal()
        {
            bool resultBlack = defaultReversiBoardController.IsTerminal();
            Assert.AreEqual(resultBlack, false);
        }
        [TestMethod]
        public void ReversiBoard_End1_IsTerminal()
        {
            string[] lines =
            {
                "|W|W|W|W|W|W|W|W|",
                "|W|W|W|W|W|W|W|W|",
                "|W|W|W|W|W|W|W|W|",
                "|W|W|W|W|W|W|W|-|",
                "|W|W|W|W|W|W|-|-|",
                "|W|W|W|W|W|W|-|B|",
                "|W|W|W|W|W|W|W|-|",
                "|W|W|W|W|W|W|W|W|",
            };
            ReversiBoard reversiBoard = new ReversiBoard(lines);
            Assert.AreEqual(defaultReversiBoardController.IsTerminal(reversiBoard), true);
        }
        [TestMethod]
        public void ReversiBoard_End2_IsTerminal()
        {
            string[] lines =
            {
                "|-|B|B|B|B|B|B|B|",
                "|-|W|W|W|W|W|-|B|",
                "|W|W|W|W|W|W|W|B|",
                "|W|W|W|W|W|W|W|B|",
                "|W|W|W|W|W|W|W|B|",
                "|W|W|W|W|W|W|W|B|",
                "|W|W|W|W|W|W|W|B|",
                "|-|W|W|W|W|W|-|-|",
            };
            ReversiBoard reversiBoard = new ReversiBoard(lines);
            Assert.AreEqual(defaultReversiBoardController.IsTerminal(reversiBoard), true);
        }
        [TestMethod]
        public void ReversiBoard_End3_IsTerminal()
        {
            string[] lines =
            {
                "|-|-|-|-|W|-|-|-|",
                "|-|-|-|-|W|W|-|-|",
                "|W|W|W|W|W|W|W|B|",
                "|-|-|W|W|W|W|-|B|",
                "|-|-|W|W|W|-|-|B|",
                "|-|-|-|-|-|-|-|-|",
                "|-|-|-|-|-|-|-|-|",
                "|-|-|-|-|-|-|-|-|"
            };
            ReversiBoard reversiBoard = new ReversiBoard(lines);
            Assert.AreEqual(defaultReversiBoardController.IsTerminal(reversiBoard), true);
        }
    }
}