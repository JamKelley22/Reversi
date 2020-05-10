using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi.View
{
    public interface IReversiBoardView
    {
        void PrintTitle();
        Move GetUserMove(Player userPlayer);
        int GetDifficulty();
        bool GetUserFirst();
        void PrintBoard(ReversiBoard board);
        void Write(string val);
        void ShowWin(Player player);
        void ShowError(String val);
        void Clear();
    }
}
