using Reversi;
using Reversi.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReversiManagers
{
    public class ReversiBoardGame
    {
        //public ReversiBoard ReversiBoard { get; set; }
        public ReversiBoardController ReversiBoardController { get; set; }
        public int DifficulityLevel { get; set; }
        public bool UserGoesFirst { get; set; }
    }
}
