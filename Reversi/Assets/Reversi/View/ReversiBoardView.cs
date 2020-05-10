using System;
using System.Numerics;

namespace Reversi.View
{
    public class ReversiBoardView: IReversiBoardView
    {
        public void PrintTitle()
        {
            Console.Clear();
            Console.WriteLine("=====Reversi=====");
            Console.WriteLine("Implemented by Jameel Kelley");
            Console.WriteLine("");
            Console.WriteLine("Press Enter key to start");
            Console.ReadLine();
        }
        public Move GetUserMove(Player userPlayer)
        {
            bool isValidInput = false;//Assume bad input
            Move userMove = new Move(false);
            do
            {
                Console.Write("Enter Move (comma seperated): ");
                string line = Console.ReadLine();
                string[] parts = line.Split(',');

                if (parts.Length != 2)
                {
                    Console.WriteLine("Invalid Input, ex. 1,1: " + line);
                    continue;
                }

                int j, i;
                bool parseSuccessX = int.TryParse(parts[0], out j);
                bool parseSuccessY = int.TryParse(parts[1], out i);
                if (!parseSuccessX || !parseSuccessY)
                {
                    Console.WriteLine("Invalid Input (cannot parse ints): " + line);
                    continue;
                }
                if (j < 1 || j > 8 || i < 1 || i > 8)
                {
                    Console.WriteLine("Invalid Input (Out of Board Range): " + line);
                    continue;
                }

                isValidInput = true;//User input was good if we get here
                Vector2 space = new Vector2(i - 1, j - 1);//Subtract 1 from each since board is 0 indexed
                ReversiBoardSpace userType = (userPlayer == Player.PlayerBlack) ? ReversiBoardSpace.BLACK : ReversiBoardSpace.WHITE;
                userMove = new Move(userType, space);

            } while (!isValidInput);
            return userMove;
        }

        public int GetDifficulty()
        {
            string val;
            int difficultyLevel;
            bool validValue;
            do
            {
                Console.Write("Difficulty Level (1-10): ");
                val = Console.ReadLine();
                bool parseSuccess = int.TryParse(val, out difficultyLevel);
                validValue = parseSuccess && difficultyLevel > 0 && difficultyLevel < 11;
                if (!validValue)
                {
                    Console.WriteLine("Invalid Value: " + val);
                }
            } while (!validValue);
            return difficultyLevel;
        }

        public bool GetUserFirst()
        {
            string val;
            bool validValue;
            do
            {
                Console.Write("Would you like to go first? (y/N): ");
                val = Console.ReadLine().ToUpper();
                validValue = val == "Y" || val == "N";
                if (!validValue)
                {
                    Console.WriteLine("Invalid Response: " + val);
                }
            } while (!validValue);
            return val == "Y";
        }
        public void PrintBoard(ReversiBoard board)
        {
            ReversiBoardSpace[,] reversiBoardSpaces = board.Spaces;
            Console.Write("   ");
            for (int i = 0; i < reversiBoardSpaces.GetLength(1); i++)
            {
                Console.Write((i + 1) + " ");
            }
            Console.WriteLine();
            for (int i = 0; i < reversiBoardSpaces.GetLength(0); i++)
            {
                Console.Write((i + 1) + " ");
                for (int j = 0; j < reversiBoardSpaces.GetLength(1); j++)
                {
                    string space = "";
                    switch (reversiBoardSpaces[i, j])
                    {
                        case ReversiBoardSpace.BLACK:
                            space = "B";
                            break;
                        case ReversiBoardSpace.WHITE:
                            space = "W";
                            break;
                        case ReversiBoardSpace.EMPTY:
                            space = "-";
                            break;
                    }
                    Console.Write("|" + space);
                }
                Console.WriteLine("|");
            }
        }

        public void Write(string val)
        {
            Console.WriteLine(val);
        }

        public void ShowWin(Player player)
        {
            Console.WriteLine(String.Format("Congratulations {0}", Enum.GetName(typeof(Player), player)));
        }

        public void ShowError(String val)
        {
            Console.WriteLine(val);
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}
