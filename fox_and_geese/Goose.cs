using System.Collections.Generic;


namespace fox_and_geese
{
    public class Goose : Piece
    {
        public Goose(Position position) : base(PlayerType.Goose, position)
        {
        }

        public override List<Move> GetValidMoves(Board board)
        {
            var moves = new List<Move>();
            // Гуси ходят только вперед (вверх по доске)
            var directions = new[]
            {
                new Position(-1, -1), new Position(-1, 0), new Position(-1, 1)
            };

            foreach (var dir in directions)
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
