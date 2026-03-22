using System;
using System.Collections.Generic;
using System.Linq;


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
            // Центральная горизонталь (строки 2,3,4)
            for (int row = 2; row <= 4; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    validPositions.Add(new Position(row, col));
                }
            }

            // Центральная вертикаль (столбцы 2,3,4)
            for (int col = 2; col <= 4; col++)
            {
                for (int row = 0; row < Size; row++)
                {
                    validPositions.Add(new Position(row, col));
                }
            }

            // Удаляем дубликаты и углы
            var center = new Position(3, 3);
            var toRemove = validPositions
                .Where(pos => Math.Abs(pos.X - center.X) > 2 && Math.Abs(pos.Y - center.Y) > 2)
                .ToList();

            foreach (var pos in toRemove)
            {
                validPositions.Remove(pos);
            }
        }

        private void InitializeBoard()
        {
            // Размещаем лису в центре
            var foxPos = new Position(3, 3);
            var fox = new Fox(foxPos);
            PlacePiece(fox, foxPos);

            // Размещаем 13 гусей на трёх верхних рядах
            int geesePlaced = 0;
            int targetGeese = 13;

            // Сначала заполняем верхние три ряда (строки 0, 1, 2)
            for (int row = 0; row < 3 && geesePlaced < targetGeese; row++)
            {
                for (int col = 0; col < Size && geesePlaced < targetGeese; col++)
                {
                    var pos = new Position(row, col);
                    // Проверяем, что позиция доступна и не занята лисой
                    if (IsPositionValid(pos) && !pos.Equals(foxPos))
                    {
                        // Проверяем, что на этой позиции еще нет фигуры
                        if (GetPieceAt(pos) == null)
                        {
                            var goose = new Goose(pos);
                            PlacePiece(goose, pos);
                            geesePlaced++;
                        }
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

        public int GetGeeseCount()
        {
            return GetGeese().Count;
        }

        public bool IsCenterPosition(Position pos)
        {
            return pos.X >= 2 && pos.X <= 4 && pos.Y >= 2 && pos.Y <= 4;
        }

        public int GetGeeseOnTopRows()
        {
            return GetGeese().Count(g => g.Position.X < 3);
        }
    }
}