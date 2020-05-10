using Reversi.Data;
using Reversi.View;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Reversi.Controller
{
    public class ReversiBoardController
    {
        ReversiBoard _model;
        IReversiBoardView _view;

        public ReversiBoard Board { get { return _model; } }

        public ReversiBoardController(IReversiBoardView view)
        {
            _model = new ReversiBoard();//Default Board Config
            _view = view;
        }

        public ReversiBoardController(string[] lines, IReversiBoardView view)
        {
            _model = new ReversiBoard(lines);//Created from string[]
            _view = view;
        }

        public ReversiBoardController(string boardFilePath, IReversiBoardView view)
        {
            _model = new ReversiBoard(boardFilePath);//Loaded from file
            _view = view;
        }

        public ReversiBoardController(ReversiBoardSpace[,] spaces, IReversiBoardView view)
        {
            _model = new ReversiBoard(spaces);
            _view = view;//Todo remove?
        }

        public void Clear()
        {
            _view.Clear();
        }

        //==========View==========
        public void PrintTitle()
        {
            _view.PrintTitle();
        }
        public int GetDifficulty()
        {
            return _view.GetDifficulty();
        }
        public bool GetUserFirst()
        {
            return _view.GetUserFirst();
        }
        public void PrintBoard()
        {
            _view.PrintBoard(_model);
        }

        public Move GetUserMove(Player userPlayer)
        {
            return _view.GetUserMove(userPlayer);
        }

        public void ShowWin(Player player)
        {
            _view.ShowWin(player);
        }

        public void ShowError(string val)
        {
            _view.ShowError(val);
        }

        public void Write(string val)
        {
            _view.Write(val);
        }

        //========================

        private bool IsOnBoard(Vector2 space)
        {
            if (space.X < 0 || space.X > Constants.REVERSI_BOARD_LENGTH - 1 || space.Y < 0 || space.Y > Constants.REVERSI_BOARD_LENGTH - 1) return false;
            return true;
        }

        public bool SetSpace(ReversiBoardSpace type, Vector2 spacePos)
        {
            return SetSpace(_model, type, spacePos);
        }
        public bool SetSpace(ReversiBoard board, ReversiBoardSpace type, Vector2 spacePos)
        {
            if (!IsOnBoard(spacePos)) return false;
            board.Spaces[(int)spacePos.X, (int)spacePos.Y] = type;
            return true;
        }

        public ReversiBoardSpace GetSpace(Vector2 spacePos)
        {
            return GetSpace(_model, spacePos);
        }
        public ReversiBoardSpace GetSpace(ReversiBoard board, Vector2 spacePos)
        {
            if (!IsOnBoard(spacePos)) throw new IndexOutOfRangeException();
            return board.Spaces[(int)spacePos.X, (int)spacePos.Y];
        }

        public bool MakeMove(Move move)
        {
            return MakeMove(_model, move);
        }
        public bool MakeMove(ReversiBoard board, Move move)
        {
            if (!IsValidMove(board, new Move(move._spaceType, move._spacePos))) return false;
            SetSpace(board, move._spaceType, move._spacePos);

            //Flip Captured Pieces
            //Check each direction to ensure all caputured pieces are flipped
            FlipDirection(board, move, new Vector2(0, -1)); //Up
            FlipDirection(board, move, new Vector2(1, -1)); //UpRight
            FlipDirection(board, move, new Vector2(1, 0)); //Right
            FlipDirection(board, move, new Vector2(1, 1)); //DownRight
            FlipDirection(board, move, new Vector2(0, 1)); //Down
            FlipDirection(board, move, new Vector2(-1, 1)); //DownLeft
            FlipDirection(board, move, new Vector2(-1, 0)); //Left
            FlipDirection(board, move, new Vector2(-1, -1)); //UpLeft

            board.CurrentPlayer = (board.CurrentPlayer == Player.PlayerBlack) ? Player.PlayerWhite : Player.PlayerBlack;
            return true;
        }

        private bool FlipDirection(ReversiBoard board, Move move, Vector2 dir)
        {
            Vector2 checkSpace = move._spacePos + dir;
            //Check to be sure new pos is on board
            if (!IsOnBoard(checkSpace)) return false;

            ReversiBoardSpace boardCheckSpace = GetSpace(board, checkSpace);
            //If check space is empty or same color then isn't valid, just return
            if (boardCheckSpace == ReversiBoardSpace.EMPTY || boardCheckSpace == move._spaceType) return false;
            
            List<Vector2> capturedSpaces = new List<Vector2>();
            capturedSpaces.Add(checkSpace);

            checkSpace += dir;
            while (IsOnBoard(checkSpace))
            {
                //Here we are looking for filled space of our own
                boardCheckSpace = GetSpace(board, checkSpace);
                //Another empty space means it isnt capped by our own piece
                if (boardCheckSpace == ReversiBoardSpace.EMPTY) return false;
                //If our color return true
                if (boardCheckSpace == move._spaceType)
                {
                    foreach(Vector2 capturedSpace in capturedSpaces)
                    {
                        SetSpace(board, move._spaceType, capturedSpace);
                    }
                    return true;
                }
                //Keep going if opponents color, add space to potental captured pieces
                capturedSpaces.Add(checkSpace);
                checkSpace += dir;
            }
            //Ran out of board so isnt valid
            return false;
        }

        public List<Move> GetMoves(Player player, bool sort)
        {
            return GetMoves(_model, player, sort);
        }
        public List<Move> GetMoves(ReversiBoard board, Player player, bool sort)
        {
            List<Move> validMoves = new List<Move>();
            for (int i = 0; i < board.Spaces.GetLength(0); i++)
            {
                for (int j = 0; j < board.Spaces.GetLength(1); j++)
                {
                    Vector2 spacePos = new Vector2(i, j);
                    if (IsValidMove(board, new Move(PlayerToSpace(player), spacePos))) validMoves.Add(new Move(PlayerToSpace(player), spacePos));
                }
            }
            if(sort)
                validMoves.Sort(CompareMoves);
            return validMoves;
        }

        private int CompareMoves(Move m1, Move m2)
        {
            if ((m1._none && m2._none) || (m1 == null && m2 == null)) return 0;//Both Empty
            if (m1._none || m1 == null) return -1;
            if (m2._none || m2 == null) return 1;

            //Both are actual moves
            int m1Weight = BoardWeights.CustomWeighting[(int)m1._spacePos.X, (int)m1._spacePos.Y];
            int m2Weight = BoardWeights.CustomWeighting[(int)m2._spacePos.X, (int)m2._spacePos.Y];

            //Todo: Do something like sort by weight then sort by moves opponent cane make?

            return m2Weight - m1Weight;
        }

        private bool IsSpaceOccupied(Vector2 space)
        {
            return IsSpaceOccupied(_model, space);
        }
        private bool IsSpaceOccupied(ReversiBoard board, Vector2 space)
        {
            if (GetSpace(board, space) == ReversiBoardSpace.EMPTY) return false;
            return true;
        }
        private ReversiBoardSpace PlayerToSpace(Player player)
        {
            return (player == Player.PlayerBlack) ? ReversiBoardSpace.BLACK : ReversiBoardSpace.WHITE;
        }

        private bool CheckDirection(Move move, Vector2 dir)
        {
            return CheckDirection(_model, move, dir);
        }
        private bool CheckDirection(ReversiBoard board, Move move, Vector2 dir)
        {
            Vector2 checkSpace = move._spacePos + dir;
            //Check to be sure new pos is on board
            if (!IsOnBoard(checkSpace)) return false;

            ReversiBoardSpace boardCheckSpace = GetSpace(board, checkSpace);
            //If check space is empty or same color then isn't valid
            if (boardCheckSpace == ReversiBoardSpace.EMPTY || boardCheckSpace == move._spaceType) return false;

            checkSpace += dir;
            while (IsOnBoard(checkSpace))
            {
                //Here we are looking for filled space of our own
                boardCheckSpace = GetSpace(board, checkSpace);
                //Another empty space means it isnt capped by our own piece
                if (boardCheckSpace == ReversiBoardSpace.EMPTY) return false;
                //If our color return true
                if (boardCheckSpace == move._spaceType) return true;
                //Keep going if opponents color
                checkSpace += dir;
            }
            //Ran out of board so isnt valid
            return false;
        }

        public bool IsValidMove(Move move)
        {
            return IsValidMove(_model, move);
        }
        public bool IsValidMove(ReversiBoard board, Move move)
        {
            if (move._none) return false;//Todo: Write test for this
            Vector2 space = move._spacePos;
            //Make sure it lies in board
            if (!IsOnBoard(space)) return false;
            //If occupied already it is not a valid move
            if (IsSpaceOccupied(board, space)) return false;
            //Check each direction to see if any make it a valid move, if any one is true then its valid
            if (CheckDirection(board, move, new Vector2(0, -1))) return true; //Up
            if (CheckDirection(board, move, new Vector2(1, -1))) return true; //UpRight
            if (CheckDirection(board, move, new Vector2(1, 0))) return true; //Right
            if (CheckDirection(board, move, new Vector2(1, 1))) return true; //DownRight
            if (CheckDirection(board, move, new Vector2(0, 1))) return true; //Down
            if (CheckDirection(board, move, new Vector2(-1, 1))) return true; //DownLeft
            if (CheckDirection(board, move, new Vector2(-1, 0))) return true; //Left
            if (CheckDirection(board, move, new Vector2(-1, -1))) return true; //UpLeft

            //No direction made this move a valid move
            return false;
        }
        public int Evaluate(Player player, int[,] boardWeights)
        {
            return Evaluate(_model, player, boardWeights);
        }
        public int Evaluate(ReversiBoard board, Player player, int[,] boardWeights)
        {
            //Todo: https://www.ultraboardgames.com/othello/tips.php
            int blackVal = 0, whiteVal = 0;

            if(boardWeights.GetLength(0) != Constants.REVERSI_BOARD_LENGTH && boardWeights.GetLength(1) != Constants.REVERSI_BOARD_LENGTH)
            {
                throw new IndexOutOfRangeException("Invalid Board Weights Size");
            }

            for(int i = 0; i < board.Spaces.GetLength(0); i++)
            {
                for (int j = 0; j < board.Spaces.GetLength(1); j++)
                {
                    if (board.Spaces[i, j] == ReversiBoardSpace.BLACK) blackVal += boardWeights[i,j];
                    if (board.Spaces[i, j] == ReversiBoardSpace.WHITE) whiteVal += boardWeights[i,j];
                }
            }

            return (player == Player.PlayerBlack) ? blackVal - whiteVal : whiteVal - blackVal;
        }

        public bool IsTerminal()
        {
            return IsTerminal(_model);
        }
        public bool IsTerminal(ReversiBoard board)
        {
            //Todo: What Happens if Black player cannot move but white player can???
            if (GetMoves(board, Player.PlayerBlack, false).Count == 0 && GetMoves(board, Player.PlayerWhite, false).Count == 0) return true;

            return false;
        }

        //========================

        public (int BestScore, Move BestMove) Minimax(ReversiBoard board, Player player, int maxDepth, int currentDepth, int alpha, int beta, ref int callNumber)
        {
            callNumber++;
            //If at end depth || in game over position
            if (IsTerminal(board) || currentDepth == maxDepth)
            {
                //Return the static evaluation of board based on player
                return (Evaluate(board, player, BoardWeights.CustomWeighting), new Move(false));
            }

            int currBestScore;
            Move currBestMove = new Move(false);//new Move(player, (player == Player.PlayerBlack) ? ReversiBoardSpace.BLACK : ReversiBoardSpace.WHITE, new Vector2(-1,-1));//What do I make this??

            currBestScore = (board.CurrentPlayer == player) ? int.MinValue : int.MaxValue;

            foreach (Move move in GetMoves(board, player, true))
            {
                int currentScore;
                Move currentMove = move;
                ReversiBoard newBoard = new ReversiBoard(board);
                MakeMove(newBoard, move);
                (currentScore, currentMove) = Minimax(newBoard, (player == Player.PlayerBlack) ? Player.PlayerWhite : Player.PlayerBlack, maxDepth, currentDepth + 1, alpha, beta, ref callNumber);

                if (board.CurrentPlayer == player)
                {
                    if (currentScore > currBestScore)
                    {
                        currBestScore = currentScore;
                        currBestMove = move;
                    }
                    alpha = Math.Max(alpha, currentScore);
                    if (beta <= alpha)
                        return (currentScore, currentMove);
                }
                else
                {
                    if (currentScore < currBestScore)
                    {
                        currBestScore = currentScore;
                        currBestMove = move;
                    }
                    beta = Math.Max(beta, currentScore);
                    if (beta <= alpha)
                        return (currentScore, currentMove);
                }
            }
            return (currBestScore, currBestMove);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;
                hash = (hash * 7) + (!Object.ReferenceEquals(null, _model) ? _model.GetHashCode() : 0);
                hash = (hash * 7) + (!Object.ReferenceEquals(null, _view) ? _view.GetHashCode() : 0);
                return hash;
            }
        }
    }
}
