// Copyright Andrei Cabungcal 2020 (C). All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAdventure.Core
{
    public struct Position
    {
        public int x, y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        //test if two positions are equal
        public override bool Equals(object obj)
        {
            if(obj.GetType() != typeof(Position)) return false;
            Position pos = (Position)obj;
            return x == pos.x && y == pos.y;
        }

        public static bool operator ==(Position lhs, Position rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Position lhs, Position rhs)
        {
            return !lhs.Equals(rhs);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
