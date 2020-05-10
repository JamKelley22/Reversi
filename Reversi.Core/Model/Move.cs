using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Reversi
{
    public class Move
    {
        public ReversiBoardSpace _spaceType;
        public Vector2 _spacePos;
        public bool _none;

        public Move(ReversiBoardSpace spaceType, Vector2 spacePos)
        {
            _spaceType = spaceType;
            _spacePos = spacePos;
        }

        public Move(bool empty)
        {
            _none = true;
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Move move = (Move)obj;
                return 
                    (move._spaceType == _spaceType) &&
                    (move._spacePos == _spacePos) &&
                    (move._none == _none);
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;
                hash = (hash * 7) + (!Object.ReferenceEquals(null, _spaceType) ? _spaceType.GetHashCode() : 0);
                hash = (hash * 7) + (!Object.ReferenceEquals(null, _none) ? _none.GetHashCode() : 0);
                hash = (hash * 7) + (!Object.ReferenceEquals(null, _spacePos) ? _spacePos.GetHashCode() : 0);
                return hash;
            }
        }

        public override string ToString()
        {
            if (_none) return "Move: NONE";
            return String.Format("Move: type({0}), pos({1},{2})", Enum.GetName(typeof(ReversiBoardSpace),_spaceType), _spacePos.X, _spacePos.Y);
        }
    }
}
