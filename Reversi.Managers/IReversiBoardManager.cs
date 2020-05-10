using ReversiManagers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi.Interfaces.Managers
{
    public interface IReversiBoardManager
    {
        public int AddReversiBoardGame(ReversiBoardGame reversiBoardGame);
        public ReversiBoardGame GetReversiBoardGame(int id);
        Dictionary<int, ReversiBoardGame> GetAllReversiBoardGames();
    }
}
