using Reversi;
using Reversi.Interfaces.Managers;
using ReversiWebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReversiManagers
{
    class ReversiBoardManager : IReversiBoardManager
    {
        

        public ReversiBoardManager()
        {
            
        }

        public int AddReversiBoardGame(ReversiBoardGame reversiBoardGame)
        {
            int hash = reversiBoardGame.ReversiBoardController.GetHashCode();
            GameManager.Instance.ReversiBoardGameMap.Add(hash, reversiBoardGame);
            return hash;
        }

        public Dictionary<int, ReversiBoardGame> GetAllReversiBoardGames()
        {
            return GameManager.Instance.ReversiBoardGameMap;
        }

        public ReversiBoardGame GetReversiBoardGame(int id)
        {
            ReversiBoardGame reversiBoardGame = null;
            GameManager.Instance.ReversiBoardGameMap.TryGetValue(id, out reversiBoardGame);
            return reversiBoardGame;
        }
    }
}
