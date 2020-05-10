using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace ReversiWebAPI.RequestObjects
{
    public class Vector2Request
    {
        public float X { get; }
        public float Y { get; }

        public Vector2Request(Vector2 request)
        {
            X = request.X;
            Y = request.Y;
        }
    }
}
