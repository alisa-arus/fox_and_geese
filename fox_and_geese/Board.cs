using System;
using System.Collections.Generic;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;


namespace fox_and_geese
{
    public class Board
    {
        private Dictionary<Position, Piece> pieces;
        public int Size { get; }
        private HashSet<Position> validPositions;

        public Board(int size = 7)
        {
            Size = size;
            pieces = new Dictionary<Position, Piece>();
            validPositions = new HashSet<Position>();
            InitializeValidPositions();
            InitializeBoard();
        }

        private void InitializeValidPositions()
        {
            // Создаем поле в виде креста 7x7
            // Доступны все клетки, кроме углов (0,0), (0,6), (6,0), (6,6)
            // и клеток, которые не входят в крест

            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    // Центральная горизонталь (3-я строка) - вся доступна
                    if (row == 3)
                    {
                        validPositions.Add(new Position(row, col));
                    }
                    // Центральная вертикаль (3-й столбец) - вся доступна
                    else if (col == 3)
                    {
                        validPositions.Add(new Position(row, col));
                    }
                    // Углы и некоторые другие клетки недоступны
                    else if ((row == 0 || row == 6) && (col == 0 || col == 6))
                    {
                        // Углы недоступны
                        continue;
                    }
                    else if (Math.Abs(row - 3) + Math.Abs(col - 3) <= 3)
                    {
                        // Добавляем клетки, находящиеся в радиусе 3 от центра (манхэттенское расстояние)
                        validPositions.Add(new Position(row, col));
                    }
                }
            }
        }

        private void InitializeBoard()
        {
            // Размещаем лису в центре
            var foxPos = new Position(3, 3);
            var fox = new Fox(foxPos);
            PlacePiece(fox, foxPos);

            // Размещаем гусей в верхней части креста
            // Гуси располагаются на всех доступных клетках выше центральной горизонтали
            int gooseCount = 0;
            foreach (var pos in validPositions)
            {
                if (pos.X < 3 && !pos.Equals(foxPos))
                {
                    var goose = new Goose(pos);
                    PlacePiece(goose, pos);
                    gooseCount++;
                    if (gooseCount >= 13) break; // Обычно 13 гусей в классической версии
                }
            }

            // Добавляем еще гусей по бокам если нужно
            if (gooseCount < 13)
            {
                foreach (var pos in validPositions)
                {
                    if (pos.X == 3 && Math.Abs(pos.Y - 3) > 1 && !pos.Equals(foxPos) && gooseCount < 13)
                    {
                        var goose = new Goose(pos);
                        PlacePiece(goose, pos);
                        gooseCount++;
                    }
                }
            }
        }

        public void PlacePiece(Piece piece, Position pos)
        {
            if (IsPositionValid(pos))
            {
                pieces[pos] = piece;
                piece.Position = pos;
            }
        }

        public void RemovePiece(Position pos)
        {
            pieces.Remove(pos);
        }

        public Piece GetPieceAt(Position pos)
        {
            pieces.TryGetValue(pos, out var piece);
            return piece;
        }

        public bool IsPositionValid(Position pos)
        {
            return pos.X >= 0 && pos.X < Size &&
                   pos.Y >= 0 && pos.Y < Size &&
                   validPositions.Contains(pos);
        }

        public Fox GetFox()
        {
            return pieces.Values.OfType<Fox>().FirstOrDefault();
        }

        public List<Goose> GetGeese()
        {
            return pieces.Values.OfType<Goose>().ToList();
        }

        public Board Clone()
        {
            var newBoard = new Board(Size);
            newBoard.pieces.Clear();

            foreach (var piece in pieces.Values)
            {
                var clonedPiece = piece.Clone();
                newBoard.PlacePiece(clonedPiece, clonedPiece.Position);
            }

            return newBoard;
        }

        public List<Position> GetValidPositions()
        {
            return validPositions.ToList();
        }
    }
}