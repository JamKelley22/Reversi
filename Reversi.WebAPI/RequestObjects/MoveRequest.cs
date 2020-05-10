using Reversi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace ReversiWebAPI.RequestObjects
{
    public class MoveRequest
    {
        public ReversiBoardSpaceRequest Space { get; set; }
        public bool None { get; set; }
    }
}
