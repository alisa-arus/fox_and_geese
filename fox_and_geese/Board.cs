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
            // три строки центральной горизонтали
            for (int row = 2; row <= 4; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    validPositions.Add(new Position(row, col));
                }
            }

            // три столбца центральной вертикали
            for (int col = 2; col <= 4; col++)
            {
                for (int row = 0; row < Size; row++)
                {
                    validPositions.Add(new Position(row, col));
                }
            }

            // удаляем дубликаты и углы
            var center = new Position(3, 3);
            var toRemove = validPositions.Where(pos => Math.Abs(pos.X - center.X) > 2 && Math.Abs(pos.Y - center.Y) > 2).ToList();

            foreach (var pos in toRemove)
            {
                validPositions.Remove(pos);
            }
        }

        private void InitializeBoard()
        {
            // в начале игры лиса помещается в центр
            var foxPos = new Position(3, 3);
            var fox = new Fox(foxPos);
            PlacePiece(fox, foxPos);

            // в начале игры все 13 гусей размещаются на трёх верхних рядах
            int geesePlaced = 0;
            int targetGeese = 13;

            // заполняем верхние три ряда игрового поля
            for (int row = 0; row < 3 && geesePlaced < targetGeese; row++)
            {
                for (int col = 0; col < Size && geesePlaced < targetGeese; col++)
                {
                    var pos = new Position(row, col);
                    // проверяем, что позиция доступна и не занята лисой
                    if (IsPositionValid(pos) && !pos.Equals(foxPos))
                    {
                        // проверяем, что на этой позиции еще нет фигуры
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
            return pos.X >= 0 && pos.X < Size && pos.Y >= 0 && pos.Y < Size && validPositions.Contains(pos);
        }

        public Fox GetFox()
        {
            return pieces.Values.OfType<Fox>().FirstOrDefault();
        }

        public List<Goose> GetGeese()
        {
            return pieces.Values.OfType<Goose>().ToList();
        }

        public int GetGeeseCount()
        {
            return GetGeese().Count;
        }
    }
}
