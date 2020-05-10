using Reversi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace ReversiWebAPI.ResponseObjects
{
    public class MoveResponse
    {
        public ReversiBoardSpaceResponse Space { get; set; }
        public bool Success { get; set; }

        public MoveResponse(Move move, bool success)
        {
            Space = new ReversiBoardSpaceResponse(move._spaceType, (int)move._spacePos.X, (int)move._spacePos.Y);
            Success = success;
        }
    }
}
