using System;


namespace fox_and_geese
{
    public class Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Position other)
                return X == other.X && Y == other.Y;
            return false;
        }

        public override int GetHashCode()
        {
            return (X << 16) ^ Y;
        }

        public bool IsAdjacent(Position other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y) == 1;
        }

        public bool IsDiagonal(Position other)
        {
            return Math.Abs(X - other.X) == 1 && Math.Abs(Y - other.Y) == 1;
        }
    }
}
