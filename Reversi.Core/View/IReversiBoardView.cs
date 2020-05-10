using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi.View
{
    public interface IReversiBoardView
    {
        public void PrintTitle();
        public Move GetUserMove(Player userPlayer);
        public int GetDifficulty();
        public bool GetUserFirst();
        public void PrintBoard(ReversiBoard board);
        public void Write(string val);
        public void ShowWin(Player player);
        public void ShowError(String val);
        public void Clear();
    }
}
