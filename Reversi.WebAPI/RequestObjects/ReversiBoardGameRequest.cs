using Reversi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiWebAPI.RequestObjects
{
    public class ReversiBoardGameRequest
    {
        public ReversiBoardRequest ReversiBoard { get; set; }
        public int DifficulityLevel { get; set; }
        public bool UserGoesFirst { get; set; }
    }
}
