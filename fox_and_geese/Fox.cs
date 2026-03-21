using System.Collections.Generic;


namespace fox_and_geese
{
    public class Fox : Piece
    {
        public Fox(Position position) : base(PlayerType.Fox, position)
        {
        }

        public override List<Move> GetValidMoves(Board board)
        {
            var moves = new List<Move>();

            // В классической "Лисе и гусях" лиса ходит по диагоналям
            var diagonalDirections = new[]
            {
                new Position(-1, -1), new Position(-1, 1),
                new Position(1, -1), new Position(1, 1)
            };

            // Обычные ходы
            foreach (var dir in diagonalDirections)
            {
                var newPos = new Position(Position.X + dir.X, Position.Y + dir.Y);
                if (board.IsPositionValid(newPos) && board.GetPieceAt(newPos) == null)
                {
                    moves.Add(new Move(this, Position, newPos, null));
                }
            }

            // Ходы со съеданием гусей
            var captureMoves = GetCaptureMoves(board);
            moves.AddRange(captureMoves);

            return moves;
        }

        public List<Move> GetCaptureMoves(Board board)
        {
            var captures = new List<Move>();
            var diagonalDirections = new[]
            {
                new Position(-1, -1), new Position(-1, 1),
                new Position(1, -1), new Position(1, 1)
            };

            foreach (var dir in diagonalDirections)
            {
                var adjacentPos = new Position(Position.X + dir.X, Position.Y + dir.Y);
                var targetPos = new Position(Position.X + dir.X * 2, Position.Y + dir.Y * 2);

                var adjacentPiece = board.GetPieceAt(adjacentPos);
                if (adjacentPiece != null && adjacentPiece.Type == PlayerType.Goose &&
                    board.IsPositionValid(targetPos) && board.GetPieceAt(targetPos) == null)
                {
                    captures.Add(new Move(this, Position, targetPos, adjacentPiece));
                }
            }

            return captures;
        }

        public override Piece Clone()
        {
            return new Fox(new Position(Position.X, Position.Y));
        }
    }
}
