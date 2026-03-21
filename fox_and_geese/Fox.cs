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

            // Лиса может ходить во всех направлениях: горизонталь, вертикаль, диагональ
            var allDirections = new[]
            {
                // Горизонтальные и вертикальные
                new Position(-1, 0), new Position(1, 0),
                new Position(0, -1), new Position(0, 1),
                // Диагональные
                new Position(-1, -1), new Position(-1, 1),
                new Position(1, -1), new Position(1, 1)
            };

            // Обычные ходы (на соседнюю клетку)
            foreach (var dir in allDirections)
            {
                var newPos = new Position(Position.X + dir.X, Position.Y + dir.Y);
                if (board.IsPositionValid(newPos) && board.GetPieceAt(newPos) == null)
                {
                    moves.Add(new Move(this, Position, newPos, null));
                }
            }

            // Ходы со съеданием гусей (прыжок через гуся)
            var captureMoves = GetCaptureMoves(board);
            moves.AddRange(captureMoves);

            return moves;
        }

        public List<Move> GetCaptureMoves(Board board)
        {
            var captures = new List<Move>();

            // Лиса может рубить во всех направлениях (как в шашках)
            var allDirections = new[]
            {
                // Горизонтальные и вертикальные
                new Position(-1, 0), new Position(1, 0),
                new Position(0, -1), new Position(0, 1),
                // Диагональные
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

        // Проверка, может ли лиса сделать еще один захват после текущего
        public bool HasAdditionalCaptures(Board board, Position currentPos)
        {
            var tempFox = new Fox(currentPos);
            return tempFox.GetCaptureMoves(board).Count > 0;
        }

        public override Piece Clone()
        {
            return new Fox(new Position(Position.X, Position.Y));
        }
    }
}
