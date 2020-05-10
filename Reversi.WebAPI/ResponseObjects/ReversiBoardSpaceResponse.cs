using Reversi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiWebAPI.ResponseObjects
{
    public class ReversiBoardSpaceResponse
    {
        public int SpaceIdentifier { get; }
        public string SpaceName { get; }
        public int Row { get; }
        public int Col { get; }

        public ReversiBoardSpaceResponse(ReversiBoardSpace space, int row, int col)
        {
            SpaceIdentifier = (int)space;
            SpaceName = Enum.GetName(typeof(ReversiBoardSpace), (int)space);
            Row = row;
            Col = col;
        }
    }
}
