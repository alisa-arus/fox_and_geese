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

            // лиса может ходить во всех направлениях: горизонталь, вертикаль, диагональ
            var allDirections = new[]
            {
                // ходы по горизонтали и вертикали
                new Position(-1, 0), new Position(1, 0),
                new Position(0, -1), new Position(0, 1),
                // ходы по диагонали
                new Position(-1, -1), new Position(-1, 1),
                new Position(1, -1), new Position(1, 1)
            };
            // обычные ходы (на соседнюю клетку)
            foreach (var dir in allDirections)
            {
                var newPos = new Position(Position.X + dir.X, Position.Y + dir.Y);
                if (board.IsPositionValid(newPos) && board.GetPieceAt(newPos) == null)
                {
                    moves.Add(new Move(this, Position, newPos, null));
                }
            }
            // ходы рубки (прыжок через гуся)
            var captureMoves = GetCaptureMoves(board);
            moves.AddRange(captureMoves);

            return moves;
        }

        public List<Move> GetCaptureMoves(Board board)
        {
            var captures = new List<Move>();

            // лиса может рубить во всех направлениях
            var allDirections = new[]
            {
                // рубка по горизонтали и по вертикали
                new Position(-1, 0), new Position(1, 0),
                new Position(0, -1), new Position(0, 1),
                // диагональная рубка (уточнить в правилах)
                //new Position(-1, -1), new Position(-1, 1),
                //new Position(1, -1), new Position(1, 1)
            };

            foreach (var dir in allDirections)
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
