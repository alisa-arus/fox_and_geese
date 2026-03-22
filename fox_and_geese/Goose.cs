using System.Collections.Generic;


namespace fox_and_geese
{
    public class Goose : Piece
    {
        public Goose(Position position) : base(PlayerType.Goose, position) {}

        public override List<Move> GetValidMoves(Board board)
        {
            var moves = new List<Move>();

            // ходы гуся (только по горизонтали и вертикали)
            var orthogonalDirections = new[]
            {
                new Position(-1, 0), new Position(1, 0),    // для вертикали
                new Position(0, -1), new Position(0, 1)     // для горизонтали
            };

            foreach (var dir in orthogonalDirections)
            {
                var newPos = new Position(Position.X + dir.X, Position.Y + dir.Y);
                if (board.IsPositionValid(newPos) && board.GetPieceAt(newPos) == null)
                {
                    moves.Add(new Move(this, Position, newPos, null));
                }
            }

            return moves;
        }

        public override Piece Clone()
        {
            return new Goose(new Position(Position.X, Position.Y));
        }
    }
}
