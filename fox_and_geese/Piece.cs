using System.Collections.Generic;


namespace fox_and_geese
{
    public abstract class Piece
    {
        public PlayerType Type { get; protected set; }
        public Position Position { get; set; }

        protected Piece(PlayerType type, Position position)
        {
            Type = type;
            Position = position;
        }

        public abstract List<Move> GetValidMoves(Board board);
        public abstract Piece Clone();
    }
}
