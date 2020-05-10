using ReversiManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiWebAPI
{
    public sealed class GameManager
    {
        private static GameManager instance = null;
        private static readonly object padlock = new object();

        public Dictionary<int, ReversiBoardGame> ReversiBoardGameMap;

        GameManager()
        {
            ReversiBoardGameMap = new Dictionary<int, ReversiBoardGame>();
        }

        public static GameManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GameManager();
                    }
                    return instance;
                }
            }
        }
    }
}
