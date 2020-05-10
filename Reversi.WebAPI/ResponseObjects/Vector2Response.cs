using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace ReversiWebAPI.ResponseObjects
{
    public class Vector2Response
    {
        public float X { get; }
        public float Y { get; }

        public Vector2Response(Vector2 response)
        {
            X = response.X;
            Y = response.Y;
        }
    }
}
