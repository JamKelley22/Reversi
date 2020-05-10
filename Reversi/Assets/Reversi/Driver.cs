using Reversi.Controller;
using Reversi.Data;
using Reversi.View;
using System;
using System.Collections.Generic;

namespace Reversi
{
    class Driver
    {
        static void Main(string[] args)
        {
            const bool DEBUG = false;
            bool aivai = false;
            Driver d = new Driver();
            ReversiBoardController controller = new ReversiBoardController(new ReversiBoardView());
            //controller = new ReversiBoardController("C:\\Users\\Jameel\\Desktop\\Spring20\\ComS437\\Minimax_Reversi\\Minimax_Reversi\\Data\\Board1.txt");

            int difficultyLevel1, difficultyLevel2;
            bool userMovesFirst;
            Player userPlayer, aiPlayer;
            if (DEBUG)
            {
                difficultyLevel1 = 7;
                difficultyLevel2 = 7;
                userMovesFirst = true;
            }
            else
            {
                controller.PrintTitle();
                controller.Clear();
                difficultyLevel1 = controller.GetDifficulty();
                difficultyLevel2 = 7;
                controller.Clear();
                userMovesFirst = controller.GetUserFirst();
                controller.Clear();
            }
            if (userMovesFirst)
            {
                userPlayer = Player.PlayerBlack;
                aiPlayer = Player.PlayerWhite;
            }
            else
            {
                aiPlayer = Player.PlayerBlack;
                userPlayer = Player.PlayerWhite;
            }

            controller.PrintBoard();


            Move userMove, aiBestMove;
            int aiBestScore;

            if (!userMovesFirst)
            {
                int numberOfMinimaxCalls = 0;
                controller.Clear();
                controller.PrintBoard();
                //(aiBestScore, aiBestMove) = controller.Minimax(controller.Board, aiPlayer, difficultyLevel, 0, ref numberOfMinimaxCalls);//Could just begin in same space everytime if default board is used
                (aiBestScore, aiBestMove) = controller.Minimax(controller.Board, aiPlayer, difficultyLevel1, 0, int.MinValue, int.MaxValue, ref numberOfMinimaxCalls);//Could just begin in same space everytime if default board is used
                controller.MakeMove(aiBestMove);
                controller.Clear();
                controller.PrintBoard();
                if (aiBestMove._none)
                    controller.Write("AI can not move");
                else
                    controller.Write(String.Format("AI Move: ({0},{1})", aiBestMove._spacePos.X + 1, aiBestMove._spacePos.Y + 1));
            }
            else
            {
                controller.Clear();
                controller.PrintBoard();
            }


            do
            {
                if(aivai)
                {
                    List<Move> useraiMoves = controller.GetMoves(userPlayer, true);

                    if (useraiMoves.Count > 0)
                    {
                        int numberOfMinimaxCalls = 0;
                        (aiBestScore, aiBestMove) = controller.Minimax(controller.Board, userPlayer, difficultyLevel2, 0, int.MinValue, int.MaxValue, ref numberOfMinimaxCalls);
                        controller.MakeMove(aiBestMove);
                        controller.Clear();
                        controller.PrintBoard();
                        controller.Write(String.Format("AI Move: ({0},{1}), #Minimax calls: {2}", aiBestMove._spacePos.X + 1, aiBestMove._spacePos.Y + 1, numberOfMinimaxCalls));
                    }
                    else
                    {
                        controller.Write("AI has no valid moves, your turn");
                    }
                }
                else
                {
                    List<Move> playerMoves = controller.GetMoves(userPlayer, false);
                    if (playerMoves.Count > 0)
                    {
                        bool validUserMove;
                        do
                        {
                            userMove = controller.GetUserMove(userPlayer);//May or may not be a valid move
                            validUserMove = controller.IsValidMove(userMove);
                            if (!validUserMove)
                            {
                                controller.ShowError(String.Format("Invalid Move pos({0},{1})", userMove._spacePos.X + 1, userMove._spacePos.Y + 1));
                            }
                        } while (!validUserMove);

                        controller.MakeMove(userMove);
                        controller.Clear();
                        controller.PrintBoard();
                    }
                    else
                    {
                        controller.Write("You have no valid moves, AI goes");
                    }
                }

                if (aivai)
                {
                    //Console.WriteLine("Waiting...");
                    //Make it wait for user input
                    //_ = Console.ReadLine();
                }

                List<Move> aiMoves = controller.GetMoves(aiPlayer, true);

                if(aiMoves.Count > 0)
                {
                    int numberOfMinimaxCalls = 0;
                    //(aiBestScore, aiBestMove) = controller.Minimax(controller.Board, aiPlayer, difficultyLevel, 0, ref numberOfMinimaxCalls);
                    (aiBestScore, aiBestMove) = controller.Minimax(controller.Board, aiPlayer, difficultyLevel1, 0, int.MinValue, int.MaxValue, ref numberOfMinimaxCalls);
                    controller.MakeMove(aiBestMove);
                    controller.Clear();
                    controller.PrintBoard();
                    controller.Write(String.Format("AI Move: ({0},{1}), #Minimax calls: {2}", aiBestMove._spacePos.X+1, aiBestMove._spacePos.Y+1, numberOfMinimaxCalls));
                }
                else
                {
                    controller.Write("AI has no valid moves, your turn");
                }

            } while (!controller.IsTerminal());

            controller.Clear();
            controller.PrintBoard();

            bool blackWins = controller.Evaluate(Player.PlayerBlack, BoardWeights.CustomWeighting) > 0;//Can a tie occur?
            controller.ShowWin((blackWins) ? Player.PlayerBlack : Player.PlayerWhite);
        }
        
    }
}
