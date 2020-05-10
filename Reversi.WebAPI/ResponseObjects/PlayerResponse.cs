using Reversi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiWebAPI.ResponseObjects
{
    public class PlayerResponse
    {
        public int PlayerIdentifier { get; }
        public string PlayerName { get; }
        public PlayerResponse(Player player)
        {
            PlayerIdentifier = (int)player;
            PlayerName = Enum.GetName(typeof(Player), (int)player);
        }
    }
}
