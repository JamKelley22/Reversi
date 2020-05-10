using Reversi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiWebAPI.RequestObjects
{
    public class ReversiBoardSpaceRequest
    {
        public int SpaceIdentifier { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
    }
}
