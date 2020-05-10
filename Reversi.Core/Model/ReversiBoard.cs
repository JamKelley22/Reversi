using System;
using System.IO;

namespace Reversi
{
    public class ReversiBoard
    {
        private ReversiBoardSpace[,] _spaces;
        public ReversiBoardSpace[,] Spaces { get { return _spaces; } set { _spaces = value; } }
        public Player CurrentPlayer { get; set; }

        public ReversiBoard()
        {
            _spaces = new ReversiBoardSpace[Constants.REVERSI_BOARD_LENGTH, Constants.REVERSI_BOARD_LENGTH];
            //Default Reversi board config has 2 white pieces and 2 black pieces placed already in diagnal

            _spaces[4, 3] = ReversiBoardSpace.BLACK;
            _spaces[3, 4] = ReversiBoardSpace.BLACK;
            _spaces[4, 4] = ReversiBoardSpace.WHITE;
            _spaces[3, 3] = ReversiBoardSpace.WHITE;

            CurrentPlayer = Player.PlayerBlack;
        }

        public ReversiBoard(ReversiBoard reversiBoard)
        {
            _spaces = (ReversiBoardSpace[,])reversiBoard._spaces.Clone();
            CurrentPlayer = reversiBoard.CurrentPlayer;
        }

        public ReversiBoard(string[] lines)
        {
            _spaces = GetSpacesFromLines(lines);
            CurrentPlayer = Player.PlayerBlack;
        }

        public ReversiBoard(ReversiBoardSpace[,] spaces)
        {
            _spaces = spaces;
            CurrentPlayer = Player.PlayerBlack;
        }

        public ReversiBoard(string boardFilePath)
        {
            if (!File.Exists(boardFilePath))
            {
                throw new FileNotFoundException("Board File Not Found", boardFilePath);
            }

            string[] lines = File.ReadAllLines(boardFilePath);
            _spaces = GetSpacesFromLines(lines);
            CurrentPlayer = Player.PlayerBlack;
        }

        private ReversiBoardSpace[,] GetSpacesFromLines(string[] lines)
        {
            if (lines.GetLength(0) != Constants.REVERSI_BOARD_LENGTH)
            {
                throw new Exception("Incorrect Board Size");
            }

            ReversiBoardSpace[,] tempSpaces = new ReversiBoardSpace[Constants.REVERSI_BOARD_LENGTH, Constants.REVERSI_BOARD_LENGTH];

            for (int i = 0; i < lines.GetLength(0); i++)
            {
                char[] lineChars = lines[i].ToCharArray();
                int currCol = 0;

                if (lineChars.GetLength(0) != 17)
                {
                    throw new Exception("Incorrect Character Count");
                }

                for (int j = 0; j < lineChars.GetLength(0); j++)
                {
                    switch (lineChars[j])
                    {
                        case 'B':
                            tempSpaces[i, currCol++] = ReversiBoardSpace.BLACK;
                            break;
                        case 'W':
                            tempSpaces[i, currCol++] = ReversiBoardSpace.WHITE;
                            break;
                        case '-':
                            tempSpaces[i, currCol++] = ReversiBoardSpace.EMPTY;
                            break;
                        case '|':
                            break;
                        default:
                            throw new Exception("Invalid Character");
                    }
                }

            }
            return tempSpaces;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Spaces.GetHashCode();
                hash = hash * 23 + CurrentPlayer.GetHashCode();
                return hash;
            }
        }
    }
}
